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
        [SerializeField]
        private TextMeshProUGUI itemNameText;
        [SerializeField]
        private TextMeshProUGUI itemAmountText;
        [SerializeField]
        private Button removeItemButton;

        private InventoryItem _item;
        private Action<InventorySlotUI> _onRemoveItem;

        private void Awake()
        {
            removeItemButton.onClick.AddListener(OnRemoveItem);
        }

        public void SetSlot(InventoryItem item)
        {
            _item = item;
            SetItemName(item.Data.Name);
            SetItemAmount(item.Quantity);
        }

        public void SetOnRemoveItem(Action<InventorySlotUI> onRemoveItem)
        {
            _onRemoveItem = onRemoveItem;
        }

        public void SetItemName(string itemName)
        {
            itemNameText.text = itemName;
        }

        public void SetItemAmount(int amount)
        {
            itemAmountText.text = amount.ToString();
        }

        private void OnRemoveItem()
        {
            _onRemoveItem?.Invoke(this);
        }
    }
}