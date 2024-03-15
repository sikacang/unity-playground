using Sirenix.OdinInspector;
using System;

namespace Tools.Inventory.Crafting
{
    public class CraftData : BaseData
    {
        [TitleGroup("Craft Data")]
        public CraftResult CraftResult;

        [TitleGroup("Craft Materials")]
        public CraftMaterial[] Materials;
    }

    [Serializable]
    public class CraftMaterial
    {
        public ItemData ItemData;
        public int Amount;
    }

    [System.Serializable]
    public class CraftResult
    {
        public ItemData ItemData;
        public int Amount;
    }
}