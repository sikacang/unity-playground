using System;
using TMPro;
using Tools.Inventory.Crafting;
using UnityEngine;
using UnityEngine.UI;

public class RequiredItemSlot : MonoBehaviour
{
    public TextMeshProUGUI itemAmountText;
    public Image itemIconImage;

    public void Setup(Sprite icon)
    {
        itemIconImage.sprite = icon;        
    }

    public void SetAmount(int amount, int currentAmountInInventory)
    {
        itemAmountText.text = $"{amount}/{currentAmountInInventory}";
    }
}
