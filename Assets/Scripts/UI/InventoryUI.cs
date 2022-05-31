using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    #region Singleton
    public static InventoryUI instance = null;

    void Awake()
    {
        if (instance == null) instance = this;
        else if (instance == this)
        {
            Debug.LogWarning("Multiple inventories!");
            Destroy(gameObject);
        }
    }
    #endregion

    Inventory inventory;
    public GameObject inventoryPanel;

    public Transform bagSlotsParent;
    public Transform EquipLeftSlotsParent;
    public Transform EquipRightSlotsParent;
    public InventorySlot[] bagSlots;
    public InventorySlot[] equipedSlots;
    public GameObject inventorySlot;
    public RectTransform subMenu;
    public RectTransform consumableSubMenu;
    private InventorySlot subMenuChoiceSlot;
    public delegate void OnItemEquiped();
    public OnItemEquiped onItemEquipedCallback;

    [SerializeField] private WindowCharachterPortrait WindowCharachterPortrait;
    [SerializeField] private Transform characterTransform;

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        WindowCharachterPortrait.Show(characterTransform);

        inventory = Inventory.instance;

        equipedSlots = EquipLeftSlotsParent.GetComponentsInChildren<InventorySlot>().Concat(EquipRightSlotsParent.GetComponentsInChildren<InventorySlot>()).ToArray();
        bagSlots = bagSlotsParent.GetComponentsInChildren<InventorySlot>();

        LoadBagsSlots();
    }

    private void LoadBagsSlots()
    {
        for (int i = 0; i < inventory.space; i++)
        {
            GameObject newInventorySlot = Instantiate(inventorySlot);
            newInventorySlot.transform.SetParent(bagSlotsParent.transform, false);
            newInventorySlot.transform.localScale = new Vector2(1, 1);
            newInventorySlot.GetComponent<InventorySlot>().slotType = InventorySlot.SlotType.BAG;
        }
        bagSlots = bagSlotsParent.GetComponentsInChildren<InventorySlot>();
    }


    public void UnequipAll()
    {
        foreach (var equipedSlot in equipedSlots)
        {
            equipedSlot.ClearSlot();
        }
    }

    public void OpenInventory()
    {
        if (inventoryPanel != null) { 
            inventoryPanel.SetActive(!inventoryPanel.activeSelf);
            WindowCharachterPortrait.Show(characterTransform);
        }       
    }

    public void Add(Collectable item)
    {
        foreach (var slot in bagSlots)
        {
            if (!slot.isBusy)
            {
                slot.AddItem(item);
                break;
            }            
        }
    }

    public void EquipCheck(InventorySlot oldSlot, InventorySlot newSlot, InventorySlotItem draggedItem)
    {        
        if (newSlot.slotType == InventorySlot.SlotType.EQUIP || oldSlot.slotType == InventorySlot.SlotType.EQUIP)
        {
            if (oldSlot.slotType == InventorySlot.SlotType.EQUIP) newSlot.Unequip();

            equipedSlots.Where(x => x.equipType == draggedItem.item.equipType).First().item = oldSlot.slotType != InventorySlot.SlotType.EQUIP ? draggedItem.item : null;

            if (newSlot.slotType == InventorySlot.SlotType.EQUIP) newSlot.Equip();

            if (onItemEquipedCallback != null)
                onItemEquipedCallback.Invoke();
        }
    }

    public void OpenSubmenu(InventorySlot item)
    {
        subMenu.gameObject.SetActive(true);

        subMenu.transform.position = Input.mousePosition;
        subMenuChoiceSlot = item;
    }

    public void OpenConsumableSubmenu(InventorySlot item)
    {
        consumableSubMenu.gameObject.SetActive(true);

        consumableSubMenu.transform.position = Input.mousePosition;
        subMenuChoiceSlot = item;
    }

    public void HideSubmenu()
    {
        subMenu.gameObject.SetActive(false);
        consumableSubMenu.gameObject.SetActive(false);
        subMenuChoiceSlot = null;
        Debug.Log("submenu hid");
    }

    public void UseItemFromSubmenu()
    {
        if (subMenuChoiceSlot.item.Use())
        {
            subMenuChoiceSlot.ClearSlot();
            HideSubmenu();
        }
    }

    public void DropItemFromSubmenu()
    {
        Inventory.instance.DropItem(subMenuChoiceSlot.item);
        subMenuChoiceSlot.ClearSlot();
        HideSubmenu();
    }
}
