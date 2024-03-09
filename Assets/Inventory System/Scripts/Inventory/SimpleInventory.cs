using UnityEngine;
using System.Collections;

namespace Tools.Inventory
{
    public class SimpleInventory : BaseInventory
    {
        // Public method
        public override void AddItem(ItemData itemData, int amount)
        {
            if(HasItem(out Item item, itemData))
            {
                AddItemCoroutine(item, itemData, amount);
            }
            else
            {
                item = new Item(itemData, 0);
                _items.Add(item);
                AddItemCoroutine(item, itemData, amount);
            }
        }

        public override void RemoveItem(Item item)
        {
            if (_items.Contains(item))
            {
                int index = _items.IndexOf(item);
                
                _items.Remove(item);
                
                BlastRemoveEvent(item, index);
            }
        }

        public override void RemoveItem(int index)
        {
            if (index < 0 || index >= _items.Count)
                return;
            
            var removedItem = _items[index];
            _items.RemoveAt(index);

            BlastRemoveEvent(removedItem, index);
        }

        public override void ReduceItem(Item item, int amount)
        {
            if (_items.Contains(item))
            {
                bool isEmpty = item.TryReduceQuantity(amount);
                
                if (isEmpty)
                    RemoveItem(item);
                else
                    OnItemReduced?.Invoke(new ItemEventArgs(item, _items.IndexOf(item)));                
            }
        }

        // Private method
        private void AddItemCoroutine(Item itemInstance, ItemData itemData, int amount)
        {
            int tryAmount = amount;
            while (amount > 0 && tryAmount > 0)
            {
                if (itemInstance.CanAddQuantity())
                {
                    amount = itemInstance.TryAddQuantity(amount);
                    OnItemAdded?.Invoke(new ItemEventArgs(itemInstance, _items.IndexOf(itemInstance)));
                }

                if (amount > 0)
                {
                    itemInstance = new Item(itemData, 0);
                    amount = itemInstance.TryAddQuantity(amount);
                    _items.Add(itemInstance);
                    OnItemAdded?.Invoke(new ItemEventArgs(itemInstance, _items.IndexOf(itemInstance)));
                }

                tryAmount--;
            }            
        }               
    }    
}