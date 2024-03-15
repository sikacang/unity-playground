using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Tools.Inventory
{
    public class LimitedInventory : BaseInventoryModel
    {
        public int MaxItems => _maxItems;
        public bool IsMax => HasEmptySlot() == false;

        [Title("Inventory Settings")]
        [PropertyOrder(-5)]
        [SerializeField]
        private int _maxItems = 10;

        public LimitedInventory(ItemData[] initials) : base(initials)
        {
        }

        public override void AddItem(ItemData itemData, int amount)
        {
            if (TryGetItem(out Item itemInstance, itemData))
            {
                if (itemInstance.CanAddQuantity())
                {
                    amount = itemInstance.AddQuantity(amount);
                    OnItemAdded?.Invoke(new ItemEventArgs(itemInstance, _items.IndexOf(itemInstance)));
                }

                AddItemCoroutine(itemData, amount);
            }
            else
            {
                if(HasEmptySlot() == false)
                    return;

                AddItemCoroutine(itemData, amount);
            }
        }

        public override void ReduceItem(Item item, int amount)
        {
            if (_items.Contains(item))
            {
                bool isEmpty = item.ReduceQuantity(amount);

                if (isEmpty)
                    RemoveItem(item);
                else
                    OnItemReduced?.Invoke(new ItemEventArgs(item, _items.IndexOf(item)));
            }
        }

        public override void RemoveItem(Item item)
        {
            if(_items.Contains(item) == false)
                return;

            int index = _items.IndexOf(item);
            _items[index] = null;

            BlastRemoveEvent(item, _items.IndexOf(item));
        }

        public override void RemoveItem(int index)
        {
            if(index < 0 || index >= _items.Count)
                return;

            var removedItem = _items[index];
            RemoveItem(removedItem);
        }

        public bool CanAddItem(Item item)
        {
            var items = _items.Where(i => i.Data == item.Data);
            bool isFull = IsMax;

            foreach (var i in items)
                isFull &= i.CanAddQuantity() == false;

            return isFull == false;
        }

        // Private method
        private void AddItemCoroutine(ItemData itemData, int amount)
        {
            int tryAmount = amount;
            while (amount > 0 && tryAmount > 0)
            {
                if(HasEmptySlot() == false)
                    break;

                if (amount > 0)
                {                    
                    int emptySlot = GetEmptySlot();

                    if(emptySlot < 0 || emptySlot >= _items.Count)
                        break;

                    var item = _items[emptySlot];
                    amount = item.AddQuantity(amount);
                    OnItemAdded?.Invoke(new ItemEventArgs(item, _items.IndexOf(item)));
                }

                tryAmount--;
            }
        }

        private bool HasEmptySlot()
        {
            return false;
        }

        private int GetEmptySlot()
        {
            return 0;
        }
    }
}
