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

        [PreviewField(50, ObjectFieldAlignment.Left)]
        public Sprite Icon;

        [Title("Amount Settings")]
        public ItemSlotType ItemSlotType;
        [ShowIf("ItemSlotType", ItemSlotType.Stack)]
        public int MaxStack = 99;


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