using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Collectable;
using static InventorySlot;

public class InventorySlotItem : MonoBehaviour
{
    public Collectable item;
    public bool CanFitInSlot(InventorySlot inventorySlot)
    {
        return ((inventorySlot.equipType == this.item.equipType) || inventorySlot.slotType != SlotType.EQUIP) && !inventorySlot.isBusy;
    }
}
