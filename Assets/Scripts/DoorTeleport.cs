using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorTeleport : MonoBehaviour
{
    [Header("Configuración de Destino")]
    [SerializeField] private string targetSceneName;

    [Header("Identificación")]
    [SerializeField] private string playerTag = "Player";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            if (!string.IsNullOrEmpty(targetSceneName))
            {
                PlayerPrefs.SetString("NextScene", targetSceneName);
                SceneManager.LoadScene("PantallaDeCarga");


            }
            else
            {
                Debug.LogError("No asignaste el nombre de la escena en el Inspector.");
            }
        }
    }
}