using Core;
using Tools.Interaction;
using Tools.Inventory;
using UnityEngine;

namespace Waroeng
{
    [RequireComponent(typeof(LimitedInventory))]
    public class Shelf : MonoBehaviour
    {
        public LimitedInventory Inventory => _inventory;
        
        private LimitedInventory _inventory;
        private ShelfManager _shelfManager;

        private void Awake()
        {
            _inventory = GetComponent<LimitedInventory>();
            _shelfManager = SceneServiceProvider.GetService<ShelfManager>();
        }

        public void OpenShelf(Interactor interactor)
        {
            var interactorsInventory = interactor.GetComponent<LimitedInventory>();

            if (interactorsInventory == null)
                return;
            
            _shelfManager.OpenShelf(this, interactorsInventory);
        }     
    }
}