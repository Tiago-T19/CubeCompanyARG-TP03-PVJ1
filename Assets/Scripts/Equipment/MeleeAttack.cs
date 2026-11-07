using UnityEngine;

public class MeleeAttack : MonoBehaviour

{
    public float damage = 10f;
    public float range = 2f;
    [SerializeField] private LayerMask enemyLayer;
    private Camera cam;

    private void Start()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Attack();
        }
    }

    private void Attack()
    {
        Debug.Log("Cuchillazo");

        Ray ray = cam.ScreenPointToRay(
            new Vector3(
                Screen.width / 2,
                Screen.height / 2
            )
        );

        if (Physics.Raycast(ray, out RaycastHit hit, range, enemyLayer))
        {
            HealthSystem health =
            hit.collider.GetComponent<HealthSystem>();

            if (health != null)
            {
                health.TakeDamage(damage);
            }
        }
    }
}