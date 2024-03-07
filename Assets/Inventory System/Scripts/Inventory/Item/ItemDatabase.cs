using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Tools.Inventory
{
    [CreateAssetMenu(fileName = "ItemDatabase", menuName = "Inventory/ItemDatabase")]
    public class ItemDatabase : ScriptableObject
    {
        public List<ItemData> Items => items;

        [SerializeField]
        [ListDrawerSettings(ShowIndexLabels = true, CustomAddFunction = nameof(AddNewItem), 
            CustomRemoveElementFunction = nameof(RemoveItem), NumberOfItemsPerPage = 10)]
        private List<ItemData> items = new List<ItemData>();

        public ItemData GetItemData(string itemId)
        {
            return items.Find(item => item.Id == itemId);
        }

#if UNITY_EDITOR
        private void AddNewItem() 
        {
            ItemData newItem = CreateInstance<ItemData>();
            newItem.name = $"{items.Count}_new item";
            newItem.GenerateId();

            items.Add(newItem);
            
            AssetDatabase.AddObjectToAsset(newItem, this);
            AssetDatabase.SaveAssets();
            
            EditorUtility.SetDirty(this);
        }

        [Button("Clear Items", ButtonSizes.Medium)]
        [PropertyOrder(-1)]
        public void ClearItem()
        {
            for (int i = 0; i < items.Count; i++)
            {
                Undo.DestroyObjectImmediate(items[i]);
                AssetDatabase.SaveAssets();
            }

            items = new();
            AssetDatabase.SaveAssets();
        }

        private void RemoveItem(ItemData removedItems)
        {
            items.Remove(removedItems);
            Undo.DestroyObjectImmediate(removedItems);
            AssetDatabase.SaveAssets();
            EditorUtility.SetDirty(this);
        }
#endif
    }
}