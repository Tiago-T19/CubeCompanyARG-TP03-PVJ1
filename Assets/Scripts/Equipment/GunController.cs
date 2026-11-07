using UnityEngine;

public class GunController : MonoBehaviour

{
    public float damage = 15f;
    public float range = 100f;
    [SerializeField] private LayerMask enemyLayer;
    public int maxAmmo = 30;
    private int currentAmmo;

    private Camera cam;

    private void Start()
    {
        currentAmmo = maxAmmo;
        cam = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Reload();
        }
    }

    private void Shoot()
    {
        if (currentAmmo <= 0)
        {
            Debug.Log("Sin balas");
            return;
        }

        currentAmmo--;

        Debug.Log("Disparo. Balas: " + currentAmmo);

        Ray ray = cam.ScreenPointToRay(
            new Vector3(
                Screen.width / 2,
                Screen.height / 2
            )
        );

        if (Physics.Raycast(ray, out RaycastHit hit, range, enemyLayer))
        {
            Debug.Log("Impactó en " + hit.collider.name);

            HealthSystem health =
            hit.collider.GetComponent<HealthSystem>();

            if (health != null)
            {
                health.TakeDamage(damage);
            }
        }
    }

    private void Reload()
    {
        currentAmmo = maxAmmo;

        Debug.Log("Recargando...");
    }
}