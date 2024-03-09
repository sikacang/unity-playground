using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;
using static UnityEditor.Progress;

namespace Tools.Inventory
{
    [System.Serializable]
    public class InventoryModel
    {
        [ShowInInspector, ReadOnly]
        private List<Item> Items = new();

        public readonly int capacity;

        public InventoryModel(int capacity)
        {
            this.capacity = capacity;
            Items = new List<Item>(capacity);
        }

        public InventoryModel(ItemData[] itemDetails, int capacity)
        {
            this.capacity = capacity;
            Items = new List<Item>(capacity);

            foreach (var itemData in itemDetails)
            {
                Items.Add(new Item(itemData, 0));
            }
        }

        public ReadOnlyCollection<Item> GetItems()
        {
            return Items.AsReadOnly();
        }        
    }
}