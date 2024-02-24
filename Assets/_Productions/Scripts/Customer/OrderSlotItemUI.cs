using Sirenix.OdinInspector;
using TMPro;
using Tools.Inventory;
using UnityEngine;
using UnityEngine.UI;

namespace Waroeng
{
    public class OrderSlotItemUI : MonoBehaviour
    {
        [SerializeField]
        private Image containerImage;
        [SerializeField]
        private Image iconImage;
        [SerializeField]
        private TextMeshProUGUI amountText;

        [Header("State Sprite")]
        [SerializeField]
        [InfoBox("0 = Complete, 1 = Not Complete")]
        private Sprite[] spriteStates;

        public void SetOrderItem(ItemData itemData, int amount)
        {
            iconImage.color = Color.white;
            iconImage.sprite = itemData.Icon;
            amountText.text = amount.ToString();
            containerImage.sprite = amount == 0 ? spriteStates[0] : spriteStates[1];
        }

        public void HideOrderItem()
        {
            iconImage.color = Color.clear;
            amountText.text = "";
            containerImage.sprite = spriteStates[1];        
        }
    }
}