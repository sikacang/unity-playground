using UnityEngine;
using Sirenix.OdinInspector;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Core
{
    [CreateAssetMenu(menuName = "Enum Id/New ID")]
    public class EnumId : ScriptableObject
    {
        public string Name {
            get { return enumName; }
            set { 
                enumName = value; 
                this.name = value; 
            }
        }

        [SerializeField]
        private string enumName;

#if UNITY_EDITOR

        [Button("Rename Enum Id", ButtonSizes.Medium)]
        [ContextMenu("Rename Enum Id")]
        public void Rename()
        {
            Name = enumName;
            AssetDatabase.SaveAssets();
            EditorUtility.SetDirty(this);
        }

#endif

    }

public static class UIEnumExtension
    {
        public static bool IsEqual(this EnumId origin, EnumId comparer)
        {
            if (origin == null || comparer == null)
                return false;

            return origin.name == comparer.name;
        }
    }
}