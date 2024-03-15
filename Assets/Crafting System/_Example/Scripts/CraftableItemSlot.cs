using System;
using Tools.Inventory.Crafting;
using UnityEngine;
using UnityEngine.UI;

public class CraftableItemSlot : MonoBehaviour
{
    public CraftItem CraftItem { get; private set; }

    public Transform requiredItemContainer;
    public RequiredItemSlot requiredItemPrefab;

    public Button craftButton;

    public Action<CraftItem> OnCraftPressed;

    private void OnDestroy()
    {
        OnCraftPressed = null;
    }

    public void Setup(CraftItem item)
    {
        CraftItem = item;
        PopulateRequiredItems();
    }

    public void EnableCraft(bool condition)
    {
        craftButton.interactable = condition;
    }

    private void PopulateRequiredItems()
    {
        DestroyChild();

        foreach (var material in CraftItem.CraftData.Materials)
        {
            var requiredItem = Instantiate(requiredItemPrefab, requiredItemContainer);
            requiredItem.Setup(material.ItemData.Icon);
            // requiredItem.SetAmount(material.Amount, material.CurrentAmountInInventory);
        }
    }

    private void DestroyChild()
    {
        foreach (Transform child in requiredItemContainer)
        {
            Destroy(child.gameObject);
        }
    }
}
