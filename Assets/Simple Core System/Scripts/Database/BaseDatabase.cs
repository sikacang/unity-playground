using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BaseDatabase<T> : ScriptableObject where T : BaseData
{
    public List<T> Items => items;

    [SerializeField]
    [ListDrawerSettings(ShowIndexLabels = true, CustomAddFunction = nameof(AddNewItem),
        CustomRemoveElementFunction = nameof(RemoveItem), NumberOfItemsPerPage = 10)]
    private List<T> items = new List<T>();

    public T GetItemData(string itemId)
    {
        return items.Find(item => item.Id == itemId);
    }

    public T GetItemData(int index)
    {
        return items[index];
    }

#if UNITY_EDITOR
    private void AddNewItem()
    {
        T newItem = CreateInstance<T>();
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

    private void RemoveItem(T removedItems)
    {
        items.Remove(removedItems);
        Undo.DestroyObjectImmediate(removedItems);
        AssetDatabase.SaveAssets();
        EditorUtility.SetDirty(this);
    }
#endif
}
