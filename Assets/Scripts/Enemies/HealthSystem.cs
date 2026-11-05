using UnityEngine;
using System;

public class HealthSystem : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;
    public bool isPlayer = false; // tildalo solo en el Player

    public float CurrentHealth => currentHealth;
    public float vidaMax => maxHealth;
    public bool IsDead => currentHealth <= 0;
    //public bool IsDead => IsDead;

    public event Action<float> OnChangeHealthPlayer;
    public event Action OnDeath;

    void Awake() => currentHealth = maxHealth;

    public void TakeDamage(float dmg)
    {
        currentHealth -= dmg;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        Debug.Log($"{gameObject.name} vida: {currentHealth}");

        if(isPlayer)
        {
            OnChangeHealthPlayer?.Invoke(currentHealth / maxHealth);
            if(GameManager.Instance) GameManager.Instance.SetVida(currentHealth);
            //GameManager.Instance.SetVida(currentHealth);
            if(currentHealth <= 0) GameManager.Instance.Derrota();
        }
        else
        {
            if(currentHealth <= 0) Die();
        }
    }

    public void Heal(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        if(isPlayer) 
        {
            OnChangeHealthPlayer?.Invoke(currentHealth / maxHealth);
            if(GameManager.Instance) GameManager.Instance.SetVida(currentHealth);
            //GameManager.Instance.SetVida(currentHealth);
        }
    }

    void Die()
    {
        OnDeath?.Invoke();
        if(GameManager.Instance) GameManager.Instance.AddPuntos(10);
        //GameManager.Instance.AddPuntos(10);
        Destroy(gameObject);
    }
}