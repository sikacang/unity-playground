using Core;
using Lean.Pool;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Waroeng
{
    public class OrderUI : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField]
        private OrderItemUI orderItemPrefab;
        [SerializeField]
        private Transform orderListContainer;
        [SerializeField]
        private List<OrderItemUI> orderItems;

        private OrderManager _orderManager;

        private void Awake()
        {
            _orderManager = SceneServiceProvider.GetService<OrderManager>();
        }

        private void OnEnable()
        {
            _orderManager.OnOrderAdded.AddListener(OnOrderAdded);
            _orderManager.OnOrderRemoved.AddListener(OnOrderRemoved);
        }

        private void OnDisable()
        {
            _orderManager.OnOrderAdded.RemoveListener(OnOrderAdded);
            _orderManager.OnOrderRemoved.RemoveListener(OnOrderRemoved);
        }

        private void OnOrderAdded(Order order)
        {
            var orderItemUI = LeanPool.Spawn(orderItemPrefab, orderListContainer);
            orderItemUI.SetOrder(order);
            orderItems.Add(orderItemUI);
        }

        private void OnOrderRemoved(Order order)
        {
            var orderItemUI = orderItems.Find(x => x.Order == order);
            if (orderItemUI != null)
            {
                LeanPool.Despawn(orderItemUI);
                orderItems.Remove(orderItemUI);
            }
        }
    }

}