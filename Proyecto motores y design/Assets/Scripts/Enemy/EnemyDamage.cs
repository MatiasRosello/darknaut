using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class EnemyDamage : MonoBehaviour
{
    [Header("Configuración de Daño")]
    [Tooltip("Cuánto daño inflige al jugador.")]
    [SerializeField] private int damageAmount = 50;

    [Tooltip("Tag del GameObject que recibirá el daño.")]
    [SerializeField] private string targetTag = "Player";

    [Tooltip("Usar colisiones (false) o triggers (true) para infligir daño.")]
    [SerializeField] private bool useTrigger = true;

    private void Reset()
    {
        // Para conveniencia, al agregar el script te marca el collider como Trigger si corresponde
        var col = GetComponent<Collider>();
        col.isTrigger = useTrigger;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!useTrigger) return;
        DoDamage(other.gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (useTrigger) return;
        DoDamage(collision.gameObject);
    }

    private void DoDamage(GameObject other)
    {
        if (!other.CompareTag(targetTag))
            return;

        var ph = other.GetComponent<PlayerHealth>();
        if (ph == null)
            return;

        if (ph.IsAlive())
        {
            ph.TakeDamage(damageAmount);
            Debug.Log($"[DamageOnContact] {name} inflige {damageAmount} a {other.name}");
        }
    }
}

