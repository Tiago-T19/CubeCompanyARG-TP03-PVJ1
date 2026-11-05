using UnityEngine;
using UnityEngine.AI;

public class JefeFinal : EnemyAI {
    public bool fase2 = false;
    public HealthSystem vida;

    protected override void Awake()
    {
        base.Awake();
        vida = GetComponent<HealthSystem>();
    }

    protected override void Update()
    {
        base.Update();

        //Si le queda menos del 50% de vida, entra en fase 2
        if(!fase2 && vida != null && vida.CurrentHealth > 0 && vida.CurrentHealth <= data.vidaMax * 0.5f)
        {
            fase2 = true;
            data.velocidad *= 1.5f;
            data.danio *= 2;
            GetComponent<NavMeshAgent>().speed = data.velocidad;
            Debug.Log("¡Jefe Final en fase 2!, derrotalo.");
        }
    }
}