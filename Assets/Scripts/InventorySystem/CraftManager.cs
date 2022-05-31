using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftManager : MonoBehaviour
{
    #region Singleton
    public static CraftManager instance = null;
    public CraftingRecipe currentRecipe = null;
    void Awake()
    {
        if (instance == null) instance = this;
        else if (instance == this)
        {
            Debug.LogWarning("Multiple CraftManagers!");
            Destroy(gameObject);
        }
    }
    #endregion


    public List<CraftingRecipe> craftingRecipes = new List<CraftingRecipe>();

    public void Craft()
    {
        if (currentRecipe.CanCraft()) { 
            foreach (var material in currentRecipe.materials)
            {
                Inventory.instance.Remove(material.Item);
            }

            foreach (var result in currentRecipe.results)
            {
                Inventory.instance.Add(result.Item);
            }
        }
        else
        {
            Debug.Log($"No materials for {currentRecipe.name}");
        }
    }
}
