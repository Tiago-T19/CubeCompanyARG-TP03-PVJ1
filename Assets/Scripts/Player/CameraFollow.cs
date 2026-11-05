using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private GameObject target_p;
    [SerializeField] private float distance = 5.0f;
    [SerializeField] private Vector3 Offset = new Vector3(0, 1.5f, 0);
    [SerializeField] private float sensitivityX = 4.0f;
    [SerializeField] private float sensitivityY = 2.0f;
    [SerializeField] private float minYAngle = -20f;
    [SerializeField] private float maxYAngle = 60f;

    private float currentX = 0f;
    private float currentY = 20f;

    void Start()
    {
        if (target_p != null) currentX = target_p.transform.eulerAngles.y;
        // NO bloquees el cursor acá, lo hace el GameManager
    }

    void Update()
    {
        if(GameManager.Instance != null && GameManager.Instance.estadoActual != EstadoJuego.Jugando)
            return;

        currentX += Input.GetAxis("Mouse X") * sensitivityX;
        currentY -= Input.GetAxis("Mouse Y") * sensitivityY;
        currentY = Mathf.Clamp(currentY, minYAngle, maxYAngle);
    }

    void LateUpdate()
    {
        if (target_p == null) return;
        Quaternion camRotation = Quaternion.Euler(currentY, currentX, 0);
        Vector3 positionOffset = camRotation * new Vector3(0, 0, -distance);
        transform.position = target_p.transform.position + Offset + positionOffset;
        transform.LookAt(target_p.transform.position + Offset);
        target_p.GetComponent<PlayerMovement>().RotateCharacter(currentX);
    }
}