using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour
{
    [Header("Parámetros de Salud")]
    [SerializeField] private int maxHealth = 100;

    [SerializeField] private int currentHealth;
    [SerializeField] private AudioClip dyingSound;
    [SerializeField] private AudioClip hurtSound;

    private Animator animator;
    private AudioSource audioSource;

    // Eventos opcionales para UI
    //public UnityEvent<int, int> OnHealthChanged; // pasa (current, max)
    //public UnityEvent OnPlayerDied;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        currentHealth = maxHealth;
        //OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    public void TakeDamage(int amount)
    {
        audioSource.PlayOneShot(hurtSound);
        currentHealth = Mathf.Max(currentHealth - amount, 0);
        //OnHealthChanged?.Invoke(currentHealth, maxHealth);

        if (currentHealth <= 0)
        {
            audioSource.PlayOneShot(dyingSound);
            animator.SetTrigger("Death");
        }
    }
    public void Die()
    {
        Debug.Log("[PlayerHealth] El jugador ha muerto.");
        //OnPlayerDied?.Invoke();
        GameManager.Instance.LoadGameOver();
    }

    public bool IsAlive()
    {
        return currentHealth > 0;
    }
}
