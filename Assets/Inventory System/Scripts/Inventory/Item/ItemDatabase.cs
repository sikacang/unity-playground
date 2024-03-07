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

        [SerializeField, ReadOnly]
        private List<ItemData> items = new List<ItemData>();

        public ItemData GetItemData(string itemId)
        {
            return items.Find(item => item.ID == itemId);
        }

#if UNITY_EDITOR
        [FoldoutGroup("Editor")]
        [Button("Add New Item")]
        public void AddNewItem(string itemName) 
        {
            ItemData newItem = CreateInstance<ItemData>();
            newItem.name = $"{items.Count}_{itemName}";
            newItem.Name = itemName;
            newItem.SetDatabase(this);

            items.Add(newItem);
            
            AssetDatabase.AddObjectToAsset(newItem, this);
            AssetDatabase.SaveAssets();
            
            EditorUtility.SetDirty(this);
        }

        [FoldoutGroup("Editor")]
        [Button("Clear Items")]
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

        [FoldoutGroup("Editor")]
        [Button("Remove Item")]
        public void RemoveItem(ItemData removedItems)
        {
            items.Remove(removedItems);
            Undo.DestroyObjectImmediate(removedItems);
            AssetDatabase.SaveAssets();
        }
#endif
    }
}