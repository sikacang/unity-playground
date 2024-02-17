using TMPro;
using Tools.Inventory.UI;
using Tools.Inventory;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Waroeng
{
    public class ShelfSlotUI : MonoBehaviour
    {
        public InventoryItem Item => _item;

        [Header("Slot Properties")]
        [SerializeField]
        private Image iconImage;
        [SerializeField]
        private TextMeshProUGUI amountText;
        [SerializeField]
        private Button slotButton;

        [SerializeField]
        private GameObject fillContainer;

        // Events
        public UnityEvent<ShelfSlotUI> OnSelectSlot;

        private InventoryItem _item;

        private void Awake()
        {
            slotButton.onClick.AddListener(() =>
            {
                OnSelectSlot?.Invoke(this);
            });
        }

        public void SetSlot(InventoryItem item)
        {
            if (item.IsEmpty)
            {
                ClearSlot();
                return;
            }

            _item = item;
            iconImage.sprite = item.Data.Icon;
            SetItemAmount(item.Quantity);

            fillContainer.SetActive(true);
            slotButton.interactable = true;
        }

        public void ClearSlot()
        {
            _item = null;
            iconImage.sprite = null;
            SetItemAmount(0);

            fillContainer.SetActive(false);
            slotButton.interactable = false;
        }

        public void SetItemAmount(int quantity)
        {
            amountText.text = quantity > 1 ? $"{quantity}" : "";
        }

        public void SetActive(bool condition)
        {
            gameObject.SetActive(condition);
        }
    }
}