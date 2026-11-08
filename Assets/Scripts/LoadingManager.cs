using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LoadingManager : MonoBehaviour
{
    [Header("UI Opcional")]
    [SerializeField] private TextMeshProUGUI textoProgreso;

    void Start()
    {
        string escenaDestino = PlayerPrefs.GetString("NextScene", "Level1");

        StartCoroutine(CargarEscenaAsincrona(escenaDestino));
    }

    IEnumerator CargarEscenaAsincrona(string nombreEscena)
    {
        AsyncOperation operacion = SceneManager.LoadSceneAsync(nombreEscena);
        operacion.allowSceneActivation = false;

        while (!operacion.isDone)
        {
            float progreso = Mathf.Clamp01(operacion.progress / 0.9f);

            if (textoProgreso != null)
            {
                textoProgreso.text = "CARGANDO... " + (progreso * 100f).ToString("F0") + "%";
            }

            if (operacion.progress >= 0.9f)
            {
                yield return new WaitForSeconds(0.5f);
                operacion.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}