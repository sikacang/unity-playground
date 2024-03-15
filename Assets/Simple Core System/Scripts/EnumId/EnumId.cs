using UnityEngine;
using Sirenix.OdinInspector;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Core
{
    public class EnumId : ScriptableObject
    {
#if UNITY_EDITOR

        [Button("Rename Enum Id", ButtonSizes.Medium, Expanded = true)]
        [ContextMenu("Rename Enum Id")]
        public void Rename(string name)
        {
            this.name = name;
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