using System;
using Tools.Inventory;
using UnityEngine;

namespace Waroeng
{
    public class Warehouse : MonoBehaviour
    {
        [SerializeField]
        private StarterItem[] starterItems;

        private LimitedInventory _inventory;

        private void Awake()
        {
            _inventory = GetComponent<LimitedInventory>();
        }

        private void Start()
        {
            _inventory.OnItemReduced.AddListener(OnItemReduced);

            foreach (var starterItem in starterItems)
            {
                _inventory.AddItem(starterItem.Data, starterItem.Amount);
            }
        }

        private void OnItemReduced(ItemEventArgs arg)
        {
            
        }
    }

    [Serializable]
    public struct StarterItem
    {
        public ItemData Data;
        public int Amount;
    }
}