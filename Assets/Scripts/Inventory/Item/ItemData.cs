using UnityEngine;
using Sirenix.OdinInspector;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Tools.Inventory
{
    [CreateAssetMenu(menuName = "Inventory/ItemData")]
    public class ItemData : ScriptableObject
    {
        public string ID { get; private set; }

        public string Name;
        public string Description;
        public ItemSlotType ItemSlotType;

        private ItemDatabase _itemDatabase;

        public void SetDatabase(ItemDatabase database)
        {
            _itemDatabase = database;
        }

        public void SetID(string id)
        {
            ID = id;
        }

#if UNITY_EDITOR
        [FoldoutGroup("Editor")]
        [ButtonGroup("Editor/Remove Item")]
        public void RemoveItem()
        {
            _itemDatabase.RemoveItem(this);
        }
#endif
    }

    public enum ItemSlotType
    {
        Stack = 0,
        Single = 1,
    }
}