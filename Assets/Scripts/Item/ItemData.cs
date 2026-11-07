using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "Scriptable Objects/ItemData")]
public class ItemData : ScriptableObject
{
    public string itemID;
    public string itemName;
    public Sprite icon;
    public string description;
    public int maxStackSize = 3;

    public GameObject prefab;
    // Consumible botiquin 
    public bool isConsumable;
    public int healAmount;
}