using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public Transform player;
    public float searchRadius = 10f;
    public float chaseSpeed = 5f;
    public float idleTime = 2f;
    
    private NavMeshAgent navMeshAgent;
    private Vector3 initialPosition;
    private bool isChasing = false;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        initialPosition = transform.position;
    }

    void Update()
    {
        if (!isChasing)
        {
            SearchForPlayer();
        }
        else
        {
            ChasePlayer();
        }
    }

    void SearchForPlayer()
    {
        if (Vector3.Distance(transform.position, player.position) <= searchRadius)
        {
            isChasing = true;
        }
        else
        {
            // Move back to the initial position when not chasing
            navMeshAgent.SetDestination(initialPosition);
        }
    }

    void ChasePlayer()
    {
        navMeshAgent.SetDestination(player.position);

        // Check if the enemy has caught up to the player
        if (Vector3.Distance(transform.position, player.position) <= navMeshAgent.stoppingDistance)
        {
            // TODO: Add logic for what happens when the enemy catches the player (e.g., game over)
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (!isChasing && other.CompareTag("Player"))
        {
            // Player is within the enemy's field of view, start chasing
            isChasing = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Player is out of the enemy's field of view, stop chasing and reset idle timer
            isChasing = false;
        }
    }
}



