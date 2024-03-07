using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Tools.Inventory
{
    public abstract class BaseInventory : MonoBehaviour
    {
        public List<InventoryItem> Items => _items;

        [Title("List of Items")]
        [SerializeField]
        [ListDrawerSettings(IsReadOnly = true, HideAddButton = true, ShowItemCount = true)]
        protected List<InventoryItem> _items = new();

        // Events
        [PropertySpace(5, 5)]
        [FoldoutGroup("Events")]
        public UnityEvent<ItemEventArgs> OnItemAdded = new();
        [FoldoutGroup("Events")]
        public UnityEvent<ItemEventArgs> OnItemRemoved = new();
        [FoldoutGroup("Events")]
        public UnityEvent<ItemEventArgs> OnItemReduced = new();
        [FoldoutGroup("Events")]
        public UnityEvent<List<InventoryItem>> OnRefreshItems = new();

        // Abstract Methods
        public abstract void AddItem(ItemData itemData, int amount);
        public abstract void RemoveItem(InventoryItem item);
        public abstract void RemoveItem(int index);
        public abstract void ReduceItem(InventoryItem item, int amount);

        // Public Methods
        public void DestroyInventory()
        {
            _items.Clear();

            OnItemAdded.RemoveAllListeners();
            OnItemRemoved.RemoveAllListeners();
            OnItemReduced.RemoveAllListeners();
            OnRefreshItems.RemoveAllListeners();
        }

        protected bool HasItem(out InventoryItem itemInstance, ItemData itemData)
        {
            itemInstance = _items.Where(name => name.IsSameItem(itemData))
                .OrderByDescending(name => name.Quantity)
                .LastOrDefault();

            return itemInstance != null;
        }

        protected void BlastRemoveEvent(InventoryItem item, int index)
        {
            OnItemRemoved?.Invoke(new ItemEventArgs(item, index));
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
