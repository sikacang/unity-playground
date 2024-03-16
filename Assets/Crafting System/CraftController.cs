using DependencyInjection;
using Tools.Inventory;
using Tools.Inventory.Crafting;
using UnityEngine;
using UnityServiceLocator;

public class CraftController : MonoBehaviour, IDependencyProvider
{    
    public CraftModel craftModel;
    public CraftDatabase craftDatabase;

    public InventoryController inventoryController;
    public BaseInventoryModel inventory;

    private void Awake()
    {
        ServiceLocator.Global.Register(this);
    }

    private void Start()
    {
        craftModel = new CraftModel(craftDatabase);
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

    [Provide]
    private CraftModel ProvideModel()
    {
        return craftModel;
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
