using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject PausePanel;
    [SerializeField] private GameObject VictoryPanel;
    [SerializeField] private GameObject GameOverPanel;
    [SerializeField] private GameObject InicioPanel;
    [SerializeField] private GameObject HUDPanel;
    [SerializeField] private TextMeshProUGUI puntajeText;
    [SerializeField] private Slider healthBar;
    [SerializeField] private HealthSystem playerHealth;

    void OnEnable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnChangeState += HandleState;
            GameManager.Instance.OnPuntajeCambiado += UpdatePuntaje;
            HandleState(GameManager.Instance.currentState);
            UpdatePuntaje(GameManager.Instance.puntajeAcumulado);
        }
        if (playerHealth != null) playerHealth.OnChangeHealthPlayer += UpdateHealth;
    }

    void OnDisable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnChangeState -= HandleState;
            GameManager.Instance.OnPuntajeCambiado -= UpdatePuntaje;
        }
        if (playerHealth != null) playerHealth.OnChangeHealthPlayer -= UpdateHealth;
    }

    void HandleState(GameState state)
    {
        if (InicioPanel) InicioPanel.SetActive(state == GameState.Start);
        if (HUDPanel) HUDPanel.SetActive(state == GameState.Playing);
        if (PausePanel) PausePanel.SetActive(state == GameState.Paused);
        if (VictoryPanel) VictoryPanel.SetActive(state == GameState.Victory);
        if (GameOverPanel) GameOverPanel.SetActive(state == GameState.GameOver);

        if (state == GameState.Playing) { Cursor.lockState = CursorLockMode.Locked; Cursor.visible = false; Time.timeScale = 1f; }
        else { Cursor.lockState = CursorLockMode.None; Cursor.visible = true; if (state != GameState.Start) Time.timeScale = 0f; }
    }

    void UpdatePuntaje(int p) { if (puntajeText) puntajeText.text = "Puntos: " + p.ToString(); }
    void UpdateHealth(float p) { if (healthBar) healthBar.value = p; }

    public void TogglePanel(GameObject panel) { panel.SetActive(!panel.activeSelf); }

    public void Btn_Reanudar() { GameManager.Instance.ReanudarJuego(); }
    public void Btn_Reiniciar() { GameManager.Instance.ReiniciarNivel(); }
    public void Btn_MenuPrincipal() { GameManager.Instance.VolverMenu(); }
    public void Btn_Salir() { GameManager.Instance.SalirDelJuego(); }
}
