using UnityEngine;
using System.Collections;

namespace Tools.Inventory
{
    public class SimpleInventory : BaseInventory
    {
        // Public method
        public override void AddItem(ItemData itemData, int amount)
        {
            if(HasItem(out InventoryItem itemInstance, itemData))
            {
                StartCoroutine(AddItemCoroutine(itemInstance, itemData, amount));
            }
            else
            {
                itemInstance = new InventoryItem(itemData);
                _items.Add(itemInstance);
                StartCoroutine(AddItemCoroutine(itemInstance, itemData, amount));
            }
        }

        public override void RemoveItem(InventoryItem item)
        {
            if (_items.Contains(item))
            {
                int index = _items.IndexOf(item);
                
                _items.Remove(item);
                
                BlastRemoveEvent(item, index);
            }
            else
            {
                Debug.Log($"Item {item.Data.name} not found in inventory");
            }
        }

        public override void RemoveItem(int index)
        {
            if (index < 0 || index >= _items.Count)
            {
                Debug.Log($"Index {index} is out of range");
                return;
            }

            var removedItem = _items[index];
            _items.RemoveAt(index);

            BlastRemoveEvent(removedItem, index);
        }

        public override void ReduceItem(InventoryItem item, int amount)
        {
            if (_items.Contains(item))
            {
                bool isEmpty = item.ReduceQuantity(amount);
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

        // Private method
        private IEnumerator AddItemCoroutine(InventoryItem itemInstance, ItemData itemData, int amount)
        {
            int tryAmount = amount;
            while (amount > 0 && tryAmount > 0)
            {
                if (itemInstance.CanAddQuantity())
                {
                    amount = itemInstance.AddQuantity(amount);
                    OnItemAdded?.Invoke(new ItemEventArgs(itemInstance, _items.IndexOf(itemInstance)));
                }

                if (amount > 0)
                {
                    itemInstance = new InventoryItem(itemData);
                    amount = itemInstance.AddQuantity(amount);
                    _items.Add(itemInstance);
                    OnItemAdded?.Invoke(new ItemEventArgs(itemInstance, _items.IndexOf(itemInstance)));
                }

                yield return null;
                tryAmount--;
            }            
        }               
    }    
}