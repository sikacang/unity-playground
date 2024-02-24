using Core;
using Core.UI;
using CustomExtensions;
using Sirenix.OdinInspector;
using System.Linq;
using Tools.Interaction;
using Tools.Inventory;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.Events;

namespace Waroeng
{
    public class Customer : MonoBehaviour
    {
        public Order Order => customerOrder;

        [SerializeField]
        private Order customerOrder;

        [SerializeField]
        [InfoBox("Order Page ID")]
        private EnumId orderPageId;

        [Header("Customer Component")]
        [SerializeField]
        private BoxCollider2D boxCollider;

        public UnityEvent<Order> OnOrderModified = new();
        public UnityEvent OnOrderCompleted = new();

        public bool CanDeliverItem(InventoryItem item, out int orderAmount)
        {
            var orderItem = customerOrder.Items.Find(x => x.Item == item.Data);
            orderAmount = orderItem != null ? orderItem.Quantity : 0;
            return customerOrder.Items.Any(x => x.Item == item.Data && x.Quantity > 0);        
        }

        public void DeliverItem(ItemData data, int quantity)
        {
            var orderItem = customerOrder.Items.Find(x => x.Item == data);
            if (orderItem != null)
            {
                orderItem.Quantity -= quantity;
                OnOrderModified.Invoke(customerOrder);
            }

            if (customerOrder.Items.All(x => x.Quantity == 0))
            {
                CompleteOrder();
            }
        }

        public void OpenOrderView(Interactor interactor)
        {
            var inventory = interactor.GetComponent<LimitedInventory>();
            if (inventory == null)
                return;

            var sceneUI = SceneServiceProvider.GetService<SceneUI>();
            if(sceneUI == null)
                return;

            var pageData = new PageData();
            pageData.Add("player_inventory", inventory);
            pageData.Add("customer", this);

            sceneUI.PushPage(orderPageId, pageData);
        }
    
        private void CompleteOrder()
        {
            OnOrderCompleted?.Invoke();
        }
    }
}