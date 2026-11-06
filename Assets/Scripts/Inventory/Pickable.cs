/*using UnityEngine;

public class PickAble : MonoBehaviour, IInteractable
{
    [SerializeField] private ItemData itemData;
    [SerializeField] private int quantity = 1;
    [SerializeField] private bool destroyOnPickUp = true;

    private bool _pickedUp = false;

    public ItemData ItemData => itemData;
    public int Quantity => quantity;

    public bool canInteract()
    {
        return !_pickedUp;
    }

    public void Interact()
    {
        if (TryPickUp())
        {
            InventoryManager.Instance.AddItem(itemData, 1);
        }
    }

    public bool TryPickUp()
    {
        if (_pickedUp || itemData == null)
            return false;

        _pickedUp = true;
        OnPickedUp();
        return true;
    }

    public void OnPickedUp()
    {
        bool added = InventoryManager.Instance.AddItem(itemData, quantity);
        if(added)
        {
            Debug.Log($"<color=green>Picked up {quantity} x {itemData.itemName}</color>");
            if (destroyOnPickUp)
                Destroy(gameObject);
        }

        if (destroyOnPickUp)
            Destroy(gameObject);
    }
}*/


using UnityEngine;

public class PickAble : MonoBehaviour, IInteractable
{
    [SerializeField] private ItemData itemData;
    [SerializeField] private int quantity = 1;
    [SerializeField] private bool destroyOnPickUp = true;

    private bool pickedUp = false;

    public ItemData ItemData => itemData;
    public int Quantity => quantity;

    public bool canInteract()
    {
        return !pickedUp;
    }

    public void Interact()
    {
        TryPickUp();
    }

    public bool TryPickUp()
    {
        if (pickedUp || itemData == null)
            return false;

        pickedUp = true;

        if (destroyOnPickUp)
            Destroy(gameObject);

        return true;
    }
}