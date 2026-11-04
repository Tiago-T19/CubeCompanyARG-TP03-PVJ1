using UnityEngine;

public class Elevador : MonoBehaviour
{
    [Header("Puntos de Destino")]
    [Tooltip("Posición del piso inferior")]
    [SerializeField] private Transform puntoInferior;
    [Tooltip("Posición del piso superior")]
    [SerializeField] private Transform puntoSuperior;

    [Header("Configuración")]
    [SerializeField] private float velocidad = 3f;

    private Transform objetivoActual;

    void Start()
    {
        objetivoActual = puntoSuperior;
    }

    void Update()
    {
        MoverAscensor();
    }

    void MoverAscensor()
    {
        if (puntoInferior == null || puntoSuperior == null) return;

        transform.position = Vector3.MoveTowards(transform.position, objetivoActual.position, velocidad * Time.deltaTime);

        if (Vector3.Distance(transform.position, objetivoActual.position) < 0.01f)
        {
            if (objetivoActual == puntoSuperior)
            {
                objetivoActual = puntoInferior;
            }
            else
            {
                objetivoActual = puntoSuperior;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.transform.SetParent(null);
        }
    }
}
