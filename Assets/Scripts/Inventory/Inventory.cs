using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using System;
using static UnityEditor.Progress;
using System.Collections;

namespace Tools.Inventory
{
    public class Inventory : MonoBehaviour
    {
        public List<InventoryItem> Items => items;

        [Header("List of Items")]
        [SerializeField] 
        private List<InventoryItem> items = new();

        // Events
        private Action<ItemEventArgs> _onItemAdded;
        private Action<ItemEventArgs> _onItemRemoved;
        private Action<ItemEventArgs> _onItemReduced;
        private Action<List<InventoryItem>> _onRefreshSlots;

        // Public method
        public void AddItem(ItemData itemData, int amount)
        {
            if(HasItem(out InventoryItem itemInstance, itemData))
            {
                StartCoroutine(AddItemCoroutine(itemInstance, itemData, amount));
            }
            else
            {
                itemInstance = new InventoryItem(itemData);
                items.Add(itemInstance);
                StartCoroutine(AddItemCoroutine(itemInstance, itemData, amount));
            }
        }

        IEnumerator AddItemCoroutine(InventoryItem itemInstance, ItemData itemData, int amount)
        {
            int tryAmount = amount;
            while (amount > 0 && tryAmount > 0)
            {
                if (itemInstance.CanAddQuantity())
                {
                    amount = itemInstance.AddQuantity(amount);
                    _onItemAdded?.Invoke(new ItemEventArgs(itemInstance, items.IndexOf(itemInstance)));
                }

                if (amount > 0)
                {
                    itemInstance = new InventoryItem(itemData);
                    amount = itemInstance.AddQuantity(amount);
                    items.Add(itemInstance);
                    _onItemAdded?.Invoke(new ItemEventArgs(itemInstance, items.IndexOf(itemInstance)));
                }

                yield return null;
                tryAmount--;
            }            
        }

        public void RemoveItem(ItemData itemData)
        {
            if (itemData == null)
            {
                Debug.LogWarning("Item Data is null");
                return;
            }

            bool hasItem = HasItem(out InventoryItem itemInstance, itemData);
            if(hasItem)
            {
                RemoveItem(itemInstance);
                Debug.Log($"Item {itemData.name} removed from inventory");
            }
            else
            {
                Debug.Log($"Item {itemData.name} not found in inventory");            
            }
        }

        public void RemoveItem(InventoryItem item)
        {
            if (items.Contains(item))
            {
                int index = items.IndexOf(item);
                
                items.Remove(item);
                
                BlastRemoveEvent(item, index);
            }
            else
            {
                Debug.Log($"Item {item.Data.name} not found in inventory");
            }
        }

        public void RemoveItem(int index)
        {
            if (index < 0 || index >= items.Count)
            {
                Debug.Log($"Index {index} is out of range");
                return;
            }

            var removedItem = items[index];
            items.RemoveAt(index);

            BlastRemoveEvent(removedItem, index);
        }

        public void ReduceItem(InventoryItem item, int amount)
        {
            if (items.Contains(item))
            {
                bool isEmpty = item.ReduceQuantity(amount);
                Debug.Log($"Item {item.Data.name} reduced by {amount} in inventory");
                
                if (isEmpty)
                    RemoveItem(item);                
                else
                    _onItemReduced?.Invoke(new ItemEventArgs(item, items.IndexOf(item)));                
            }
            else
            {
                Debug.Log($"Item {item.Data.name} not found in inventory");
            }
        }

        public void AddItemAddListener(Action<ItemEventArgs> callback)
        {
            _onItemAdded += callback;
        }

        public void AddItemRemoveListener(Action<ItemEventArgs> callback)
        {
            _onItemRemoved += callback;
        }

        public void AddRefreshSlotsListener(Action<List<InventoryItem>> callback)
        {
            _onRefreshSlots += callback;
        }

        public void AddItemReducedListener(Action<ItemEventArgs> callback)
        {
            _onItemReduced += callback;
        }

        public void RemoveItemAddListener(Action<ItemEventArgs> callback)
        {
            _onItemAdded -= callback;
        }

        public void RemoveItemRemoveListener(Action<ItemEventArgs> callback)
        {
            _onItemRemoved -= callback;
        }

        public void RemoveRefreshSlotsListener(Action<List<InventoryItem>> callback)
        {
            _onRefreshSlots -= callback;
        }

        public void RemoveItemReducedListener(Action<ItemEventArgs> callback)
        {
            _onItemReduced -= callback;
        }

        // Private method
        private bool HasItem(out InventoryItem itemInstance, ItemData itemData)
        {
            itemInstance = items.Where(name => name.IsSameItem(itemData))
                .OrderByDescending(name => name.Quantity)
                .LastOrDefault();

            return itemInstance != null;
        }

        private void RefreshSlots()
        {
            _onRefreshSlots?.Invoke(items);
        }

        private void BlastRemoveEvent(InventoryItem item, int index)
        {
            _onItemRemoved?.Invoke(new ItemEventArgs(item, index));            
        }
    }

    public struct ItemEventArgs
    {
        public InventoryItem Item;
        public int Index;

        public ItemEventArgs(InventoryItem item, int index)
        {
            Item = item;
            Index = index;
        }
    }
}