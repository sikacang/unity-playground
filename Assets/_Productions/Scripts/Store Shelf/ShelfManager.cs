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
        private LimitedInventory _playerInventory;
        [ShowInInspector, ReadOnly]
        private LimitedInventory _shelfInventory;

        public UnityEvent<LimitedInventory, LimitedInventory> OnOpenShelf = new();
        public UnityEvent OnCloseShelf = new();
        
        [Button]
        public void OpenShelf(Shelf shelf, LimitedInventory playerInventory)
        {
            _shelfInventory = shelf.Inventory;
            _playerInventory = playerInventory;

            OnOpenShelf.Invoke(_playerInventory, _shelfInventory);
        }

        public void CloseShelf()
        {
            _playerInventory = null;
            _shelfInventory = null;

            OnCloseShelf?.Invoke();
        }

        public bool TryTransferItems(LimitedInventory sourceInventory, LimitedInventory targetInventory, InventoryItem item)
        {
            if (targetInventory.IsMax && targetInventory.CanAddItem(item) == false)
                return false;

            targetInventory.AddItem(item.Data, 1);
            sourceInventory.ReduceItem(item, 1);

            return true;
        }
    }
}
