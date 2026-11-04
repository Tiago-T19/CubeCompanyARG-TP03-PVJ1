using UnityEngine;
using System;

public class HealthSystem : MonoBehaviour, IDamageable
{
    [Header("Asigna el .asset o ScriptableObject de este enemigo")]
    public EnemyData data; // Arrastra EnemigoComun, Especial o JefeFinal

    [Header("Fallback cuando no utilizemos el .asset o ScriptableObject")]
    [SerializeField] private float maxHealthFallback = 100f;

    private float currentHealth;
    private bool isDead;

    public float CurrentHealth => currentHealth;
    public bool IsDead => isDead;

    // Evento para sumar puntos, lo usa el GameManager
    public static Action<EnemyData> OnEnemigoMuerto;
    // Evento para el HUD
    public static Action<float, float> OnVidaCambiada; // vida actual, vida max

    private float VidaMaxReal {
        get { return data != null ? data.vidaMax : maxHealthFallback; }
    }

    private void Awake()
    {
        currentHealth = VidaMaxReal;
        isDead = false;
    }

    //Método que debe usar mi compañero con RayCast y Attack.cs de melee
    public void TakeDamage(float amount)
    {
        if (isDead) return;

        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, VidaMaxReal);
        
        OnVidaCambiada?.Invoke(currentHealth, VidaMaxReal);
        Debug.Log($"{gameObject.name} recibió {amount} daño. Vida: {currentHealth}");

        if (currentHealth <= 0f)
        {
            Dead();
        }
    }

    public void Heal(float amount)
    {
        if (isDead) return;
        currentHealth = Mathf.Clamp(currentHealth + amount, 0f, VidaMaxReal);
        OnVidaCambiada?.Invoke(currentHealth, VidaMaxReal);
    }

    private void Dead()
    {
        isDead = true;
        Debug.Log($"{gameObject.name} eliminado");

        // Si es enemigo, avisa que tiene que sumar puntos
        if (data != null)
        {
            OnEnemigoMuerto?.Invoke(data);
        }

        // Si es el Player, llama a Derrota
        if (CompareTag("Player"))
        {
            // Si ya tienen GameManager con singleton
            //if (GameManager.Instance != null)
            {
            //    GameManager.Instance.Derrota();
            }
        }
        else
        {
            // Si es enemigo, lo destruye
            //Destroy(gameObject);
        }
        
    }
}

