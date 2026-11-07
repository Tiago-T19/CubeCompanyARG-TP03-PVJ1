using UnityEngine;
using System;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] private EnemyData datosEnemigo; // asset enemigo.
    [SerializeField] private bool isPlayer = false;

    private float maxHealth;
    private float currentHealth;

    public float CurrentHealth => currentHealth;
    public float vidaMax => maxHealth;
    public bool IsDead => currentHealth <= 0;

    public event Action<float> OnChangeHealthPlayer;
    public event Action OnDeath;

    void Awake()
    {
        maxHealth = datosEnemigo != null ? datosEnemigo.vidaMax : 100f;

        if (isPlayer && GameManager.Instance != null)
            currentHealth = GameManager.Instance.GetVida();
        else
            currentHealth = maxHealth;
    }

    public void TakeDamage(float dmg)
    {
        currentHealth -= dmg;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        Debug.Log($"{gameObject.name} vida: {currentHealth}");

        if (isPlayer)
        {
            OnChangeHealthPlayer?.Invoke(currentHealth / maxHealth);
            if (GameManager.Instance) GameManager.Instance.SetVida(currentHealth);
            if (currentHealth <= 0) GameManager.Instance.Derrota();
        }
        else
        {
            if (currentHealth <= 0) Die();
        }
    }

    public void Heal(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        if (isPlayer)
        {
            OnChangeHealthPlayer?.Invoke(currentHealth / maxHealth);
            if (GameManager.Instance) GameManager.Instance.SetVida(currentHealth);
        }
    }

    void Die()
    {
        OnDeath?.Invoke();
        int puntos = datosEnemigo != null ? datosEnemigo.puntosAlMorir : 10;
        if (GameManager.Instance) GameManager.Instance.AddPuntos(puntos);
        Destroy(gameObject);
    }
}