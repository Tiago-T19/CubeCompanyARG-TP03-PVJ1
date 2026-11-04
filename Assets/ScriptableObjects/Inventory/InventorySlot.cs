using UnityEngine;

public class InventorySlot 
{
    public ItemData itemData;
    public int quantity;
    public InventorySlot(ItemData data, int qty)
    {
        this.itemData = data;
        this.quantity = qty;
    }
}
