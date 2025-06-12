using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(NavMeshAgent))]   
public class EnemyFootsteps : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private NavMeshAgent agent;
    private EnemyState enemyState;

    [Header("Intervalo básico (segundos)")]
    [Tooltip("Tiempo base entre pisadas cuando patrulla/investiga (se divide por velocidad).")]
    [SerializeField] private float baseInterval = 0.5f;
    [SerializeField] private float chaseInterval = 0.6f;

    [Header("Datos de superficie")]
    [SerializeField] private SurfaceData[] surfaces;
    [Tooltip("Clip por defecto si no coincide tag.")]
    [SerializeField] private AudioClip defaultClip;
    [Tooltip("Volumen base de las pisadas (0–1).")]
    [SerializeField][Range(0f, 1f)] private float footstepVolume = 1f;
    [SerializeField][Range(1f, 3f)] private float chaseStepVolume = 1.5f;

    private float nextStepTime = 0f;

    private void Awake()
    {
        if (agent == null)
        {
            agent = GetComponent<NavMeshAgent>();
        }
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
        enemyState = GetComponent<EnemyState>();
    }

    private void Update()
    {
        // Solo emitir pisadas si se está moviendo
        if (agent.velocity.magnitude < 0.1f) return;

        bool isChasing = (enemyState != null && enemyState.IsChasing);

        // --- calcular intervalo ---
        // baseInterval adaptado a la velocidad del enemigo (caminando es normal, si persigue es mas rapido)
        float speedFactor = agent.velocity.magnitude / agent.speed;
        float rawInterval = baseInterval / Mathf.Clamp(speedFactor, 0.5f, 2f);
        float interval = rawInterval * (isChasing ? chaseInterval : 1f);

        if (Time.time < nextStepTime) return;

        RaycastHit hit;
        AudioClip clip = defaultClip;

        if (Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.down, out hit, 1f))
        {
            foreach (var s in surfaces)
            {
                if (hit.collider.CompareTag(s.surfaceTag))
                {
                    clip = s.walkClip;
                    break;
                }
            }
        }

        float volume = Mathf.Clamp01(footstepVolume * (isChasing ? chaseStepVolume : 1f));
        audioSource.PlayOneShot(clip, volume);

        nextStepTime = Time.time + interval;
    }


}
