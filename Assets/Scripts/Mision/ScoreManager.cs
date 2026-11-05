using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; } // ScoreManager.Instance.AddPoints(points);

    private int currentScore; // Puntos actuales del jugador
    public int CurrentScore => currentScore;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void AddPoints(int points) // Suma puntos
    {
        currentScore += points;

        if (currentScore < 0) currentScore = 0; // Evita que sea negativo

        Debug.Log($"<color=yellow><b>Puntos: <color=white>{currentScore}</b></color></color>");
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }
}
