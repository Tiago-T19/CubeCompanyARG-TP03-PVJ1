using UnityEngine;
using UnityEngine.UIElements;

public class HotBarUIController : MonoBehaviour
{
    private UIDocument document;

    private Image slot1;
    private Image slot2;
    private Image slot3;

    private void Start()
    {
        document = GetComponent<UIDocument>();

        VisualElement root = document.rootVisualElement;

        slot1 = root.Q<Image>("slot01");
        slot2 = root.Q<Image>("slot02");
        slot3 = root.Q<Image>("slot03");

        Debug.Log("slot01: " + slot1);
        Debug.Log("slot02: " + slot2);
        Debug.Log("slot03: " + slot3);

        InventoryManager.Instance.OnInventoryChanged += UpdateUI;

        UpdateUI();
    }

    private void OnDestroy()
    {
        if (InventoryManager.Instance != null)
            InventoryManager.Instance.OnInventoryChanged -= UpdateUI;
    }

    private void UpdateUI()
    {
        if (InventoryManager.Instance == null)
            return;

        InventorySlot[] slots =
            InventoryManager.Instance.GetSlots();

        slot1.image = null;
        slot2.image = null;
        slot3.image = null;

        if (slots.Length > 0 && slots[0] != null)
        {
            slot1.image = slots[0].itemData.icon.texture;
        }

        if (slots.Length > 1 && slots[1] != null)
        {
            slot2.image = slots[1].itemData.icon.texture;
        }

        if (slots.Length > 2 && slots[2] != null)
        {
            slot3.image = slots[2].itemData.icon.texture;
        }
    }
}