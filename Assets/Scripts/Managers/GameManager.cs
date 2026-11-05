using UnityEngine;
using UnityEngine.SceneManagement;

public enum EstadoJuego { Inicio, Jugando, Pausa, Victoria, Derrota }

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public EstadoJuego estadoActual = EstadoJuego.Inicio;

    [Header("Datos Persistentes")]
    public float vidaActual = 100f;
    public float vidaMax = 100f;
    public int puntajeAcumulado = 0;
    public int itemsRecolectados = 0;

    [Header("UI Paneles")]
    public GameObject panelInicio;
    public GameObject panelHUD;
    public GameObject panelPausa;
    public GameObject panelVictoria;
    public GameObject panelDerrota;

    void Awake()
    {
        if (Instance == null) { Instance = this; DontDestroyOnLoad(gameObject); ResetDatos(); }
        else { Destroy(gameObject); return; }
    }

    void Start() => CambiarEstado(EstadoJuego.Inicio);

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (estadoActual == EstadoJuego.Jugando)
                CambiarEstado(EstadoJuego.Pausa);
            else if (estadoActual == EstadoJuego.Pausa)
                ReanudarJuego();
        }
    }

    public void CambiarEstado(EstadoJuego nuevo)
    {
        estadoActual = nuevo;
        Debug.Log($"[GameManager] Estado: {estadoActual}");

        if(panelInicio) panelInicio.SetActive(false);
        if(panelHUD) panelHUD.SetActive(false);
        if(panelPausa) panelPausa.SetActive(false);
        if(panelVictoria) panelVictoria.SetActive(false);
        if(panelDerrota) panelDerrota.SetActive(false);

        switch (nuevo)
        {
            case EstadoJuego.Inicio:
                Time.timeScale = 1f; // FIX: en 1 para que ande el click
                if(panelInicio) panelInicio.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                break;
            case EstadoJuego.Jugando:
                Time.timeScale = 1f;
                if(panelHUD) panelHUD.SetActive(true);
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                break;
            case EstadoJuego.Pausa:
                Time.timeScale = 0f;
                if(panelPausa) panelPausa.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                break;
            case EstadoJuego.Victoria:
            case EstadoJuego.Derrota:
                Time.timeScale = 0f;
                if(panelVictoria && nuevo == EstadoJuego.Victoria) panelVictoria.SetActive(true);
                if(panelDerrota && nuevo == EstadoJuego.Derrota) panelDerrota.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                break;
        }
    }

    public void SalirJuego()
    {
        Debug.Log("[GameManager] Salir del juego");
        UnityEditor.EditorApplication.isPlaying = false; // Para el editor
        Application.Quit();
    }

    public void IniciarJuego() { ResetDatos(); CambiarEstado(EstadoJuego.Jugando); }
    public void EntrarComisaria() { CambiarEstado(EstadoJuego.Jugando); SceneManager.LoadScene("Comisaria"); }
    public void SalirComisaria() { CambiarEstado(EstadoJuego.Jugando); SceneManager.LoadScene("Nivel1"); }
    public void ReanudarJuego() => CambiarEstado(EstadoJuego.Jugando);
    public void PausarJuego() => CambiarEstado(EstadoJuego.Pausa);
    public void Victoria() => CambiarEstado(EstadoJuego.Victoria);
    public void Derrota() => CambiarEstado(EstadoJuego.Derrota);
    public void ReiniciarNivel() {
         Time.timeScale = 1f;
         ResetDatos();
         CambiarEstado(EstadoJuego.Jugando);

         var player = FindFirstObjectByType<PlayerMovement>();
         if (player != null) player.transform.position = new Vector3(68,1,422); // FIX: reinicia la posición del jugador al inicio del nivel
         if(panelHUD) panelHUD.SetActive(true);
         }
    public void ResetDatos() { vidaActual = vidaMax; puntajeAcumulado = 0; itemsRecolectados = 0; }
    public void SetVida(float v) => vidaActual = v;
    public float GetVida() => vidaActual;
    public void AddPuntos(int p) => puntajeAcumulado += p;
    public void AddItem(int c = 1) => itemsRecolectados += c;
}