using UnityEngine;
using Tools.Inventory;
using Sirenix.OdinInspector;

public class InventoryDebugger : MonoBehaviour
{
    private BaseInventoryModel inventory;

    private void Start()
    {
        var inventory = GetComponent<InventoryController>();
        this.inventory = inventory.inventory;
    }

    [Button("Add Item")]
    private void AddItem(ItemData data, int amount)
    {
        inventory.AddItem(data, amount);
    }
}
