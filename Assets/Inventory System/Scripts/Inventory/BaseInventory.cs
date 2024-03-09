using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Tools.Inventory
{
    public abstract class BaseInventory : MonoBehaviour
    {
        public List<Item> Items => _items;

        [Title("List of Items")]
        [SerializeField]
        [ListDrawerSettings(IsReadOnly = true, HideAddButton = true, ShowItemCount = true)]
        protected List<Item> _items = new();

        // Events
        [PropertySpace(5, 5)]
        [FoldoutGroup("Events")]
        public UnityEvent<ItemEventArgs> OnItemAdded = new();
        [FoldoutGroup("Events")]
        public UnityEvent<ItemEventArgs> OnItemRemoved = new();
        [FoldoutGroup("Events")]
        public UnityEvent<ItemEventArgs> OnItemReduced = new();
        [FoldoutGroup("Events")]
        public UnityEvent<List<Item>> OnRefreshItems = new();

        // Abstract Methods
        public abstract void AddItem(ItemData itemData, int amount);
        public abstract void RemoveItem(Item item);
        public abstract void RemoveItem(int index);
        public abstract void ReduceItem(Item item, int amount);

        // Public Methods
        public void DestroyInventory()
        {
            _items.Clear();

            OnItemAdded.RemoveAllListeners();
            OnItemRemoved.RemoveAllListeners();
            OnItemReduced.RemoveAllListeners();
            OnRefreshItems.RemoveAllListeners();
        }

        protected bool HasItem(out Item item, ItemData itemData)
        {
            item = _items.Where(name => name.IsSameItem(itemData.Id))
                .OrderByDescending(name => name.Quantity)
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
