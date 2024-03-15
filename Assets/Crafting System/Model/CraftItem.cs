using Sirenix.OdinInspector;

namespace Tools.Inventory.Crafting
{
    [System.Serializable]
    public class CraftItem
    {
        public string Id { get; private set; }
        
        [ShowInInspector, ReadOnly]
        public string DataId { get; private set; }
        public CraftData CraftData { get; private set; }
        
        public bool IsCraftable => _isCraftable && _isEnoughMaterial;

        [ShowInInspector, ReadOnly]
        private bool _isCraftable;

        [ShowInInspector, ReadOnly]
        private bool _isEnoughMaterial;

        public CraftItem(CraftData data)
        {
            Id = $"{RandomStringGenerator.GenerateRandomString(5)}";
            DataId = data.Id;
            CraftData = data;
        }

        public void EnableCraftable(bool condition)
        {
            _isCraftable = condition;
        }

        public void SetHasMaterial(bool condition)
        {
            _isEnoughMaterial = condition;
        }
    }
}