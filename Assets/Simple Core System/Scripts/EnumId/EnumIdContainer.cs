using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;


#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Core
{
    [CreateAssetMenu(fileName = "EnumIdContainer", menuName = "Core/Enum Container", order = 1)]
    public class EnumIdContainer : ScriptableObject
    {
        [SerializeField]
        [PropertyOrder(0)]
        [ListDrawerSettings(ShowIndexLabels = true, CustomRemoveElementFunction = nameof(RemoveEnumId), NumberOfItemsPerPage = 10)]
        private List<EnumId> _enumIds = new();

#if UNITY_EDITOR

        [Button("Add New", ButtonSizes.Medium)]
        [PropertyOrder(-1)]
        public void AddNewEnum()
        {
            var newEnum = ScriptableObject.CreateInstance<EnumId>();
            newEnum.Name = $"New Enum {_enumIds.Count + 1}";
            _enumIds.Add(newEnum);

            AssetDatabase.AddObjectToAsset(newEnum, this);
            AssetDatabase.SaveAssets();

            EditorUtility.SetDirty(this);
            EditorUtility.SetDirty(newEnum);
        }

        private void RemoveEnumId(EnumId enumId)
        {
            if (_enumIds.Contains(enumId))
            {
                _enumIds.Remove(enumId);
                Undo.DestroyObjectImmediate(enumId);
                AssetDatabase.SaveAssets();
                EditorUtility.SetDirty(this);
            }
        }

#endif
    }
}