using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static UnityEditor.Progress;

namespace Tools.Inventory.UI
{
    public class GridInventorySlotUI : MonoBehaviour
    {
        public Item Item => _item;

        [Header("Slot Properties")]
        [SerializeField]
        private Image iconImage;
        [SerializeField]
        private TextMeshProUGUI amountText;
        [SerializeField]
        private Button slotButton;

        [SerializeField]
        private GameObject fillContainer;

        // Events
        public UnityEvent<GridInventorySlotUI> OnSelectSlot;

        private Item _item;

        private void Awake()
        {
            slotButton.onClick.AddListener(() =>
            {
                OnSelectSlot?.Invoke(this);
            });
        }

        public void SetSlot(Item item)
        {
            if (item.IsEmpty)
            {
                ClearSlot();
                return;
            }

            _item = item;
            iconImage.sprite = item.Data.Icon;
            SetItemAmount(item.Quantity);

            fillContainer.SetActive(true);
            slotButton.interactable = true;
        }

        public void ClearSlot()
        {
            _item = null;
            iconImage.sprite = null;
            SetItemAmount(0);

            fillContainer.SetActive(false);
            slotButton.interactable = false;
        }

        public void SetItemAmount(int quantity)
        {
            amountText.text = quantity > 1 ? $"{quantity}" : "";
        }

        public void SetActive(bool condition)
        {
            gameObject.SetActive(condition);
        }
    }
}
