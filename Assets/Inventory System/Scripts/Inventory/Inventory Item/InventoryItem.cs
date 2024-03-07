using Sirenix.OdinInspector;
using UnityEngine;

namespace Tools.Inventory
{
    [System.Serializable]
    public class InventoryItem
    {
        public ItemData Data => _data;
        public int Quantity => _quantity;
        [ShowInInspector]
        public bool IsEmpty { get; private set; }

        [ShowInInspector, ReadOnly]        
        protected ItemData _data;
        [ShowInInspector, ReadOnly]
        protected int _quantity;

        public InventoryItem()
        {
            IsEmpty = true;
        }

        public InventoryItem(ItemData data)
        {
            Fill(data);
        }

        public virtual int AddQuantity(int amount)
        {
            /*switch (_data.ItemSlotType)
            {
                case ItemSlotType.Stack:
                    if (_quantity + amount > Data.MaxStack)
                    {
                        int leftOver = _quantity + amount - Data.MaxStack;
                        _quantity = Data.MaxStack;
                        return leftOver;
                    }
                    else
                    {
                        _quantity += amount;
                        return 0;
                    }
                    
                case ItemSlotType.Single:
                    _quantity = 1;
                    return amount - 1;
            }*/

            return 0;
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
    
        public bool CanAddQuantity()
        {
            return _quantity < Data.MaxStack;
        }
    
        public void Fill(ItemData data)
        {
            _data = data;
            IsEmpty = false;
        }

        public void Clear()
        {
            _data = null;
            _quantity = 0;
            IsEmpty = true;
        }
    }
}