using Core;
using System;
using System.Collections;
using System.Collections.Generic;
using Tools.Inventory;
using UnityEngine;
using UnityEngine.Events;

namespace Waroeng
{
    public class CharacterOpenShelf : MonoBehaviour
    {
        private LimitedInventory _playerInventory;
        private LimitedInventory _closestShelf;
        private ShelfManager _shelfManager;

        public UnityEvent OnOpenShelf = new();
        public UnityEvent OnCloseShelf = new();

        private bool _isOpen;

        private void Awake()
        {
            _playerInventory = GetComponent<LimitedInventory>();
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
            _isOpen = false;
            OnCloseShelf?.Invoke();
        }

        private void DisableCharacter(BaseInventory arg0, BaseInventory arg1)
        {
            _isOpen = true;            
            OnOpenShelf?.Invoke();
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.E) && _isOpen == false)
            {
                if(_closestShelf != null)
                {
                    _shelfManager.OpenShelf(_playerInventory, _closestShelf);
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.TryGetComponent(out LimitedInventory shelf))
            {
                _closestShelf = shelf;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if(collision.TryGetComponent(out LimitedInventory shelf))
            {
                _closestShelf = null;
            }
        }
    }
}