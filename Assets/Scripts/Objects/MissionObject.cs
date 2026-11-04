using UnityEngine;

public class MissionObject : MonoBehaviour
{
    // Prueba de interaccion
    public void Interact()
    {
        Debug.Log($"<color=green>Recogido</color>");

        Destroy(gameObject);
    }
}
