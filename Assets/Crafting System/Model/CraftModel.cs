using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;

namespace Tools.Inventory.Crafting
{
    [System.Serializable]
    public class CraftModel
    {
        public List<CraftItem> Craftables => craftableItems;

        [ShowInInspector]
        private List<CraftItem> craftableItems = new();

        public CraftModel(CraftDatabase db)
        {
            for (int i = 0; i < db.Items.Count; i++)
            {
                var craftItem = new CraftItem(db.GetItemData(i));
                craftableItems.Add(craftItem);
            }
        }

        public CraftResult TryCraftItem(CraftItem item)
        {
            if (!item.IsCraftable)
                return null;

            return item.CraftData.CraftResult;
        }
    }
}


/*
 For crafting it can :

1. Get the list of items that can be crafted
2. Get the list of items that can be crafted with the given items
3. Get item result based on given items
 
 */