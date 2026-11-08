using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [Header("Asigna el .asset o ScriptableObject de este enemigo")]
    public EnemyData data;

    [Header("Fallback cuando no utilizemos el .asset o ScriptableObject")]
    [SerializeField] private float speedFallback = 3.5f;
    [SerializeField] private float detectionRadiusFallback = 8f;
    [SerializeField] private float attackRadiusFallback = 2f;
    [SerializeField] private float attackCooldownFallback = 1.5f;
    [SerializeField] private float damageFallback = 10f;

    public enum AIState { Patrol, Chase, Attack }

    [Header("Estados")]
    [SerializeField] protected AIState currentState = AIState.Patrol;

    [Header("Patrulla Aleatoria (Random NavMesh)")]
    [SerializeField] private float patrolRadius = 15f; // Radio máximo desde el origen para buscar puntos al azar
    [SerializeField] private float waitTimeAtPoint = 2f;
    private Vector3 startPosition; // Posición inicial para calcular el radio de patrulla
    protected bool isWaitingAtPoint;

    protected NavMeshAgent agent;
    protected Transform playerTransform;
    protected HealthSystem playerHealth;
    protected bool isAttacking = false;

    // Lee del .asset si existe, si no del fallback
    protected float SpeedReal => data != null ? data.velocidad : speedFallback;
    protected float DetectionRadiusReal => data != null ? data.detectionRadius : detectionRadiusFallback;
    protected float AttackRadiusReal => data != null ? data.attackRadius : attackRadiusFallback;
    protected float AttackCooldownReal => data != null ? data.attackCooldown : attackCooldownFallback;
    protected float DamageReal => data != null ? data.danio : damageFallback;

    protected virtual void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = SpeedReal;
    }

    protected virtual void Start()
    {
        startPosition = transform.position;

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            playerTransform = playerObj.transform;
            playerHealth = playerObj.GetComponent<HealthSystem>();

            if (playerHealth == null)
            {
                Debug.LogWarning("El objeto con Tag 'Player' no tiene el componente HealthSystem adjunto.");
            }
        }
        else
        {
            Debug.LogError("No hay GameObject con Tag Player para EnemyAI");
        }

        GoToNextRandomPoint();
    }

    protected virtual void Update()
    {
        if (playerTransform == null || (playerHealth != null && playerHealth.IsDead))
        {
            agent.isStopped = true;
            return;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

        switch (currentState)
        {
            case AIState.Patrol:
                UpdatePatrolState(distanceToPlayer);
                break;
            case AIState.Chase:
                UpdateChaseState(distanceToPlayer);
                break;
            case AIState.Attack:
                UpdateAttackState(distanceToPlayer);
                break;
        }
    }

    protected virtual void UpdatePatrolState(float distanceToPlayer)
    {
        agent.isStopped = false;

        if (distanceToPlayer <= DetectionRadiusReal)
        {
            currentState = AIState.Chase;
            return;
        }

        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            if (!isWaitingAtPoint)
            {
                StartCoroutine(WaitAndMoveToRandomPoint());
            }
        }
    }

    protected virtual void UpdateChaseState(float distanceToPlayer)
    {
        agent.isStopped = false;
        agent.SetDestination(playerTransform.position);

        if (distanceToPlayer <= AttackRadiusReal)
        {
            currentState = AIState.Attack;
            agent.isStopped = true;
            return;
        }

        // Si el jugador se aleja demasiado, vuelve a patrullar de forma aleatoria
        if (distanceToPlayer > DetectionRadiusReal * 1.2f)
        {
            currentState = AIState.Patrol;
            GoToNextRandomPoint();
        }
    }

    protected virtual void UpdateAttackState(float distanceToPlayer)
    {
        Vector3 direction = (playerTransform.position - transform.position);
        direction.y = 0;
        if (direction != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(direction);
        }

        if (distanceToPlayer > AttackRadiusReal * 1.25f)
        {
            currentState = AIState.Chase;
            agent.isStopped = false;
            isAttacking = false;
            StopAllCoroutines();
            return;
        }

        if (!isAttacking)
        {
            StartCoroutine(AttackCooldownRoutine());
        }
    }

    protected virtual void ExecuteAttack()
    {
        if (playerHealth == null) return;
        Debug.Log($"¡Enemigo {data?.nombre ?? "Común"} ataca e inflige {DamageReal} de daño!");
        playerHealth.TakeDamage(DamageReal);
    }

    protected IEnumerator AttackCooldownRoutine()
    {
        isAttacking = true;
        agent.isStopped = true;

        ExecuteAttack();

        yield return new WaitForSeconds(AttackCooldownReal);

        isAttacking = false;

        if (playerTransform != null && Vector3.Distance(transform.position, playerTransform.position) <= AttackRadiusReal)
        {
            agent.isStopped = true;
        }
        else
        {
            agent.isStopped = false;
        }
    }

    protected IEnumerator WaitAndMoveToRandomPoint()
    {
        isWaitingAtPoint = true;
        yield return new WaitForSeconds(waitTimeAtPoint);

        GoToNextRandomPoint();

        isWaitingAtPoint = false;
    }

    // Método que calcula una posición aleatoria válida dentro del NavMesh
    protected void GoToNextRandomPoint()
    {
        Vector3 randomDirection = Random.insideUnitSphere * patrolRadius;
        randomDirection += startPosition; 

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, 5f, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
    }

    protected IEnumerator WaitAndMoveToNextPoint()
    {
        yield return StartCoroutine(WaitAndMoveToRandomPoint());
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, DetectionRadiusReal);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, AttackRadiusReal);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(Application.isPlaying ? startPosition : transform.position, patrolRadius);
    }
}