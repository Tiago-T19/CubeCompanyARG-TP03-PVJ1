using UnityEngine;

public class GoalZone : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        // Si el Jefe sigue vivo, no te deja escapar
        if (GameManager.Instance != null && !GameManager.Instance.jefeFinalMuerto)
        {
            Debug.Log("Debes matar al Jefe Final para escapar");
            return;
        }

        Debug.Log("¡Victoria, has escapado!");
        GameManager.Instance.CambiarEstado(EstadoJuego.Victoria);
    }
}
