using Core;
using Sirenix.OdinInspector;
using Tools.Inventory;
using UnityEngine;
using UnityEngine.Events;

namespace Waroeng
{
    public class ShelfManager : SceneService
    {
        [ShowInInspector, ReadOnly]
        private LimitedInventory _firstInventory;
        [ShowInInspector, ReadOnly]
        private LimitedInventory _secondInventory;

        public UnityEvent<LimitedInventory, LimitedInventory> OnOpenShelf = new();
        public UnityEvent OnCloseShelf = new();
        
        [Button]
        public void OpenShelf(LimitedInventory firstInventory, LimitedInventory secondInventory)
        {
            _firstInventory = firstInventory;
            _secondInventory = secondInventory;

            OnOpenShelf.Invoke(_firstInventory, _secondInventory);
        }

        public void CloseShelf()
        {
            _firstInventory = null;
            _secondInventory = null;

            OnCloseShelf?.Invoke();
        }

        public bool TryTransferItems(LimitedInventory sourceInventory, LimitedInventory targetInventory, InventoryItem item)
        {
            if (targetInventory.IsMax)
                return false;

            targetInventory.AddItem(item.Data, 1);
            sourceInventory.ReduceItem(item, 1);

            return true;
        }
    }
}
