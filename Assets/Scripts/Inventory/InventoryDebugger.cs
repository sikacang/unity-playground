using UnityEngine;
using Tools.Inventory;
using Sirenix.OdinInspector;

public class InventoryDebugger : MonoBehaviour
{
    [SerializeField]
    private Inventory inventory;

    [Button("Add Item")]
    private void AddItem(ItemData data, int amount)
    {
        inventory.AddItem(data, amount);
    }

    [Button("Remove Item")]
    private void RemoveItem(ItemData data)
    {
        inventory.RemoveItem(data);
    }

    [Button("Remove Item by Index")]
    private void RemoveItem(int index)
    {
        inventory.RemoveItem(index);   
    }

    [Button("Reduce Item")]
    private void ReduceItem(ItemData data, int amount)
    {
        
    }
}
