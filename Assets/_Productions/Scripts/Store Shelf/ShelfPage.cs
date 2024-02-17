using Core;
using Core.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using Tools.Inventory;
using Tools.Inventory.UI;
using UnityEngine;

namespace Waroeng
{
    public class ShelfPage : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField]
        private ShelfUI playerShelf;
        [SerializeField]
        private ShelfUI storeShelf;

        private ShelfManager _shelfManager;
        private UIPage _uiPage;

        private void Awake()
        {
            _shelfManager = SceneServiceProvider.GetService<ShelfManager>();
            _uiPage = GetComponent<UIPage>();

            _uiPage.OnClose.AddListener(() =>
            {
                playerShelf.CloseInventory();
                storeShelf.CloseInventory();

                if (_shelfManager != null)
                    _shelfManager.CloseShelf();
            });
        }

        private void Start()
        {
            if(_shelfManager != null)
            {
                _shelfManager.OnOpenShelf.AddListener(OnOpenShelf);
            }

            playerShelf.OnPressedSlot.AddListener((slot) =>
            {
                _shelfManager.TryTransferItems(playerShelf.Inventory, storeShelf.Inventory, slot.Item);
            });

            storeShelf.OnPressedSlot.AddListener((slot) =>
            {
                _shelfManager.TryTransferItems(storeShelf.Inventory, playerShelf.Inventory, slot.Item);                
            });
        }

        private void OnOpenShelf(LimitedInventory playerInventory, LimitedInventory shelfInventory)
        {
            playerShelf.SetInventory(playerInventory);
            storeShelf.SetInventory(shelfInventory);

            _uiPage.OpenPage();
        }
    }
}