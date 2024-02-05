using Cathei.BakingSheet;
using Cathei.BakingSheet.Examples;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataSheetContainer : SheetContainerBase
{
    public DataSheetContainer(Microsoft.Extensions.Logging.ILogger logger) : base(logger)
    {
    }

    public UnitSheet Units { get; private set; }
    public RewardSheet Rewards { get; private set; }
}

public class UnitSheet : Sheet<UnitSheet.Row>
{
    public class Row : SheetRow
    {
        public string Name { get; set; }
        public float Health { get; set; }
        public float Damage { get; set; }
    }
}

public class RewardSheet : Sheet<RewardSheet.Row>
{
    public class Row : SheetRowArray<Elem>
    {
    }

    public class Elem : SheetRowElem
    {
        public string ItemId { get; private set; }
        public string DropType { get; private set; }
        public string ItemType { get; private set; }
        public int Amount { get; private set; }
        public float Chance { get; private set; }
    }
}

public enum DropType
{
    Chance, Bonus
}

public enum ItemType
{
    Unit, Item, Recollection
}