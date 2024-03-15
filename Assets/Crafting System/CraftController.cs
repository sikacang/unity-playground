using Tools.Inventory;
using Tools.Inventory.Crafting;
using UnityEngine;

public class CraftController : MonoBehaviour
{
    public CraftDatabase craftDatabase;
    public CraftModel craftModel;
    public CraftingPage craftPage;

    public InventoryController inventoryController;
    public BaseInventoryModel inventory;

    private void Start()
    {
        craftModel = new CraftModel(craftDatabase);

        craftPage.PrepareCraftModel(craftModel);
        craftPage.OnRequestCraft.AddListener(OnRequestCraft);

        inventory = inventoryController.inventory;

        foreach (var craftItem in craftModel.Craftables)
        {
            craftItem.SetHasMaterial(IsCraftable(craftItem));
        }
    }

    public bool IsCraftable(CraftItem craftItem)
    {
        bool hasEnoughMaterial = true;

        var craftMaterials = craftItem.CraftData.Materials;
        for (int i = 0; i < craftMaterials.Length; i++)
        {
            var material = craftMaterials[i];
            var hasItem = inventory.TryGetItem(out var item, material.ItemData);

            hasEnoughMaterial &= hasItem && item.Quantity >= material.Amount;

            if (hasEnoughMaterial == false)
                break;
        }

        return craftItem.IsCraftable && hasEnoughMaterial;
    }

    private void OnRequestCraft(CraftItem craftItem)
    {
        if (IsCraftable(craftItem) == false)
            return;

        var result = craftModel.TryCraftItem(craftItem);
        if (result != null)
        {
            inventory.AddItem(result.ItemData, result.Amount);
        }
    }
}
