using System;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    [SerializeField] private int inventorySize = 3;
    private InventorySlot[] slots;

    public Action OnInventoryChanged;

    public void Awake()
    {
       if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }
       Instance = this;
    slots = new InventorySlot[inventorySize];
    }

    public bool AddItem(ItemData itemData, int quantity)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] != null && slots[i].itemData == itemData)
            {
                slots[i].quantity += quantity;
                OnInventoryChanged?.Invoke();
                return true;
            }
        }



            for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] == null)
            {
                slots[i] = new InventorySlot(itemData, quantity);
                OnInventoryChanged?.Invoke();
                return true;
            }
        }
        return false; // No hay espacio en el inventario
    }
}
