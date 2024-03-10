using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Tools.Inventory
{
    public class ItemSlotUI : MonoBehaviour
    {
        public Item Item { get; private set; }

        public TextMeshProUGUI amountText;
        public Image iconImage;
        public GameObject selectedOutline;

        public UnityEvent<ItemSlotUI> OnClick = new();

        private void Awake()
        {
            var button = GetComponent<Button>();

            if(button == null)
            {
                Debug.LogError("No button found on ItemSlotUI");
            }
            else
            {
                button.onClick.AddListener(OnClickHandler);
            }

            selectedOutline.SetActive(false);
        }

        private void OnDestroy()
        {
            OnClick.RemoveAllListeners();
        }

        public void SetItem(Item item)
        {
            Item = item;

            if(Item == null)
            {
                iconImage.enabled = false;
                amountText.enabled = false;
            }
            else
            {
                iconImage.sprite = Item.Data.Icon;
                iconImage.enabled = Item.Data.Icon != null;

                amountText.text = Item.Quantity.ToString();
                amountText.enabled = Item.Quantity > 1;
            }
        }

        public void SelectItem(bool selected)
        {
            selectedOutline.SetActive(selected);
        }

        private void OnClickHandler()
        {
            OnClick?.Invoke(this);
        }
    }
}