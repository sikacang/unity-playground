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

        public Item(ItemData data, int quantity)
        {
            Id = $"{RandomStringGenerator.GenerateRandomString(5)}";
            DataId = data.Id;
            Data = data;
            Quantity = quantity;
        }

        public int AddQuantity(int amount)
        {
            if(Quantity + amount > Data.MaxStack)
            {
                int leftOver = Quantity + amount - Data.MaxStack;
                Quantity = Data.MaxStack;
                return leftOver;
            }
            else
            {
                Quantity += amount;
                return 0;
            }
        }

        public bool ReduceQuantity(int amount)
        {
            Quantity -= amount;

            if (Quantity <= 0)
            {
                Quantity = 0;
                return true;
            }

            return false;
        }
    
        public bool CanAddQuantity()
        {
            return Quantity < Data.MaxStack;
        }
    }
}