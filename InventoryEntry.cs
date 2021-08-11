using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryEntry
{
    public ItemPickup invEntry;
    public int stackSize;
    public int inventorySlot;
    public Sprite hbSprite;

    public InventoryEntry(int stackSize, ItemPickup invEntry,  Sprite hbSprite)
    {
        this.invEntry = invEntry;

        this.stackSize = stackSize;
        this.inventorySlot = 0;
        this.hbSprite = hbSprite;
    }
}
