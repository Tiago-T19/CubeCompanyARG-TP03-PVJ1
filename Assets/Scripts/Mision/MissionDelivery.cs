using UnityEngine;

public class MissionDelivery : MonoBehaviour
{
    [Header("Mision")]
    [SerializeField] private MissionManager missionManager;

    public void Delivery()
    {
        missionManager.DeliverObjects(); // Entrega el objeto de mision
    }
}
