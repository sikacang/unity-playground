using Sirenix.OdinInspector;
using UnityEngine;

namespace Tools.Inventory
{
    [System.Serializable]
    public class InventoryItem
    {
        public ItemData Data => _data;
        public int Quantity => _quantity;

        [ShowInInspector, ReadOnly]        
        protected ItemData _data;
        [ShowInInspector, ReadOnly]
        protected int _quantity;

        public InventoryItem(ItemData data, int quantity)
        {
            this._data = data;
            this._quantity = quantity;
        }

        public virtual bool TryAddQuantity(int amount)
        {
            _quantity += amount;
            return true;
        }

        public virtual bool ReduceQuantity(int amount)
        {
            _quantity -= amount;
            if (_quantity <= 0)
            {
                _quantity = 0;
                return true;
            }

            return false;
        }

        public bool IsSameItem(ItemData itemData)
        {
            return _data == itemData;
        }
    }
}