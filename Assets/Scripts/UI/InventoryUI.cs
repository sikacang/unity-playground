using Core.UI;
using System.Collections.Generic;
using UnityEngine;

namespace Tools.Inventory.UI
{
    public class InventoryUI : MonoBehaviour
    {
        [SerializeField]
        private SimpleInventory inventory;

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
            inventory.OnItemAdded.AddListener(OnItemAdded);
            inventory.OnItemRemoved.AddListener(OnItemRemoved);
            inventory.OnRefreshItems.AddListener(OnRefreshSlots);
            inventory.OnItemReduced.AddListener(OnItemReduced);
            
            OnRefreshSlots(inventory.Items);
        }

        private void OnPageClose()
        {
            ClearSlots();
            inventory.OnItemAdded.RemoveListener(OnItemAdded);
            inventory.OnItemRemoved.RemoveListener(OnItemRemoved);
            inventory.OnRefreshItems.RemoveListener(OnRefreshSlots);
            inventory.OnItemReduced.RemoveListener(OnItemReduced);
        }

        private void OnItemAdded(ItemEventArgs args)
        {
            Debug.Log($"Item added {args.Item.Data.Name} to slot {args.Index}");
            SpawnItemSlot(args.Index, args.Item);            
        }

        private void OnItemRemoved(ItemEventArgs args)
        {
            if(args.Index > _itemSlots.Count)
            {
                Debug.LogError("Index out of range");
                return;
            }

            Debug.Log($"Item removed {args.Item.Data.Name} from slot {args.Index}");
            _itemSlots[args.Index].SetActive(false);
            OnRefreshSlots(inventory.Items);
        }

        private void OnRefreshSlots(List<InventoryItem> list)
        {
            ClearSlots();

            for (int i = 0; i < list.Count; i++)
            {
                SpawnItemSlot(i, list[i]);
            }
        }

        private void OnItemReduced(ItemEventArgs args)
        {
            if (args.Index > _itemSlots.Count || args.Item == null)
            {
                Debug.LogError("Index out of range");
                return;
            }

            var slot = _itemSlots[args.Index];
            slot.SetItemAmount(args.Item.Quantity);
        }

        private void ClearSlots()
        {
            for (int i = 0; i < _itemSlots.Count; i++)
            {
                _itemSlots[i].SetActive(false);
            }
        }
    
        private void OnRemoveSlotPressed(InventorySlotUI slot)
        {
            Debug.Log("Remove slot pressed : " + _itemSlots.IndexOf(slot));
            inventory.RemoveItem(slot.Item);
        }

        private void SpawnItemSlot(int index, InventoryItem item)
        {
            if (index < _itemSlots.Count)
            {
                var slot = _itemSlots[index];
                slot.SetSlot(item);
                slot.AddOnRemoveItemListener(OnRemoveSlotPressed);
                slot.AddOnReduceItemListener(OnReduceSlotPressed);
                slot.SetActive(true);
            }
            else
            {
                InventorySlotUI slot = Instantiate(itemPrefab, container);
                slot.SetSlot(item);
                slot.AddOnRemoveItemListener(OnRemoveSlotPressed);
                slot.AddOnReduceItemListener(OnReduceSlotPressed);
                _itemSlots.Add(slot);
            }
        }

        private void OnReduceSlotPressed(InventorySlotUI slot, int amount)
        {
            inventory.ReduceItem(slot.Item, amount);
        }
    }
}