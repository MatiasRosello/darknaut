using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyHearing : MonoBehaviour
{
    [SerializeField] private float hearingRadius = 5f;

    [SerializeField] private float detectionThreshold = 0.3f;

    [SerializeField] private float playerMakingSoundThreshold = 1f;

    [SerializeField] private Transform playerTransform;

    private EnemyState enemyState;

    private void Awake()
    {
        enemyState = GetComponent<EnemyState>();
        if (enemyState == null)
            Debug.LogError($"[Hearing] No se encontró EnemyState en {name}");
        else
            Debug.Log($"[Hearing] {name} encontró su EnemyState ok.");
    }
    private void Start()
    {
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.OnNoiseMade += OnNoiseMade;
            Debug.Log($"[Hearing] {name} se suscribió a OnNoiseMade");
        }
        else
        {
            Debug.LogWarning("[Hearing] SoundManager.Instance es null. ¿Olvidaste añadir el SoundManager a la escena?");
        }

        if (playerTransform == null)
        {
            Debug.LogWarning($"[Hearing] {name}: no hay playerTransform asignado en el Inspector.");
        }
    }

    private void OnDisable()
    {
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.OnNoiseMade -= OnNoiseMade;
        }   
    }

    private void OnNoiseMade(Vector3 noisePosition, float intensity)
    {
        //Comprobar siempre proximidad directa jugador ↔ enemigo:
        if (playerTransform != null)
        {
            float distToPlayer = Vector3.Distance(transform.position, playerTransform.position);
            if (distToPlayer <= detectionThreshold)
            {
                Debug.Log($"[Hearing] {name}: jugador a {distToPlayer:F2} ≤ {detectionThreshold}. ¡Detectado cuerpo a cuerpo!");
                enemyState.ChasePlayer();
                return;
            }
        }

        //Si no estamos cuerpo a cuerpo, revisamos la distancia al ruido:
        float distanceToNoise = Vector3.Distance(transform.position, noisePosition);
        Debug.Log($"[Hearing] {name} recibió OnNoiseMade. Distancia al ruido: {distanceToNoise:F2}. Radio × intensidad = {hearingRadius * intensity:F2}");

        if (distanceToNoise <= hearingRadius * intensity)
        {
            //Si el jugador (en este momento) está dentro de chaseRadius, despachamos persecución directamente
            if (playerTransform != null)
            {
                float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

                float soundDistToPlayer = Vector3.Distance(noisePosition, playerTransform.position);

                if (distanceToPlayer <= enemyState.chaseRadius && soundDistToPlayer < playerMakingSoundThreshold)
                {
                    enemyState.ChasePlayer();
                    return;
                }
            }

            Debug.Log($"[Hearing] {name}: ¡Oí ruido, voy a InvestigateNoise!");
            enemyState.InvestigateNoise(noisePosition);
        }
    }

    public void SetHearingRadius(float newRadius)
    {
        hearingRadius = newRadius;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, hearingRadius);
        Gizmos.DrawWireSphere(playerTransform.position, playerMakingSoundThreshold);

        if (playerTransform != null)
        {
            // Opcional: dibujar chaseRadius en rojo para ver hasta dónde persigue
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, enemyState != null ? enemyState.chaseRadius : 0f);
        }
    }
}
