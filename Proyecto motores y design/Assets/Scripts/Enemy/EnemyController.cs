using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private float patrolSpeed = 3f;

    [SerializeField] private float investigateDuration = 3f;

    private NavMeshAgent agent;
    private int currentWaypointIndex = 0;
    private bool isInvestigating = false;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        if (agent == null)
        {
            Debug.LogError($"[EnemyController] Se requiere un NavMeshAgent en {name}.");
        }
    }

    private void Start()
    {
        agent.speed = patrolSpeed;
        if (waypoints != null && waypoints.Length > 0)
        {
            agent.SetDestination(waypoints[currentWaypointIndex].position);
        }
    }

    private void Update()
    {
        // Si estamos investigando, no seguimos patrullando de manera normal
        if (isInvestigating) return;

        // Si el agente llegó (o prácticamente llegó) al waypoint actual, avanzamos al siguiente
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            GoNextWaypoint();
        }
    }

    private void GoNextWaypoint()  // Avanza al siguiente waypoint en orden cíclico.
    {
        if (waypoints == null || waypoints.Length == 0) return;

        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
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
