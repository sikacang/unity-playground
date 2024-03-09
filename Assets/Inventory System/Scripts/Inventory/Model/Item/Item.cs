using Sirenix.OdinInspector;

namespace Tools.Inventory
{
    [System.Serializable]
    public class Item
    {
        [ShowInInspector]
        public string Id { get; private set; }

        [ShowInInspector]
        public string DataId { get; private set; }

        [ShowInInspector]
        public int Quantity { get; private set; }

        [ShowInInspector]
        public ItemData Data { get;private set; }

        [ShowInInspector]
        public bool IsEmpty { get; private set; }       

        public int Index { get; set; }

        public Item(ItemData data, int quantity)
        {
            Id = $"{RandomStringGenerator.GenerateRandomString(5)}";
            DataId = data.Id;
            Data = data;
            Quantity = quantity;
        }

        public int TryAddQuantity(int amount)
        {
            int leftOver = Quantity + amount - Data.MaxStack;

            if(Quantity + amount > Data.MaxStack)
            {
                Quantity = Data.MaxStack;
                return leftOver;
            }
            else
            {
                Quantity += amount;
                return 0;
            }
        }

        public bool TryReduceQuantity(int amount)
        {
            Quantity -= amount;

            if (Quantity <= 0)
            {
                Quantity = 0;
                return true;
            }

            return false;
        }

        public bool IsSameItem(string dataId)
        {
            return DataId.Equals(dataId);
        }
    
        public bool CanAddQuantity()
        {
            return Quantity < Data.MaxStack;
        }
    }
}