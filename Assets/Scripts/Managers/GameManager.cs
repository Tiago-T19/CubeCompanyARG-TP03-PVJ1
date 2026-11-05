using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public enum GameState { Start, Playing, Paused, Victory, GameOver }
public enum EstadoJuego { Inicio, Jugando, Pausa, Victoria, Derrota }

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Estado - Punto 5 TP")]
    public EstadoJuego estadoActual = EstadoJuego.Inicio;
    public GameState currentState = GameState.Start;

    // Eventos que espera el UIManager de tu profe
    public event Action<GameState> OnChangeState;
    public event Action<int> OnEnemyChange;
    public event Action<int> OnEnemyCountChanged;

    [Header("Datos Persistentes - Punto 2 TP")]
    public float vidaActual = 100f;
    public float vidaMax = 100f;
    public int puntajeAcumulado = 0;
    public int itemsRecolectados = 0;
    private int enemigosRestantes = 0;

    void Awake()
    {
        if (Instance == null) 
        { 
            Instance = this; 
            DontDestroyOnLoad(gameObject); 
            ResetDatos(); 
        }
        else 
        { 
            Destroy(gameObject); 
            return; 
        }
    }

    void Start() => CambiarEstado(EstadoJuego.Inicio);

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))
        {
            if (estadoActual == EstadoJuego.Jugando) PausarJuego();
            else if (estadoActual == EstadoJuego.Pausa) ReanudarJuego();
        }
    }

    public void CambiarEstado(EstadoJuego nuevo)
    {
        estadoActual = nuevo;
        currentState = (GameState)nuevo;
        Debug.Log($"[GameManager] Estado cambiado a: {estadoActual}");

        if (nuevo == EstadoJuego.Pausa)
            Time.timeScale = 0f;
        else
            Time.timeScale = 1f;

        if (nuevo == EstadoJuego.Jugando)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        OnChangeState?.Invoke(currentState);
    }

    // --- CAMBIO DE NIVELES CON PERSISTENCIA ---
    public void IniciarJuego() 
    { 
        ResetDatos(); 
        SceneManager.LoadScene("Level1"); 
        CambiarEstado(EstadoJuego.Jugando); 
    }

    public void EntrarComisaria() 
    { 
        SceneManager.LoadScene("Level2"); 
        CambiarEstado(EstadoJuego.Jugando); 
    }

    public void CargarNivel2() => EntrarComisaria();

    public void SalirComisaria() 
    { 
        SceneManager.LoadScene("Level1"); 
        CambiarEstado(EstadoJuego.Jugando); 
    }

    public void VolverMenu() 
    { 
        SceneManager.LoadScene("MainMenu"); 
        CambiarEstado(EstadoJuego.Inicio); 
    }

    public void ReanudarJuego() => CambiarEstado(EstadoJuego.Jugando);
    public void PausarJuego() => CambiarEstado(EstadoJuego.Pausa);
    public void Victoria() => CambiarEstado(EstadoJuego.Victoria);
    public void Derrota() => CambiarEstado(EstadoJuego.Derrota);

    public void ReiniciarNivel()
    {
        Time.timeScale = 1f;
        ResetDatos();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        CambiarEstado(EstadoJuego.Jugando);
    }

    public void SalirDelJuego()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }

    // --- DATOS QUE VIAJAN ENTRE NIVELES ---
    public void ResetDatos() 
    { 
        vidaActual = vidaMax; 
        puntajeAcumulado = 0; 
        itemsRecolectados = 0; 
        enemigosRestantes = 0; 
    }

    public void SetVida(float v) { vidaActual = v; }
    public float GetVida() => vidaActual;
    public void AddPuntos(int p) { puntajeAcumulado += p; }
    public void AddItem(int c = 1) { itemsRecolectados += c; }
    public void SetEnemigos(int c) 
    { 
        enemigosRestantes = c; 
        OnEnemyChange?.Invoke(c); 
        OnEnemyCountChanged?.Invoke(c); 
    }
}