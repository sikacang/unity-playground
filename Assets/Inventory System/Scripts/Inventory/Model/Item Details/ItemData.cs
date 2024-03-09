using UnityEngine;
using Sirenix.OdinInspector;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Tools.Inventory
{
    public class ItemData : ScriptableObject
    {
        [ShowInInspector, ReadOnly, PropertyOrder(-5)]
        [HorizontalGroup("ItemSplit", 0.75f), LabelWidth(75), VerticalGroup("ItemSplit/Left")]
        public string Id { get; private set; }

        [HorizontalGroup("ItemSplit", 0.75f), LabelWidth(75), VerticalGroup("ItemSplit/Left")]
        [OnValueChanged(nameof(RenameFileData))]        
        public string Name;

        [HorizontalGroup("ItemSplit", 0.75f), LabelWidth(75), VerticalGroup("ItemSplit/Left")]
        public int MaxStack = 1;

        [HorizontalGroup("ItemSplit", 0.75f), VerticalGroup("ItemSplit/Left")]
        [TextArea, HideLabel]
        public string Description;

        [HorizontalGroup("ItemSplit"), VerticalGroup("ItemSplit/Right")]
        [PreviewField(100), HideLabel]
        public Sprite Icon;

#if UNITY_EDITOR
        [HorizontalGroup("ItemSplit"), VerticalGroup("ItemSplit/Left")]
        [Button("Generate ID")]
        public void GenerateId()
        {
            System.Random rnd = new System.Random();
            int myRandomNo = rnd.Next(1000, 9999);
            Id = $"{RandomStringGenerator.GenerateRandomString(4)}-{myRandomNo}";
        }

        private void RenameFileData()
        {
            this.name = Name;
            AssetDatabase.SaveAssets();
            EditorUtility.SetDirty(this);
        }
#endif
    }
}