using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[Serializable]
public struct ItemAmount
{
    public Collectable Item;
    [Range(1, 999)]
    public int Amount;
}

[CreateAssetMenu]
public class CraftingRecipe : ScriptableObject
{
    public new string name;
    public string info;
    public List<ItemAmount> materials;
    public List<ItemAmount> results;

    public bool CanCraft()
    {
        if (Inventory.instance.items.Count == 0) return false;

        //var inventoryItems = Inventory.instance.GetInventoryInItems();

        foreach (var material in materials)
        {
            if (!Inventory.instance.items.Contains(material.Item)) return false;
        }
        return true;
    }

    public void Craft()
    {

    }
}
