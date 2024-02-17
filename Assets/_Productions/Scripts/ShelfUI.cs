using System.Collections;
using System.Collections.Generic;
using Tools.Inventory;
using UnityEngine;
using Flexalon;
using UnityEngine.Events;
using Tools.Inventory.UI;

namespace Waroeng
{
    public class ShelfUI : MonoBehaviour
    {
        public LimitedInventory Inventory => _inventory;

        [Header("Item Slots")]
        [SerializeField]
        private Transform slotContainer;
        [SerializeField]
        private ShelfSlotUI slotPrefab;
        [SerializeField]
        private List<ShelfSlotUI> itemSlots = new();

        [Header("Content Layout")]
        [SerializeField]
        private FlexalonGridLayout contentLayout;
        [SerializeField]
        private int initialSlotCapacity = 10;

        public UnityEvent<ShelfSlotUI> OnPressedSlot = new();

        private LimitedInventory _inventory;

        private void Start()
        {
            for (int i = 0; i < initialSlotCapacity; i++)
            {
                var slot = CreateSlot();
                slot.SetActive(false);
                itemSlots.Add(slot);
            }
        }

        public void SetInventory(LimitedInventory inventory)
        {
            _inventory = inventory;

            SetLayoutRow(inventory.Items.Count);

            if (itemSlots.Count < inventory.Items.Count)
            {
                int missingSlot = _inventory.Items.Count - itemSlots.Count;
                for (int i = 0; i < missingSlot; i++)
                {
                    var slot = CreateSlot();
                    slot.SetActive(false);
                    itemSlots.Add(slot);
                }
            }

            OpenInventory();
        }

        public void OpenInventory()
        {
            _inventory.OnItemAdded.AddListener(OnItemAdded);
            _inventory.OnItemRemoved.AddListener(OnItemRemoved);
            _inventory.OnRefreshItems.AddListener(OnRefreshSlots);
            _inventory.OnItemReduced.AddListener(OnItemReduced);

            OnRefreshSlots(_inventory.Items);
        }

        public void CloseInventory()
        {
            ClearSlots();

            _inventory.OnItemAdded.RemoveListener(OnItemAdded);
            _inventory.OnItemRemoved.RemoveListener(OnItemRemoved);
            _inventory.OnRefreshItems.RemoveListener(OnRefreshSlots);
            _inventory.OnItemReduced.RemoveListener(OnItemReduced);
        }

        private void OnItemAdded(ItemEventArgs args)
        {
            SetupSlot(args.Index, args.Item);
        }

        private void OnItemRemoved(ItemEventArgs args)
        {
            if (args.Index > itemSlots.Count)
            {
                Debug.LogError("Index out of range");
                return;
            }

            Debug.Log($"Item removed from slot {args.Index}");
            itemSlots[args.Index].ClearSlot();
        }

        private void OnRefreshSlots(List<InventoryItem> list)
        {
            ClearSlots();

            for (int i = 0; i < list.Count; i++)
            {
                SetupSlot(i, list[i]);
            }
        }

        private void OnItemReduced(ItemEventArgs args)
        {
            if (args.Index > itemSlots.Count || args.Item == null)
            {
                Debug.LogError("Index out of range");
                return;
            }

            var slot = itemSlots[args.Index];
            slot.SetItemAmount(args.Item.Quantity);
        }

        private void SetupSlot(int index, InventoryItem item)
        {
            if (index < itemSlots.Count)
            {
                var slot = itemSlots[index];
                slot.SetSlot(item);
                slot.SetActive(true);
            }
            else
            {
                Debug.Log($"Index {index + 1} out of range. Slot capacity {itemSlots.Count}");
            }
        }

        private void ClearSlots()
        {
            for (int i = 0; i < itemSlots.Count; i++)
            {
                itemSlots[i].ClearSlot();
                itemSlots[i].SetActive(false);
            }
        }

        private void OnSelectedSlot(ShelfSlotUI slot)
        {
            OnPressedSlot?.Invoke(slot);
        }

        private ShelfSlotUI CreateSlot()
        {
            var slot = Instantiate(slotPrefab, slotContainer);
            slot.ClearSlot();
            slot.OnSelectSlot.AddListener(OnSelectedSlot);

            return slot;
        }

        private void SetLayoutRow(int capacity)
        {
            contentLayout.Rows = (uint)Mathf.CeilToInt((capacity / 5.0f));
        }
    }

}