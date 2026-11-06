using UnityEngine;

public class MissionManager : MonoBehaviour
{
    [Header("Mision")]
    [SerializeField] private int totalMissionObjects = 5; // Total requerido para completar la mision

    private int recoveredObjects; // Objetos recogidos
    public int RecoveredObjects => recoveredObjects;
    private int deliveredObjects; // Objetos entregados
    public int DeliveredObjects => deliveredObjects;
    public int PendingObjects => recoveredObjects - deliveredObjects;

    // AllObjectsDelivered sera TRUE cuando la cantidad de objetos entregados sea mayor o igual al total requerido
    public bool AllObjectsDelivered => deliveredObjects >= totalMissionObjects;

    private void OnEnable()
    {
        MissionObject.OnMissionObjectCollected += RegisterCollectedObject; // Nos suscribimos al evento de recoleccion
    }

    private void OnDisable()
    {
        MissionObject.OnMissionObjectCollected -= RegisterCollectedObject; // Nos desuscribimos del evento de recoleccion
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q)) ShowMissionStatus();
    }

    private void RegisterCollectedObject()
    {
        if (recoveredObjects >= totalMissionObjects) return;

        recoveredObjects++; // Suma un objeto recogido

        Debug.Log($"<color=purple>Objetos recuperados: <color=white>{recoveredObjects}/{totalMissionObjects}</color></color>");
    }

    public void ShowMissionStatus()
    {
        Debug.Log($"<color=purple><b>Objetos: <color=white>{deliveredObjects}/{totalMissionObjects}</color></b></color>");
    }

    public void DeliverObjects()
    {
        if (PendingObjects <= 0) return;

        deliveredObjects = recoveredObjects; // Registra la entrega

        Debug.Log($"<color=green>Objetos entregados: <color=white>{deliveredObjects}/{totalMissionObjects}</color></color>");

        if (AllObjectsDelivered)
        {
            Debug.Log("<color=green>Mision completada</color>");
        }
    }
}
