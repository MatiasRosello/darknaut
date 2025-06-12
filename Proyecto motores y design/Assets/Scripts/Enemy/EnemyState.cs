using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyState : MonoBehaviour
{
    [Header("Alerta")]
    [Tooltip("Clip que suena cuando el enemigo detecta al jugador y antes de perseguir.")]
    [SerializeField] private AudioClip alertClip;
    [Tooltip("Retraso (seg) entre detectar al jugador y comenzar la persecución.")]
    [SerializeField] private float alertDelay = 1f;
    private AudioSource alertAudioSource;

    [Header("Puntos de patrulla")]
    [SerializeField] private Transform[] waypoints;
    [Header("Velocidades")]
    [SerializeField] private float patrolSpeed = 3f;
    [SerializeField] private float chaseMult = 1.2f;
    [Header("Investigacion")]
    [SerializeField] private float investigateDuration = 3f;

    [Header("Distancias de persecucion")]
    [SerializeField] public float chaseRadius = 3f;
    [SerializeField] private float loseRadius = 6f;

    [Header("Referencias")]
    public Transform playerTransform;
    private NavMeshAgent agent;

    private int currentWaypointIndex = 0;
    private bool isInvestigating = false;
    private bool isChasing = false;
    private bool isAlerting = false;

    public bool IsChasing => isChasing; //getter publico


    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        alertAudioSource = GetComponent<AudioSource>();
        if (agent == null)
        {
            Debug.LogError($"[EnemyController] Se requiere un NavMeshAgent en {name}."); 
        }

    }

    private void Start()
    {
        if (playerTransform == null)
        {
            Debug.LogWarning($"[EnemyController] {name}: No se asignó playerTransform en el Inspector.");
        }

        agent.speed = patrolSpeed;
        if (waypoints != null && waypoints.Length > 0)
        {
            agent.SetDestination(waypoints[currentWaypointIndex].position);
        }
    }

    private void Update()
    {
        if (isAlerting)
            return;

        if (isChasing)
        {
            float distToPlayer = Vector3.Distance(transform.position, playerTransform.position);
            if (distToPlayer > loseRadius)
            {
                StopChasing();
                return;
            }

            agent.SetDestination(playerTransform.position);
            return;
        }

        // Si estamos investigando, no seguimos patrullando de manera normal
        if (isInvestigating) return;

        // Si el agente llegó (o prácticamente llegó) al waypoint actual, avanzamos al siguiente
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            GoNextWaypoint();
        }
    }

    public void ChasePlayer()
    {
        if (playerTransform == null || isChasing || isAlerting)
            return;

        // Cancelamos investigación
        StopAllCoroutines();

        // Iniciamos alerta
        StartCoroutine(AlertAndChase());
        Debug.Log($"[EnemyController] {name} entra en PERSECUCIÓN hacia {playerTransform.name}");
    }

    private IEnumerator AlertAndChase()
    {
        isAlerting = true;

        // Sonido de alerta
        if (alertAudioSource != null && alertClip != null )
        {
            alertAudioSource.PlayOneShot(alertClip);
        }

        //detenerse justo antes de perseguir
        agent.isStopped = true;
        yield return new WaitForSeconds(alertDelay); //Espera X segundos y vuelve a perseguir

        // Iniciamos persecución
        agent.isStopped = false;
        isChasing = true;
        agent.speed = patrolSpeed * 1.2f;
        Debug.Log($"[EnemyController] {name} arranca persecución tras alerta");

        isAlerting = false;

    }

    private void StopChasing()
    {
        isChasing = false;
        isInvestigating = false;

        agent.speed = patrolSpeed;

        // Encontrar el waypoint más cercano para reanudar la patrulla:
        if (waypoints != null && waypoints.Length > 0)
        {
            float minDist = float.MaxValue;
            int closestIndex = 0;
            for (int i = 0; i < waypoints.Length; i++)
            {
                float dist = Vector3.Distance(transform.position, waypoints[i].position);
                if (dist < minDist)
                {
                    minDist = dist;
                    closestIndex = i;
                }
            }
            currentWaypointIndex = closestIndex;
            agent.SetDestination(waypoints[currentWaypointIndex].position);
            Debug.Log($"[EnemyController] {name} pierde al jugador y vuelve a patrullar hacia {waypoints[currentWaypointIndex].name}");
        }
        else
        {
            Debug.LogWarning($"[EnemyController] {name} no tiene waypoints asignados para reanudar patrulla.");
        }
    }

    private void GoNextWaypoint()  // Avanza al siguiente waypoint en orden cíclico.
    {
        if (waypoints == null || waypoints.Length == 0) return;

        int nextIndex = (currentWaypointIndex +1) % waypoints.Length;

        currentWaypointIndex = nextIndex;
        agent.SetDestination(waypoints[currentWaypointIndex].position);
    }

    public void InvestigateNoise(Vector3 noisePosition)
    {
        // Si ya está investigando algo, ignoramos nuevos ruidos hasta terminar
        if (isInvestigating) return;

        StopAllCoroutines();
        StartCoroutine(InvestigateCoroutine(noisePosition));
    }

    private IEnumerator InvestigateCoroutine(Vector3 noisePosition)  // Rutina que mueve al enemigo a noisePosition, espera y retorna a patrullar.
    {
        isInvestigating = true;

        // Ajustamos la Y del destino a la del enemigo (evitamos que intente ir a una altura distinta)
        Vector3 adjustedNoisePos = new Vector3(noisePosition.x, transform.position.y, noisePosition.z);

        agent.SetDestination(noisePosition);

        while (!agent.pathPending && agent.remainingDistance > agent.stoppingDistance)
        {
            yield return null;
        }

        yield return new WaitForSeconds(investigateDuration);

        isInvestigating = false;
        agent.speed = patrolSpeed;
        GoNextWaypoint();
    }
}
