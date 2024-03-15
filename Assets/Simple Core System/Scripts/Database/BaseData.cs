using Sirenix.OdinInspector;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class BaseData : ScriptableObject
{
    [TitleGroup("Base")]
    [ShowInInspector, ReadOnly, PropertyOrder(-7)]
    public string Id { get; private set; }

    [TitleGroup("Base")]
    [OnValueChanged(nameof(RenameFileData)), PropertyOrder(-5)]
    public string Name;

#if UNITY_EDITOR
    [ButtonGroup("Base/Generate ID")]
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
