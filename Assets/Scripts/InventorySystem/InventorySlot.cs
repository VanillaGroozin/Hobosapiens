using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static Collectable;

public class InventorySlot : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    StatsManager statsManager;
    InventoryUI inventoryUI;
    public Image icon;
    public Collectable item;
    public GameObject inventoryItem;
    public EquipType equipType;
    public SlotType slotType;
    public bool isBusy = false;
    public enum SlotType
    {
        SHOP = 1,
        LOOT = 2,
        BAG = 3,
        EQUIP = 4
    }

    void Start()
    {
        
        statsManager = StatsManager.instance;
        inventoryUI = InventoryUI.instance;
    }

    private void Update()
    {
        if (GetComponent<Button>() != null)
            GetComponent<Button>().enabled = isBusy;
    }

    public void AddItem (Collectable item)
    {
        this.item = item;

        //Add GO child to slot
        GameObject newInventoryItem = Instantiate(inventoryItem);
        newInventoryItem.transform.SetParent(transform, false);
        newInventoryItem.transform.localScale = new Vector2(.45f, .45f);
        newInventoryItem.GetComponentInChildren<Image>().sprite = item.item.icon;
        newInventoryItem.GetComponentInChildren<InventorySlotItem>().item = item;
        isBusy = true;
    }

    public void ClearSlot()
    {
        item = null;
        isBusy = false;
        foreach (Transform child in transform) Destroy(child.gameObject);       
    }

    //move item from slot to another
    public void OnDrop(PointerEventData eventData)
    {
        var oldSlot = eventData.pointerDrag.GetComponentInParent<InventorySlot>();
        var newSlot = this;
        var draggedItem = eventData.pointerDrag.GetComponent<InventorySlotItem>();

        if (draggedItem.GetComponent<InventorySlotItem>().item.isConsumable && newSlot.slotType == SlotType.EQUIP) return;

        //when stealing
        if (oldSlot.slotType == SlotType.SHOP && (newSlot.slotType == SlotType.BAG || newSlot.slotType == SlotType.EQUIP))
        {
            if (!StatsManager.instance.IsSkillUnlocked(StatsManager.SkillType.Stealer)) return;
        }


        if (oldSlot.slotType == SlotType.LOOT && newSlot.slotType != SlotType.LOOT)
            Inventory.instance.items.Add(draggedItem.GetComponent<InventorySlotItem>().item);
        else if (oldSlot.slotType != SlotType.LOOT && newSlot.slotType == SlotType.LOOT)
            Inventory.instance.items.Remove(draggedItem.GetComponent<InventorySlotItem>().item);


        if (eventData.pointerDrag != null && draggedItem.CanFitInSlot(this))
        {
            oldSlot.isBusy = false;
            newSlot.isBusy = true;

            newSlot.item = oldSlot.item;
            oldSlot.item = null;

            //Move child to new parent
            eventData.pointerDrag.GetComponent<RectTransform>().transform.SetParent(transform);
            eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);

            inventoryUI.EquipCheck(oldSlot, newSlot, draggedItem);
        }
    }

    public void Equip()
    {
        if (item.item == null || item.item.stats.Count < 1) return;

        foreach (var stat in this.item.item.stats)
        {
            if (!object.Equals(stat, null))
                statsManager.ModifyStat(stat.name, "+" + stat.GetValue().ToString());
        }
    }

    public void Unequip()
    {
        if (item.item == null || item.item.stats.Count < 1) return;

        foreach (var stat in this.item.item.stats)
        {
            if (!object.Equals(stat, null))
                statsManager.ModifyStat(stat.name, "-" + stat.GetValue().ToString());
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Tooltip.HideTooltip_Static();

        Debug.Log("OnPointerExit of Inventory slot fired");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!object.Equals(item, null))
        {
            string statsStr = string.Empty;
            foreach (var stat in item.item.stats)
            {
                if (stat.GetValue() > 0) {
                    statsStr += $"{stat.name}: +{stat.GetValue()}\n";
                }
            }
            Tooltip.ShowTooltip_Static($"{item._name}", $"{statsStr}Price: {(slotType == SlotType.SHOP ? item.buyPrice : item.sellPrice)}");
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        inventoryUI.HideSubmenu();

        if (slotType == SlotType.EQUIP) return;


        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (slotType == SlotType.SHOP)
            {
                ShoppingManager.instance.selectedSlot = this;              
            }
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (!isBusy) return;

            if (item.isConsumable)
            {
                inventoryUI.OpenConsumableSubmenu(this);
                Debug.Log("is Consumable");
            }
            else {
                inventoryUI.OpenSubmenu(this);
                Debug.Log("is not Consumable"); 
            }
        }       
    }
}
