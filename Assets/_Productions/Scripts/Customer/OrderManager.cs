using Core;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using Tools.Inventory;
using UnityEngine;
using UnityEngine.Events;

namespace Waroeng
{
    public class OrderManager : SceneService
    {
        [Title("Properties")]
        [SerializeField]
        private List<Order> orders;
        [SerializeField]
        private int maxOrder = 3;

        [Title("Events")]
        public UnityEvent<Order> OnOrderAdded;
        public UnityEvent<Order> OnOrderRemoved;
        public UnityEvent<List<Order>> OnOrderChanged;

        public void AddNewOrder(Order order)
        {
            if (orders.Count < maxOrder)
            {
                orders.Add(order);
                OnOrderAdded.Invoke(order);
                OnOrderChanged.Invoke(orders);
            }            
        }

        public void RemoveOrder(Order order)
        {
            if (orders.Contains(order))
            {
                orders.Remove(order);
                OnOrderRemoved.Invoke(order);
                OnOrderChanged.Invoke(orders);
            }
        }

        public void DeliverItem(InventoryItem item)
        {

        }

        public OrderItem[] debugItems;

        [Button]
        public void DebugOrder()
        {
            var order = new Order();
            order.Items = new OrderItem[debugItems.Length];
            for (int i = 0; i < debugItems.Length; i++)
            {
                order.Items[i] = new OrderItem
                {
                    Item = debugItems[i].Item,
                    Quantity = debugItems[i].Quantity
                };
            }

            AddNewOrder(order);
        }

        [Button]
        public void DebugRemoveOrder(int index)
        {
            var order = orders[index];
            if (order != null)
            {
                RemoveOrder(order);
            }
        }
    }    
}