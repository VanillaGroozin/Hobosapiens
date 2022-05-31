using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static LootableObject;

public class LootManager : MonoBehaviour
{
    public GameObject lootPanel;
    public GameObject lootWindow;
    public GameObject inventoryPanel;
    public GameObject inventorySlot;

    public Collectable collectable1;
    public Collectable collectable2;
    public Collectable collectable3;

    //0 - common, 1 - rare, 2 - epic, 3 - legendary
    private int[] TrashCanLootChance = { 60, 30, 10, 0 };
    private int[] EpicTrashCanLootChance = { 50, 40, 15, 0 };

    public List<GameObject> commonCollectables = new List<GameObject>();
    public List<GameObject> rareCollectables = new List<GameObject>();
    public List<GameObject> epicCollectables = new List<GameObject>();

    #region Singleton
    public static LootManager instance = null;


    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if (instance == null) instance = this;
        else if (instance == this)
        {
            Debug.LogWarning("Multiple LootManagers!");
            Destroy(gameObject);
        }
    }
    #endregion

    internal void StopLooting()
    {
        lootWindow.SetActive(false);
    }

    private void ClearLootPanel()
    {
        foreach (Transform child in lootPanel.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }

    public void Loot(LootableObject lootableObject, LootableType lootableType)
    {
        ClearLootPanel();

        lootWindow.SetActive(true);
        //inventoryPanel.SetActive(true);


        if (!lootableObject.lootGenerated)
            GenerateLoot(lootableObject, lootableType);
        else LoadLoot(lootableObject);

        lootableObject.lootGenerated = true;
    }

    private void GenerateLoot(LootableObject lootableObject, LootableType lootableType)
    {
        int total = 0;

        //getting chances of loot from type
        int[] lootChance = lootableType switch
        {
            LootableType.TrashCan => TrashCanLootChance,
            LootableType.EpicTrashCan => EpicTrashCanLootChance,
            _ => null
        };

        foreach (var chance in lootChance)
        {
            total += chance;
        }

        int lootableObjectCells = UnityEngine.Random.Range(1, 9);
        for (int i = 0; i < lootableObjectCells; i++)
        {
            //randomize the loot
            int randomNum = UnityEngine.Random.Range(0, total);

            foreach (var chance in lootChance)
            {
                if (randomNum <= chance)
                {
                    //got chance and create loot depending on chance and loot type
                    var lootType = Array.IndexOf(lootChance, chance);
                    CreateLoot(lootableObject, lootType);
                    break;
                }
                else
                {
                    randomNum -= chance;
                }
            }
        }
    }

    private void CreateLoot(LootableObject lootableObject, int lootType)
    {
        GameObject newInventorySlot = Instantiate(inventorySlot);
        newInventorySlot.GetComponent<InventorySlot>().slotType = InventorySlot.SlotType.LOOT;
        newInventorySlot.transform.SetParent(lootPanel.transform, false);
        newInventorySlot.transform.localScale = new Vector2(1, 1);

        Collectable newCollectable = GetRandomCollectable(lootType);

        //save for load
        lootableObject.collectables.Add(newCollectable);

        newInventorySlot.GetComponent<InventorySlot>().AddItem(newCollectable);
    }

    private void LoadLoot(LootableObject lootableObject)
    {
        foreach (var collectable in lootableObject.collectables)
        {
            GameObject newInventorySlot = Instantiate(inventorySlot);
            newInventorySlot.transform.SetParent(lootPanel.transform, false);
            newInventorySlot.transform.localScale = new Vector2(1, 1);
            newInventorySlot.GetComponent<InventorySlot>().slotType = InventorySlot.SlotType.LOOT;
            newInventorySlot.GetComponent<InventorySlot>().AddItem(collectable);
        }       
    }


    private Collectable GetRandomCollectable(int lootType)
    {
        List<GameObject> possibleCollectables = new List<GameObject>();

        if (lootType == 0)
            possibleCollectables = commonCollectables;
        else if (lootType == 1)
            possibleCollectables = rareCollectables;
        else if (lootType == 2)
            possibleCollectables = epicCollectables;


        int rand = UnityEngine.Random.Range(0, possibleCollectables.Count);
        return possibleCollectables[rand].GetComponent<Collectable>();
    }
}
