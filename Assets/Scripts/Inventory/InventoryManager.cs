using System;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    [SerializeField] private int inventorySize = 3;
    private InventorySlot[] slots;

    private int selectedSlot = 0;
    public int SelectedSlot => selectedSlot;

    public Action OnInventoryChanged;

    public void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        slots = new InventorySlot[inventorySize];
    }

    private void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll > 0f)
        {
            ChangeSelectedSlot(-1);
        }

        if (scroll < 0f)
        {
            ChangeSelectedSlot(1);
        }

        if (Input.GetMouseButtonDown(0))
        {
            UseSelectedItem();
        }
    }
    public bool AddItem(ItemData itemData, int quantity)
    {
        Debug.Log("Agregando: " + itemData.itemName);

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

                if (i == selectedSlot)
                {
                    EquipmentManager.Instance.Equip(itemData);
                }

                return true;
            }
        }
        return false; // No hay espacio en el inventario
    }

    public InventorySlot[] GetSlots()
    {
        return slots;
    }


    public void ChangeSelectedSlot(int direction)
    {
        selectedSlot += direction;

        if (selectedSlot >= slots.Length)
            selectedSlot = 0;

        if (selectedSlot < 0)
            selectedSlot = slots.Length - 1;

        EquipSelectedItem();

        OnInventoryChanged?.Invoke();
    }


    private void EquipSelectedItem()
    {
        if (slots[selectedSlot] == null)
            return;

        EquipmentManager.Instance.Equip(
            slots[selectedSlot].itemData
        );
    }

    private void UseSelectedItem()
    {
        if (slots[selectedSlot] == null)
            return;

        ItemData item = slots[selectedSlot].itemData;

        if (!item.isConsumable)
            return;

        GameObject playerObj =
            GameObject.FindGameObjectWithTag("Player");

        if (playerObj == null)
            return;

        HealthSystem player =
            playerObj.GetComponent<HealthSystem>();

        if (player != null)
        {
            player.Heal(item.healAmount);

            Debug.Log("Curado +" + item.healAmount);
        }

        slots[selectedSlot].quantity--;

        if (slots[selectedSlot].quantity <= 0)
        {
            slots[selectedSlot] = null;
        }

        OnInventoryChanged?.Invoke();
    }
}