using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemieAI : MonoBehaviour
{
    public NavMeshAgent enemy;
    public Transform player;

    public List<Transform> destinations;
    private Transform currentDest;
    private Vector3 dest;

    private int randNum;
    public int destinationAmount;

    public bool isWalking, isChasing, isWaiting;

    public float walkSpeed, chaseSpeed;
    public float minIdleTime, maxIdleTime, idleTime;
    public float viewRadius;
    public float viewAngle;

    public LayerMask targetPlayer;
    public LayerMask obstacleMask;

    private void Start()
    {
        isWalking = true;
        randNum = Random.Range(0, destinationAmount);
        currentDest = destinations[randNum];
    }

    private void Update()
    {
        Vector3 playerTarget = (player.position - transform.position).normalized;

        if (Vector3.Angle(transform.forward, playerTarget) < viewAngle / 2)
        {
            float distanceToTarget = Vector3.Distance(transform.position, player.position);
            if (distanceToTarget <= viewRadius)
            {
                if (Physics.Raycast(transform.position, playerTarget, distanceToTarget, obstacleMask) == false)
                {
                    ChasePlayer();
                    isWalking = false;
                    isWaiting = false;
                    isChasing = true;
                }
            }
        }
        else
        {
            isWalking = true;
            isChasing = false;
        }

        if (!isWaiting && isWalking && !isChasing)
        {
            dest = currentDest.position;
            enemy.destination = dest;
            enemy.speed = walkSpeed;
            if (enemy.remainingDistance <= enemy.stoppingDistance)
            {
                enemy.speed = 0;
                StartCoroutine(StayIdle());
            }
        }
    }

    void ChasePlayer()
    {
        dest = player.position;
        enemy.destination = dest;
        enemy.speed = chaseSpeed;
    }

    IEnumerator StayIdle()
    {
        isWaiting = true;
        isWalking = false;
        idleTime = Random.Range(minIdleTime, maxIdleTime);
        yield return new WaitForSeconds(idleTime);
        isWaiting = false;
        isWalking = true;
        randNum = Random.Range(0, destinationAmount);
        currentDest = destinations[randNum];
    }
}
