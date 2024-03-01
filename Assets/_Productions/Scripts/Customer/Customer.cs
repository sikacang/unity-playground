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
        public Order Order => _customerOrder;

        [ShowInInspector,ReadOnly]
        private Order _customerOrder;

        [SerializeField,ReadOnly]
        [InfoBox("Order Page ID")]
        private EnumId orderPageId;

        [Header("Customer Component")]
        [SerializeField,ReadOnly]
        private BoxCollider2D boxCollider;

        public UnityEvent<Order> OnOrderModified = new();
        public UnityEvent OnOrderCompleted = new();

        [Header("Debug")]
        [SerializeField]
        private Order debugOrder;

        private void Start()
        {
            SetOrder(debugOrder);
        }

        public void SetOrder(Order order)
        {
            _customerOrder = order;
            OnOrderModified.Invoke(_customerOrder);
        }

        public bool CanDeliverItem(InventoryItem item, out int orderAmount)
        {
            var orderItem = _customerOrder.Items.Find(x => x.Item == item.Data);
            orderAmount = orderItem != null ? orderItem.Quantity : 0;
            return _customerOrder.Items.Any(x => x.Item == item.Data && x.Quantity > 0);        
        }

        public void DeliverItem(ItemData data, int quantity)
        {
            var orderItem = _customerOrder.Items.Find(x => x.Item == data);
            if (orderItem != null)
            {
                orderItem.Quantity -= quantity;
                OnOrderModified.Invoke(_customerOrder);
            }

            if (_customerOrder.Items.All(x => x.Quantity == 0))
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