using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; } // ScoreManager.Instance.AddPoints((int)points);

    private int currentScore; // Puntos actuales del jugador
    public int CurrentScore => currentScore;

    int highScore; // Puntos maximos del jugador
    public int HighScore => highScore;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        currentScore = 0;
        highScore = PlayerPrefs.GetInt("highScore", 0); // Cargamos el record guardado en PlayerPrefs
    }

    public void AddPoints(int points) // Suma puntos
    {
        currentScore += points;

        if (currentScore < 0) currentScore = 0; // Evita que sea negativo

        if (currentScore > highScore)
        {
            highScore = currentScore; // Actualizamos el record
            PlayerPrefs.SetInt("highScore", highScore); // Guardamos el record en PlayerPrefs
            PlayerPrefs.Save(); // Guardamos los cambios en PlayerPrefs

            Debug.Log($"<color=green><b>¡Nuevo record!</b></color>");
        }

        Debug.Log($"<color=yellow><b>Puntos: <color=white>{currentScore}</b></color></color>");
        Debug.Log($"<color=yellow><b>Mejor Puntaje: <color=white>{highScore}</b></color></color>");
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }
}
