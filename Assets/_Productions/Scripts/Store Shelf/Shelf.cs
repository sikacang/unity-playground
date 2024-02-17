using System.Collections;
using System.Collections.Generic;
using Tools.Inventory;
using UnityEngine;

namespace Waroeng
{
    [RequireComponent(typeof(LimitedInventory))]
    public class Shelf : MonoBehaviour
    {
        public LimitedInventory Inventory => _inventory;

        private LimitedInventory _inventory;

        private void Awake()
        {
            _inventory = GetComponent<LimitedInventory>();
        }

        public void AddItem(ItemData data, int amount)
        {
            _inventory.AddItem(data, amount);
        }
    }
}