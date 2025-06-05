using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyHearing : MonoBehaviour
{
    float distance;
    [SerializeField] private float hearingRadius = 5f;

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
        float distanceToNoise = Vector3.Distance(transform.position, noisePosition);
        Debug.Log($"[Hearing] {name} recibió OnNoiseMade. Distancia al ruido: {distanceToNoise:F2}. Radio × intensidad = {hearingRadius * intensity:F2}");

        if (distanceToNoise <= hearingRadius * intensity)
        {
            Debug.Log($"[Hearing] {name}: ¡Oí ruido, voy a InvestigateNoise!");
            enemyController.InvestigateNoise(noisePosition);
        }
    }

    public void SetHearingRadius(float newRadius)
    {
        hearingRadius = newRadius;
    }
}
