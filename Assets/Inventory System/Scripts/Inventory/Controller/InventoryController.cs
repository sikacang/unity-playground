using Core;
using Core.UI;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace Tools.Inventory
{
    public class InventoryController : MonoBehaviour
    {
        public ItemData[] startingItems;

        [SerializeField]
        private EnumId inventoryPageId;
        private InventoryPage _view;

        [ShowInInspector, ReadOnly]
        public SimpleInventory inventory;

        private void Awake()
        {
            inventory = new SimpleInventory(startingItems);
            inventory.OnRefreshItems += RefreshItems;
            inventory.OnItemRemoved += (_) =>
            {
                RefreshItems(inventory.Items);
            };
            inventory.OnItemReduced += (_) =>
            {
                RefreshItems(inventory.Items);
            };
            inventory.OnItemAdded += (_) =>
            {
                RefreshItems(inventory.Items);
            };

            var sceneUI = SceneServiceProvider.GetService<SceneUI>();
            var page = sceneUI.GetPage(inventoryPageId);

            if (page != null)
            {
                _view = page.GetComponent<InventoryPage>();
                _view.OnReduced.AddListener(inventory.ReduceItem);
                _view.OnRemoved.AddListener(inventory.RemoveItem);

                page.OnOpen.AddListener(() => _view.PopulateInventory(inventory.Items));
            }
        }

        private void RefreshItems(List<Item> list)
        {
            if (_view != null)
                _view.PopulateInventory(list);            
        }
    }
}