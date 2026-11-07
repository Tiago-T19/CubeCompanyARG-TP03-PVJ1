using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    public static EquipmentManager Instance;

    public Transform handPoint;

    private GameObject currentObject;

    private void Awake()
    {
        Instance = this;
    }

    public void Equip(ItemData item)
    {
        if (currentObject != null)
        {
            Destroy(currentObject);
        }

        if (item.prefab != null)
        {
            currentObject = Instantiate(
                item.prefab,
                handPoint
            );

            currentObject.transform.localPosition = Vector3.zero;
            currentObject.transform.localRotation = Quaternion.identity;
        }
    }
}