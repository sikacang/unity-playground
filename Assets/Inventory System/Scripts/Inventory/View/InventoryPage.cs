using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Tools.Inventory
{
    public class InventoryPage : MonoBehaviour
    {
        [SerializeField]
        private Transform itemSlotContainer;
        [SerializeField]
        private ItemSlotUI itemSlotUI;

        [Header("Information Panel")]
        public GameObject informationPanel;
        public TextMeshProUGUI itemNameText;
        public TextMeshProUGUI itemDescriptionText;
        public Image itemIconImage;
        public TextMeshProUGUI itemAmountText;

        [Header("Methods")]
        public Button removeButton;
        public Button reduceButton;
        public TMP_InputField reduceAmountField;

        public UnityEvent<Item, int> OnReduced = new();
        public UnityEvent<Item> OnRemoved = new();

        private ItemSlotUI _currentSelectedSlot;

        private void Start()
        {
            removeButton.onClick.AddListener(OnRemoveItem);
            reduceButton.onClick.AddListener(OnReduceItem);
        }

        private void OnReduceItem()
        {
            if (_currentSelectedSlot == null)
                return;
            
            int amount = int.TryParse(reduceAmountField.text, out amount) ? amount : 1;

            OnReduced?.Invoke(_currentSelectedSlot.Item, amount);
        }

        private void OnRemoveItem()
        {
            if(_currentSelectedSlot == null)
                return;

            OnRemoved?.Invoke(_currentSelectedSlot.Item);
        }

        public void PopulateInventory(List<Item> items)
        {
            DestroySlot();

            foreach (var item in items)
            {
                var slot = GetSlot();
                slot.SetItem(item);
                slot.OnClick.AddListener(OnSelected);
            }

            HideItemInformation();
        }        

        public void OnPageClosed()
        {
            DestroySlot();
            HideItemInformation();            
        }

        private void OnSelected(ItemSlotUI slot)
        {
            _currentSelectedSlot?.SelectItem(false);
            _currentSelectedSlot = slot;
            _currentSelectedSlot.SelectItem(true);

            ShowItemInformation();
        }

        private void ShowItemInformation()
        {
            if (_currentSelectedSlot == null)
                return;

            var item = _currentSelectedSlot.Item;
            itemNameText.text = item.Data.Name;
            itemDescriptionText.text = item.Data.Description;
            itemIconImage.sprite = item.Data.Icon;
            itemAmountText.text = item.Quantity.ToString();

            informationPanel.SetActive(true);
        }

        private void HideItemInformation()
        {
            informationPanel.SetActive(false);
        }

        private ItemSlotUI GetSlot()
        {
            var slot = Instantiate(itemSlotUI, itemSlotContainer);
            return slot;
        }

        private void DestroySlot()
        {
            _currentSelectedSlot = null;

            foreach (Transform child in itemSlotContainer)
            {
                Destroy(child.gameObject);
            }
        }
    }
}