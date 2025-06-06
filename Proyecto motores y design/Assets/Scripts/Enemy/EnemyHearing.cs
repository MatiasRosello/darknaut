using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyHearing : MonoBehaviour
{
    [SerializeField] private float hearingRadius = 5f;

    [SerializeField] private float detectionThreshold = 0.3f;

    [SerializeField] private Transform playerTransform;

    private EnemyController enemyController;

    private void Awake()
    {
        enemyController = GetComponent<EnemyController>();
        if (enemyController == null)
            Debug.LogError($"[Hearing] No se encontró EnemyController en {name}");
        else
            Debug.Log($"[Hearing] {name} encontró su EnemyController ok.");
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
                enemyController.ChasePlayer();
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

                if (distanceToPlayer <= enemyController.chaseRadius)
                {
                    enemyController.ChasePlayer();
                    return;
                }
            }

            Debug.Log($"[Hearing] {name}: ¡Oí ruido, voy a InvestigateNoise!");
            enemyController.InvestigateNoise(noisePosition);
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

        if (playerTransform != null)
        {
            // Opcional: dibujar chaseRadius en rojo para ver hasta dónde persigue
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, enemyController != null ? enemyController.chaseRadius : 0f);
        }
    }
}
