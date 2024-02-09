using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tools.Inventory
{
    [System.Serializable]
    public class StackedInventoryItem : InventoryItem
    {
        [SerializeField]
        private int maxStack = 99;

        public StackedInventoryItem(ItemData data, int quantity) : base(data, quantity)
        {
        }

        public override bool TryAddQuantity(int amount)
        {
            if (_quantity + amount > maxStack)
            {
                _quantity = maxStack;
                return false;
            }
            else
            {
                _quantity += amount;
                return true;
            }
        }
    }
}