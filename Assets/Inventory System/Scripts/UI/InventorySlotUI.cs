using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Tools.Inventory.UI
{
    public class InventorySlotUI : MonoBehaviour
    {
        public Item Item => _item;

        [SerializeField]
        private TextMeshProUGUI itemNameText;
        [SerializeField]
        private TextMeshProUGUI itemAmountText;
        [SerializeField]
        private Button removeItemButton;
        [SerializeField]
        private Button reduceItemButton;

        private Item _item;
        private Action<InventorySlotUI> _onRemoveItem;
        private Action<InventorySlotUI, int> _onReduceItem;

        private void Awake()
        {
            removeItemButton.onClick.AddListener(OnRemoveItem);
            reduceItemButton.onClick.AddListener(OnReduceItem);
        }

        public void SetSlot(Item item)
        {
            _item = item;
            SetItemName(item.Data.Name);
            SetItemAmount(item.Quantity);
        }

        public void AddOnRemoveItemListener(Action<InventorySlotUI> onRemoveItem)
        {
            _onRemoveItem = onRemoveItem;
        }

        public void AddOnReduceItemListener(Action<InventorySlotUI, int> callback)
        {
            _onReduceItem = callback;
        }

        public void SetItemName(string itemName)
        {
            itemNameText.text = itemName;
        }

        public void SetItemAmount(int amount)
        {
            itemAmountText.text = amount.ToString();
        }

        public void SetActive(bool value)
        {
            gameObject.SetActive(value);

            if (value == false)
            {
                _item = null;
                _onRemoveItem = null;
                _onReduceItem = null;
            }
        }

        private void OnRemoveItem()
        {
            _onRemoveItem?.Invoke(this);
        }

        private void OnReduceItem()
        {
            _onReduceItem?.Invoke(this, 1);
        }
    }
}