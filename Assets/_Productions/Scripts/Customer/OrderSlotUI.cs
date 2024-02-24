using UnityEngine;

namespace Waroeng
{
    public class OrderSlotUI : MonoBehaviour
    {
        [SerializeField]
        private OrderSlotItemUI[] orderSlots;

        private Order _order;

        public void SetOrder(Order order)
        {
            _order = order;
            RefreshSlots();
        }

        public void RefreshSlots()
        {
            for (int i = 0; i < orderSlots.Length; i++)
            {                
                if (i < _order.Items.Length)
                {
                    orderSlots[i].SetOrderItem(_order.Items[i].Item, _order.Items[i].Quantity);
                }
                else
                {
                    orderSlots[i].HideOrderItem();
                }
            }
        }
    }
}