using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShoppingManager : MonoBehaviour
{
    #region Singleton
    public static ShoppingManager instance = null;
    void Awake()
    {
        if (instance == null) instance = this;
        else if (instance == this)
        {
            Debug.LogWarning("Multiple ShoppingManagers!");
            Destroy(gameObject);
        }
    }
    #endregion


    private List<InventorySlot> buySlots = null;
    private List<InventorySlot> paySlots = null;
    public Transform buyPanel;
    public Transform payPanel;
    public GameObject inventorySlot;
    public List<Collectable> collectablesToSell;
    public InventorySlot selectedSlot;
    private Inventory inventory;
    private StatsManager statsManager;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        inventory = Inventory.instance;
        statsManager = StatsManager.instance;

        paySlots = payPanel.GetComponentsInChildren<InventorySlot>().ToList();
        LoadShop();
    }

    public void Buy()
    {       
        if (selectedSlot != null)
        {
            if (statsManager.CanBuy(selectedSlot.item)) { 
                if (inventory.Add(selectedSlot.item))
                {
                    statsManager.gold -= selectedSlot.item.buyPrice;
                    selectedSlot.ClearSlot();
                }
            }
        }           
    }


    private void ClearBuyPanel()
    {
        foreach (Transform child in buyPanel.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }
    private void LoadShop()
    {
        ClearBuyPanel();
        foreach (var item in collectablesToSell)
        {
            GameObject newInventorySlot = Instantiate(inventorySlot);
            newInventorySlot.transform.SetParent(buyPanel.transform, false);
            newInventorySlot.transform.localScale = new Vector2(1, 1);
            newInventorySlot.GetComponent<InventorySlot>().AddItem(item);
            newInventorySlot.GetComponent<InventorySlot>().slotType = InventorySlot.SlotType.SHOP;
        }
    }
}
