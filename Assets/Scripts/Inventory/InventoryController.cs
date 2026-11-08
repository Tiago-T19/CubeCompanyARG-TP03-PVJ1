//using System;
using UnityEngine;
using UnityEngine.UIElements;
using UnityCursor = UnityEngine.Cursor;

public class InventoryController : MonoBehaviour
{
    private VisualElement root;
    private bool inventoryOpen = false;

    // Slots visuales
    private VisualElement slot1;
    private VisualElement slot2;
    private VisualElement slot3;

    private void Start()
    {
        root = GetComponent<UIDocument>().rootVisualElement;
        // Buscamos los contenedores de los slots
        slot1 = root.Q<VisualElement>("Slot1");
        slot2 = root.Q<VisualElement>("Slot2");
        slot3 = root.Q<VisualElement>("Slot3");


        InventoryManager.Instance.OnInventoryChanged += UpdateSelectedVisual;

        // Resaltamos el primer slot
        UpdateSelectedVisual();

        root.style.display = DisplayStyle.None;

        UnityCursor.lockState = CursorLockMode.Locked;
        UnityCursor.visible = false;
    }

    private void OnDestroy()
    {
        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.OnInventoryChanged -= UpdateSelectedVisual;
        }
    }

    private void UpdateSelectedVisual()
    {
        slot1.RemoveFromClassList("slotSelected");
        slot2.RemoveFromClassList("slotSelected");
        slot3.RemoveFromClassList("slotSelected");

        switch (InventoryManager.Instance.SelectedSlot)
        {
            case 0:
                slot1.AddToClassList("slotSelected");
                break;

            case 1:
                slot2.AddToClassList("slotSelected");
                break;

            case 2:
                slot3.AddToClassList("slotSelected");
                break;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            inventoryOpen = !inventoryOpen;

            root.style.display = inventoryOpen
                ? DisplayStyle.Flex
                : DisplayStyle.None;

            if (inventoryOpen)
            {
                UnityCursor.lockState = CursorLockMode.None;
                UnityCursor.visible = true;
            }
            else
            {
                UnityCursor.lockState = CursorLockMode.Locked;
                UnityCursor.visible = false;
            }
        }
    }
}