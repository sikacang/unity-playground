using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Tools.Inventory
{
    [Serializable]
    public abstract class BaseInventoryModel
    {
        public List<Item> Items => _items;

        [ShowInInspector]
        protected List<Item> _items = new();

        // Events
        public Action<ItemEventArgs> OnItemAdded;
        public Action<ItemEventArgs> OnItemRemoved;
        public Action<ItemEventArgs> OnItemReduced;
        public Action<List<Item>> OnRefreshItems;

        public BaseInventoryModel(ItemData[] initials)
        {
            for (int i = 0; i < initials.Length; i++)
            {
                _items.Add(new Item(initials[i], 1));
            }
        }

        // Abstract Methods
        public abstract void AddItem(ItemData itemData, int amount);
        public abstract void RemoveItem(Item item);
        public abstract void RemoveItem(int index);
        public abstract void ReduceItem(Item item, int amount);

        // Public Methods        
        public void DestroyInventory()
        {
            _items.Clear();

            OnItemAdded = null;
            OnItemRemoved = null;
            OnItemReduced = null;
            OnRefreshItems = null;
        }

        public bool TryGetItem(out Item item, ItemData itemData)
        {
            item = _items.Where(i => i.DataId == itemData.Id)
                .OrderByDescending(j => j.Quantity)
                .LastOrDefault();

            return item != null;
        }

        protected void BlastRemoveEvent(Item item, int index)
        {
            OnItemRemoved?.Invoke(new ItemEventArgs(item, index));
        }
    }

    public struct ItemEventArgs
    {
        public Item Item;
        public int Index;

        public ItemEventArgs(Item item, int index)
        {
            Item = item;
            Index = index;
        }
    }
}
