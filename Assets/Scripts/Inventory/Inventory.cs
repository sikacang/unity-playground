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
                itemInstance.TryAddQuantity(amount);
            }
            else
            {
                var newItem = new InventoryItem(itemData, amount);
                items.Add(newItem);
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
            itemInstance = items.FirstOrDefault(item => item.IsSameItem(itemData));
            return itemInstance != null;
        }
    }
}