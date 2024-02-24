using CustomExtensions;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Waroeng
{
    public class OrderItemUI : MonoBehaviour
    {
        public Order Order => _order;

        [Header("UI References")]
        [SerializeField]
        private OrderItemIconComponent[] orderItemIconComponents;

        private Order _order;

        public void SetOrder(Order order)
        {
            _order = order;        

            for (int i = 0; i < orderItemIconComponents.Length; i++)
            {
                if (i < order.Items.Length)
                {
                    orderItemIconComponents[i].Container.SetActive(true);
                    orderItemIconComponents[i].IconImage.color = Color.white;
                    orderItemIconComponents[i].IconImage.sprite = order.Items[i].Item.Icon;
                    orderItemIconComponents[i].QuantityText.text = order.Items[i].Quantity.ToString();
                }
                else
                {
                    orderItemIconComponents[i].Container.SetActive(false);
                    orderItemIconComponents[i].IconImage.color = Color.clear;
                    orderItemIconComponents[i].QuantityText.text = string.Empty;
                }
            }
        }

        [System.Serializable]
        public struct OrderItemIconComponent
        {
            public GameObject Container;
            public Image IconImage;
            public TextMeshProUGUI QuantityText;
        }
    }
}