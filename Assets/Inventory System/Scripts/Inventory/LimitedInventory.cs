using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Tools.Inventory
{
    public class LimitedInventory : BaseInventory
    {
        public int MaxItems => _maxItems;
        public bool IsMax => HasEmptySlot() == false;

        [Title("Inventory Settings")]
        [PropertyOrder(-5)]
        [SerializeField]
        private int _maxItems = 10;

        public override void AddItem(ItemData itemData, int amount)
        {
            if (HasItem(out Item itemInstance, itemData))
            {
                if (itemInstance.CanAddQuantity())
                {
                    Debug.Log("Can Add");
                    amount = itemInstance.TryAddQuantity(amount);
                    OnItemAdded?.Invoke(new ItemEventArgs(itemInstance, _items.IndexOf(itemInstance)));
                }

                Debug.Log("Has Item");
                StartCoroutine(AddItemCoroutine(itemData, amount));
            }
            else
            {
                if(HasEmptySlot() == false)
                    return;

                Debug.Log("Adding new item");
                StartCoroutine(AddItemCoroutine(itemData, amount));
            }
        }

        public override void ReduceItem(Item item, int amount)
        {
            if (_items.Contains(item))
            {
                bool isEmpty = item.TryReduceQuantity(amount);
                Debug.Log($"Item {item.Data.name} reduced by {amount} in inventory");

                if (isEmpty)
                    RemoveItem(item);
                else
                    OnItemReduced?.Invoke(new ItemEventArgs(item, _items.IndexOf(item)));
            }
            else
            {
                Debug.Log($"Item {item.Data.name} not found in inventory");
            }
        }

        public override void RemoveItem(Item item)
        {
            if(_items.Contains(item) == false)
                return;

            item.Clear();

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
        private IEnumerator AddItemCoroutine(ItemData itemData, int amount)
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

                    var itemInstance = _items[emptySlot];
                    itemInstance.Fill(itemData);
                    amount = itemInstance.TryAddQuantity(amount);
                    OnItemAdded?.Invoke(new ItemEventArgs(itemInstance, _items.IndexOf(itemInstance)));
                }

                yield return null;
                tryAmount--;
            }
        }

        private bool HasEmptySlot()
        {            
            return _items.Any(item => item.IsEmpty);
        }

        private int GetEmptySlot()
        {
            var slot = _items.FindIndex(item => item.IsEmpty);
            return slot;
        }
    }
}
