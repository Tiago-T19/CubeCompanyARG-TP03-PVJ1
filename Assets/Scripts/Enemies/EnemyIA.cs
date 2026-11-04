using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [Header("Asigna el .asset o ScriptableObject de este enemigo")]
    public EnemyData data; // Arrastra EnemigoComun.asset

    [Header("Fallback cuando no utilizemos el .asset o ScriptableObject")]
    [SerializeField] private float speedFallback = 3.5f;
    [SerializeField] private float detectionRadiusFallback = 8f;
    [SerializeField] private float attackRadiusFallback = 2f;
    [SerializeField] private float attackCooldownFallback = 1.5f;
    [SerializeField] private float damageFallback = 10f;

    public enum AIState { Patrol, Chase, Attack }

    [Header("Estados")]
    [SerializeField] protected AIState currentState = AIState.Patrol;

    [Header("Patrulla")]
    [SerializeField] protected Transform[] patrolPoints;
    [SerializeField] protected float waitTimeAtPoint = 2f;
    protected int currentPatrolIndex;
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
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            playerTransform = playerObj.transform;
            playerHealth = playerObj.GetComponent<HealthSystem>();
        }
        else
        {
            Debug.LogError("No hay GameObject con Tag Player para EnemyAI");
        }

        if (patrolPoints.Length > 0)
        {
            GoToNextPatrolPoint();
        }
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
                StartCoroutine(WaitAndMoveToNextPoint());
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
            return;
        }

        if (distanceToPlayer > DetectionRadiusReal * 1.2f)
        {
            currentState = AIState.Patrol;
            GoToNextPatrolPoint();
        }
    }

    protected virtual void UpdateAttackState(float distanceToPlayer)
    {
        transform.LookAt(new Vector3(playerTransform.position.x, transform.position.y, playerTransform.position.z));

        if (distanceToPlayer > AttackRadiusReal)
        {
            currentState = AIState.Chase;
            agent.isStopped = false;
            return;
        }

        if (!isAttacking)
        {
            StartCoroutine(AttackCooldown());
        }
    }

    protected virtual void ExecuteAttack()
    {
        if (playerHealth == null) return;
        Debug.Log($"Enemigo {data?.nombre} ataca por {DamageReal}");
        playerHealth.TakeDamage(DamageReal);
    }

    protected IEnumerator AttackCooldown()
    {
        isAttacking = true;
        agent.isStopped = true;
        ExecuteAttack();
        yield return new WaitForSeconds(AttackCooldownReal);
        isAttacking = false;
        agent.isStopped = false;
    }

    protected IEnumerator WaitAndMoveToNextPoint()
    {
        isWaitingAtPoint = true;
        yield return new WaitForSeconds(waitTimeAtPoint);
        if (patrolPoints.Length > 0)
        {
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
            GoToNextPatrolPoint();
        }
        isWaitingAtPoint = false;
    }

    protected void GoToNextPatrolPoint()
    {
        if (patrolPoints.Length == 0) return;
        agent.SetDestination(patrolPoints[currentPatrolIndex].position);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, DetectionRadiusReal);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, AttackRadiusReal);
    }
}
