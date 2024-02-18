using Core.UI;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Tools.Inventory.UI
{
    public class GridInventoryUI : MonoBehaviour
    {
        [ShowInInspector, ReadOnly]
        private BaseInventory inventory;

        [Header("Item Information")]
        [SerializeField]
        private TextMeshProUGUI itemNameText;
        [SerializeField]
        private TextMeshProUGUI itemDescriptionText;
        [SerializeField]
        private Image itemIconImage;
        [SerializeField]
        private Button removeItemButton;
        [SerializeField]
        private Button reduceItemButton;
        [SerializeField]
        private GameObject itemInformationContainer;

        [Header("Item Slots")]
        [SerializeField]
        private Transform slotContainer;
        [SerializeField]
        private GridInventorySlotUI slotPrefab;
        [SerializeField]
        private List<GridInventorySlotUI> itemSlots = new();
        [SerializeField]
        private int initialSlotCapacity = 10;

        private GridInventorySlotUI _selectedSlot;

        private void Awake()
        {
            for (int i = 0; i < initialSlotCapacity; i++)
            {
                var slot = CreateSlot();
                slot.SetActive(false);
                itemSlots.Add(slot);
            }

            removeItemButton.onClick.AddListener(RemoveSelectedItem);
            reduceItemButton.onClick.AddListener(ReduceSelectedItem);
        }

        public void SetInventory(BaseInventory inventory)
        {
            this.inventory = inventory;
        }

        public void OpenInventory()
        {
            inventory.OnItemAdded.AddListener(OnItemAdded);
            inventory.OnItemRemoved.AddListener(OnItemRemoved);
            inventory.OnRefreshItems.AddListener(OnRefreshSlots);
            inventory.OnItemReduced.AddListener(OnItemReduced);

            OnRefreshSlots(inventory.Items);
            ClearItemInformation();
        }

        public void CloseInventory()
        {
            ClearSlots();
            ClearItemInformation();

            inventory.OnItemAdded.RemoveListener(OnItemAdded);
            inventory.OnItemRemoved.RemoveListener(OnItemRemoved);
            inventory.OnRefreshItems.RemoveListener(OnRefreshSlots);
            inventory.OnItemReduced.RemoveListener(OnItemReduced);
        }

        private void OnItemAdded(ItemEventArgs args)
        {
            SetupSlot(args.Index, args.Item);
            Debug.Log($"Item added {args.Item.Data.Name} to slot {args.Index}");
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
            }
        }

        private void OnSelectedSlot(GridInventorySlotUI slot)
        {
            _selectedSlot = slot;
            var item = slot.Item;
            PopulateItemInformation(item.Data.Name, item.Data.Description, item.Data.Icon);
        }

        private void PopulateItemInformation(string name, string description, Sprite icon)
        {
            itemNameText.text = name;
            itemDescriptionText.text = description;
            itemIconImage.sprite = icon;
            itemIconImage.enabled = icon != null;
            itemInformationContainer.SetActive(true);
        }

        private void ClearItemInformation()
        {
            _selectedSlot = null;
            PopulateItemInformation(name: "", description: "", icon: null);
            itemInformationContainer.SetActive(false);
        }

        private void RemoveSelectedItem()
        {
            if(_selectedSlot == null)
            {
                Debug.LogError("No item selected");
                return;
            }

            inventory.RemoveItem(_selectedSlot.Item);
            ClearItemInformation();
        }

        private void ReduceSelectedItem()
        {
            if(_selectedSlot == null)
            {
                Debug.LogError("No item selected");
                return;
            }

            inventory.ReduceItem(_selectedSlot.Item, 1);
        }
    
        private GridInventorySlotUI CreateSlot()
        {
            var slot = Instantiate(slotPrefab, slotContainer);
            slot.ClearSlot();
            slot.OnSelectSlot.AddListener(OnSelectedSlot);

            return slot;
        }
    }
}