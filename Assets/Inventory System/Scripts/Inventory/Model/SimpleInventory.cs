using System;

namespace Tools.Inventory
{
    [Serializable]
    public class SimpleInventory : BaseInventoryModel
    {
        public SimpleInventory(ItemData[] initials) : base(initials)
        {
        }

        // Public method
        public override void AddItem(ItemData itemData, int amount)
        {
            if(TryGetItem(out Item item, itemData))
            {
                AddItemProcess(item, itemData, amount);
            }
            else
            {
                item = new Item(itemData, 0);
                _items.Add(item);
                AddItemProcess(item, itemData, amount);
            }
        }

        public override void RemoveItem(Item item)
        {
            if (_items.Contains(item) == false)
                return;

            int index = _items.IndexOf(item);
            _items.Remove(item);

            BlastRemoveEvent(item, index);
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
                bool isEmpty = item.ReduceQuantity(amount);
                
                if (isEmpty)
                    RemoveItem(item);
                else
                    OnItemReduced?.Invoke(new ItemEventArgs(item, _items.IndexOf(item)));                
            }
        }

        // Private method
        private void AddItemProcess(Item item, ItemData itemData, int amount)
        {
            int tryAmount = amount;
            int addCount = 0;

            while (amount > 0 && tryAmount > 0)
            {
                if (item.CanAddQuantity())
                {
                    amount = item.AddQuantity(amount);
                    addCount++;
                }

                if (amount > 0)
                {
                    item = new Item(itemData, 0);
                    amount = item.AddQuantity(amount);
                    _items.Add(item);
                    addCount++;
                }

                tryAmount--;
            }         
            
            if(addCount > 1)
                OnRefreshItems?.Invoke(_items);
            else
                OnItemAdded?.Invoke(new ItemEventArgs(item, _items.IndexOf(item)));
        }               
    }    
}