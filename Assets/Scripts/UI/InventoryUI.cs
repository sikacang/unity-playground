using Core.UI;
using Lean.Pool;
using System.Collections.Generic;
using UnityEngine;

namespace Tools.Inventory.UI
{
    public class InventoryUI : MonoBehaviour
    {
        [SerializeField]
        private Inventory inventory;

        [Header("Item Lists")]
        [SerializeField]
        private InventorySlotUI itemPrefab;
        [SerializeField]
        private Transform container;

        private List<InventorySlotUI> _itemSlots = new();

        private UIPage _uiPage;

        private void Awake()
        {
            _uiPage = GetComponent<UIPage>();

            _uiPage.OnOpen.AddListener(OnPageOpen);
            _uiPage.OnClose.AddListener(OnPageClose);
        }

        private void OnPageOpen()
        {
            inventory.AddItemAddListener(OnItemAdded);
            inventory.AddItemRemoveListener(OnItemRemoved);
            inventory.AddRefreshSlotsListener(OnRefreshSlots);
            
            OnRefreshSlots(inventory.Items);
        }

        private void OnPageClose()
        {
            ClearSlots();
            inventory.RemoveItemAddListener(OnItemAdded);
            inventory.RemoveItemRemoveListener(OnItemRemoved);
            inventory.RemoveRefreshSlotsListener(OnRefreshSlots);
        }

        private void OnItemAdded(ItemEventArgs args)
        {
            
        }

        private void OnItemRemoved(ItemEventArgs args)
        {
            
        }

        private void OnRefreshSlots(List<InventoryItem> list)
        {
            ClearSlots();

            for (int i = 0; i < list.Count; i++)
            {
                if (i < _itemSlots.Count)
                {
                    var slot = _itemSlots[i];
                    slot.SetSlot(list[i]);
                    slot.SetActive(true);
                }
                else
                {
                    InventorySlotUI slot = Instantiate(itemPrefab, container);
                    slot.SetSlot(list[i]);
                    _itemSlots.Add(slot);
                }                
            }
        }

        private void ClearSlots()
        {
            for (int i = 0; i < _itemSlots.Count; i++)
            {
                _itemSlots[i].SetActive(false);
            }
        }
    }
}