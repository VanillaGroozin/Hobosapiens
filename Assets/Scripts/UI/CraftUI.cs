using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;
using UnityEngine.UI;

public class CraftUI : MonoBehaviour
{
    public GameObject recipesPanel;
    public GameObject recipeObj;
    public GameObject craftSlotObj;

    public GameObject recipeInfo;
    public GameObject recipeTitle;

    public GameObject craftPanel;
    public GameObject materialsPanel;

    public Button craftBtn;

    public void OpenCrafts()
    {
        if (craftPanel != null) craftPanel.SetActive(!craftPanel.activeSelf);
    }

    private void Start()
    {
        LoadRecipesPanel();
    }
    public void LoadRecipesPanel()
    {
        foreach (var recipe in CraftManager.instance.craftingRecipes)
        {
            GameObject newRecipe = Instantiate(recipeObj);
            newRecipe.transform.localScale = new Vector2(1, 1);
            newRecipe.GetComponentInChildren<TextMeshProUGUI>().text = recipe.name; 
            newRecipe.transform.SetParent(recipesPanel.transform, false);
            newRecipe.name = recipe.name;                 
        }
        
        //newRecipe.GetComponentInChildren<Image>().sprite = item.icon;
        //newRecipe.GetComponentInChildren<InventorySlotItem>().item = item;
    }

    public void LoadRecipeFromBtnClick()
    {
        LoadRecipeByName(EventSystem.current.currentSelectedGameObject.name);
    }

    private void LoadRecipeByName(string recipeName)
    {
        var selectedRecipe = CraftManager.instance.craftingRecipes.Where(x => x.name == recipeName).FirstOrDefault();
        if (selectedRecipe != null)
        {
            CraftManager.instance.currentRecipe = selectedRecipe;

            recipeInfo.GetComponentInChildren<TextMeshProUGUI>().text = selectedRecipe.info;
            recipeTitle.GetComponentInChildren<TextMeshProUGUI>().text = selectedRecipe.name;

            //clear materials
            for (int i = 0; i < materialsPanel.transform.childCount; i++)
            {
                Transform child = materialsPanel.transform.GetChild(i);
                Destroy(child.gameObject);
            }
            //

            //add materials
            foreach (var material in selectedRecipe.materials)
            {
                GameObject newMaterial = Instantiate(craftSlotObj);
                newMaterial.transform.localScale = new Vector2(2, 2);
                newMaterial.transform.GetChild(0).GetComponentInChildren<Image>().sprite = material.Item.item.icon;
                newMaterial.transform.SetParent(materialsPanel.transform, false);
            }
            //


            //craftBtn.interactable = selectedRecipe.CanCraft();
        }
    }

    public void Craft()
    {
        CraftManager.instance.Craft();
    }
}
