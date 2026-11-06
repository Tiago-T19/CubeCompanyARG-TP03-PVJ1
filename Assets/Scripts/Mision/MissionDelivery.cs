using UnityEngine;

public class MissionDelivery : MonoBehaviour
{
    [Header("Mision")]
    [SerializeField] private MissionManager missionManager;
    [SerializeField] private GameObject door; // Puerta que se abre al completar la mision

    public void Delivery()
    {
        missionManager.DeliverObjects(); // Entrega el objeto de mision
        missionManager.ShowMissionStatus(); // Muestra el estado de la mision
        door.SetActive(false); // Abre la puerta
    }
}
