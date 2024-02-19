using Core;
using Tools.Inventory;
using UnityEngine;
using UnityEngine.Events;

namespace Waroeng
{
    public class CharacterOpenShelf : MonoBehaviour
    {
        private ShelfManager _shelfManager;

        public UnityEvent OnOpenShelf = new();
        public UnityEvent OnCloseShelf = new();

        private void Awake()
        {
            _shelfManager = SceneServiceProvider.GetService<ShelfManager>();
        }

        private void OnEnable()
        {
            if(_shelfManager != null)
            {
                _shelfManager.OnOpenShelf.AddListener(DisableCharacter);
                _shelfManager.OnCloseShelf.AddListener(EnableCharacter);
            }
        }

        private void OnDisable()
        {
            if(_shelfManager != null)
            {
                _shelfManager.OnOpenShelf.RemoveListener(DisableCharacter);
                _shelfManager.OnCloseShelf.RemoveListener(EnableCharacter);
            }
        }

        private void EnableCharacter()
        {
            OnCloseShelf?.Invoke();
        }

        private void DisableCharacter(BaseInventory arg0, BaseInventory arg1)
        {    
            OnOpenShelf?.Invoke();
        }
    }
}