using Tools.Inventory;

namespace Waroeng
{
    [System.Serializable]
    public class Order
    {
        public OrderItem[] Items;
        public float Time;
    }

    [System.Serializable]
    public class OrderItem
    {
        public ItemData Item;
        public int Quantity;
    }
}