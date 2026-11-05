using UnityEngine;
using TMPro;

public class HUDManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textoVida;
    [SerializeField] private TextMeshProUGUI textoPuntos;

    void OnEnable()
    {
        HealthSystem.OnVidaCambiada += ActualizarVidaFloat;
        if (GameManager.Instance != null)
        {
            ActualizarVida((int)GameManager.Instance.vidaActual);
            ActualizarPuntos(GameManager.Instance.puntajeAcumulado);
        }
    }

    void OnDisable()
    {
        HealthSystem.OnVidaCambiada -= ActualizarVidaFloat;
    }

    void ActualizarVidaFloat(float actual, float max) => ActualizarVida((int)actual);
    public void ActualizarPuntos(int p) => textoPuntos.text = $"PUNTOS: {p}";
    public void ActualizarVida(int v) => textoVida.text = $"VIDA: {v}";
}