using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public enum GameState { Start, Playing, Paused, Victory, GameOver }
public enum EstadoJuego { Inicio, Jugando, Pausa, Victoria, Derrota }

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Estado")]
    public EstadoJuego estadoActual = EstadoJuego.Inicio;
    public GameState currentState = GameState.Start;

    // Eventos que espera el UIManager
    public event Action<GameState> OnChangeState;
    public event Action<int> OnPuntajeCambiado;

    [Header("Datos Persistentes")]
    public float vidaActual = 100f;
    public float vidaMax = 100f;
    public int puntajeAcumulado = 0;
    public int itemsRecolectados = 0;
    public bool jefeFinalMuerto = false;

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

    // --- CAMBIO DE NIVELES CON PANTALLA DE CARGA ---
    public void IniciarJuego()
    {
        ResetDatos();
        PlayerPrefs.SetString("NextScene", "Level1");
        SceneManager.LoadScene("PantallaDeCarga");
        CambiarEstado(EstadoJuego.Jugando);
    }

    public void EntrarComisaria()
    {
        PlayerPrefs.SetString("NextScene", "Level2");
        SceneManager.LoadScene("PantallaDeCarga");
        CambiarEstado(EstadoJuego.Jugando);
    }

    public void CargarNivel2() => EntrarComisaria();

    public void SalirComisaria()
    {
        PlayerPrefs.SetString("NextScene", "Level1");
        SceneManager.LoadScene("PantallaDeCarga");
        CambiarEstado(EstadoJuego.Jugando);
    }

    public void VolverMenu()
    {
        PlayerPrefs.SetString("NextScene", "MainMenu");
        SceneManager.LoadScene("PantallaDeCarga");
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
        string escenaActual = SceneManager.GetActiveScene().name;
        PlayerPrefs.SetString("NextScene", escenaActual);
        SceneManager.LoadScene("PantallaDeCarga");
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
        jefeFinalMuerto = false;
        OnPuntajeCambiado?.Invoke(puntajeAcumulado);
    }

    public void SetVida(float v) { vidaActual = v; }
    public float GetVida() => vidaActual;

    public void AddPuntos(int p)
    {
        puntajeAcumulado += p;
        OnPuntajeCambiado?.Invoke(puntajeAcumulado);
    }

    public void MatarJefeFinal()
    {
        jefeFinalMuerto = true;
        Debug.Log("Jefe Final derrotado, ¡ya puedes escapar!");
    }

    public void AddItem(int c = 1) { itemsRecolectados += c; }
}