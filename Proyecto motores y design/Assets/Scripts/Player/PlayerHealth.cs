using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour
{
    [Header("Parámetros de Salud")]
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int damageAmount = 50;

    [SerializeField] private int currentHealth;

    // Eventos opcionales para UI u otros sistemas
    public UnityEvent<int, int> OnHealthChanged; // pasa (current, max)
    public UnityEvent OnPlayerDied;


    private void Start()
    {
        currentHealth = maxHealth;
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    public void TakeHit()
    {
        TakeDamage(damageAmount);
    }

    public void TakeDamage(int amount)
    {
        currentHealth = Mathf.Max(currentHealth - amount, 0);
        OnHealthChanged?.Invoke(currentHealth, maxHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }
    public void Die()
    {
        Debug.Log("[PlayerHealth] El jugador ha muerto.");
        OnPlayerDied?.Invoke();
        GameManager.Instance.LoadGameOver();
    }

    public bool IsAlive()
    {
        return currentHealth > 0;
    }
}
