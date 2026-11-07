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