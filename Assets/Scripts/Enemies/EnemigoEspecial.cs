using UnityEngine;
using UnityEngine.AI;

public class EnemigoEspecial : EnemyAI
{
    [Header("Enemigo Especial")]
    public float duracionRalentizacion = 3f;

    protected override void ExecuteAttack()
    {
        // 1. Hace el daño normal de tu EnemyAttack.cs reciclado
        base.ExecuteAttack();

        // 2. Habilidad especial: lo deja lento 3 segundos
        var movPlayer = playerTransform.GetComponent<PlayerMovement>();
        if (movPlayer != null)
        {
            movPlayer.Speed *= 0.5f;
            Invoke(nameof(RestaurarVelocidad), duracionRalentizacion);
        }
    }

    void RestaurarVelocidad()
    {
        // Busca al player y le devuelve la velocidad
        var playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            var mov = playerObj.GetComponent<PlayerMovement>();
            if (mov != null) mov.Speed = 5f; // tu velocidad normal
        }
    }
}