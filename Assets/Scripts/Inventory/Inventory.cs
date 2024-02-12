using UnityEngine;
using System.Linq;
using System.Collections.Generic;

namespace Tools.Inventory
{
    public class Inventory : MonoBehaviour
    {
        [Header("List of Items")]
        [SerializeField] 
        private List<InventoryItem> items = new();

        // Public method
        public void AddItem(ItemData itemData, int amount)
        {
            if(HasItem(out InventoryItem itemInstance, itemData))
            {
                AddStackedItem(itemInstance);
            }
            else
            {
                itemInstance = new InventoryItem(itemData);
                items.Add(itemInstance);
                AddStackedItem(itemInstance);
            }

            void AddStackedItem(InventoryItem itemInstance)
            {
                while (amount > 0)
                {
                    amount = itemInstance.AddQuantity(amount);
                    if (amount > 0)
                    {
                        itemInstance = new InventoryItem(itemData);
                        amount = itemInstance.AddQuantity(amount);
                        items.Add(itemInstance);
                    }
                }
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
                items.Remove(itemInstance);
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
                items.Remove(item);
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

            items.RemoveAt(index);
        }

        public void ReduceItem(ItemData itemData, int amount)
        {
            bool hasItem = HasItem(out InventoryItem itemInstance, itemData);
            if(hasItem)
            {
                bool isEmpty = itemInstance.ReduceQuantity(amount);
                if(isEmpty)
                    RemoveItem(itemData);                
                else
                    Debug.Log($"Item {itemData.name} reduced by {amount} in inventory");                
            }
            else
            {
                Debug.Log($"Item {itemData.name} not found in inventory");            
            }
        }

        // Private method
        private bool HasItem(out InventoryItem itemInstance, ItemData itemData)
        {
            itemInstance = items.Where(name => name.IsSameItem(itemData))
                .OrderByDescending(name => name.Quantity)
                .LastOrDefault();

            return itemInstance != null;
        }
    }
}