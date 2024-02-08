using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tools.Inventory
{
    [CreateAssetMenu(menuName = "Inventory/ItemData")]
    public class ItemData : ScriptableObject
    {
        public int ID { get; private set; }

        public string Name;
        public string Description;
    }
}