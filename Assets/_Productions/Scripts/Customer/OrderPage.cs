using Core.UI;
using Sirenix.OdinInspector;
using System;
using Tools.Inventory;
using UnityEngine;

namespace Waroeng
{
    public class OrderPage : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField]
        private ShelfUI playerShelf;
        [SerializeField]
        private OrderSlotUI orderSlotUI;

        [ShowInInspector]
        private LimitedInventory _playerInventory;
        [ShowInInspector]
        private Customer _customer;

        private UIPage _uiPage;

        private void Awake()
        {
            _uiPage = GetComponent<UIPage>();
            _uiPage.OnPushed.AddListener(SetupOrderView);
            _uiPage.OnClose.AddListener(OnPageClosed);
        }        

        private void Start()
        {
            playerShelf.OnPressedSlot.AddListener((slot) =>
            {
                TryDeliverOrder(slot.Item);
            });
        }

        private void SetupOrderView(PageData data)
        {
            if (data.TryGet<LimitedInventory>("player_inventory", out var inventory) == false)
                return;

            if(data.TryGet<Customer>("customer", out var customer) == false)
                return;

            _playerInventory = inventory;
            _customer = customer;
            _customer.OnOrderCompleted.AddListener(CompleteOrder);

            playerShelf.SetInventory(inventory);
            orderSlotUI.SetOrder(customer.Order);
        }

        private void OnPageClosed()
        {
            if(_customer != null)
                _customer.OnOrderCompleted.RemoveListener(CompleteOrder);
        }

        private bool TryDeliverOrder(InventoryItem item)
        {
            if (_customer.CanDeliverItem(item, out int orderAmount) == false)
                return false;

            int reduceAmount = Mathf.Min(orderAmount, item.Quantity);
            _customer.DeliverItem(item.Data, reduceAmount);
            _playerInventory.ReduceItem(item, reduceAmount);
            orderSlotUI.RefreshSlots();

            return true;
        }

        private void CompleteOrder()
        {
            _uiPage.Return();
        }
    }
}
