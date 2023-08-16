using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class enemyAI : MonoBehaviour
{
    public NavMeshAgent ai;
    public List<Transform> destinations;
    public Animator aiAnim;
    public float walkSpeed, chaseSpeed, minIdleTime, maxIdleTime, idleTime, sightDistance, catchDistance, chaseTime, minChaseTime, maxChaseTime, jumpscareTime;
    public float viewAngle = 90f;
    public Transform player;
    public bool walking, chasing;
    Transform currentDest;
    Vector3 dest;
    int randNum;
    public int destinationAmount;
    public Vector3 rayCastOffset;
    public string deathScene;
    private bool isWaiting = false;

    void Start()
    {
        walking = true;
        randNum = Random.Range(0, destinationAmount);
        currentDest = destinations[randNum];
    }

    void Update()
    {
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Check if the player is within the view angle and sight distance
        if (angleToPlayer <= viewAngle * 0.5f && distanceToPlayer <= sightDistance)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position + rayCastOffset, directionToPlayer, out hit, sightDistance))
            {
                if (hit.collider.gameObject.tag == "Player")
                {
                    walking = false;
                    chasing = true;
                    // Stop walking animation or any other animation you want here
                }
            }
        }

        if (chasing)
        {
            dest = player.position;
            ai.destination = dest;
            ai.speed = chaseSpeed;
            if (distanceToPlayer <= catchDistance)
            {
                // Do whatever action you want when the player is caught
                // For example, load a game over scene
                Debug.Log("Te atrapé");
            }
        }
        else if (!isWaiting && walking)
        {
            dest = currentDest.position;
            ai.destination = dest;
            ai.speed = walkSpeed;
            // Start walking animation or any other animation you want here
            if (ai.remainingDistance <= ai.stoppingDistance)
            {
                ai.speed = 0;
                // Start idle routine if not already waiting
                if (!isWaiting)
                {
                    StartCoroutine(stayIdle());
                }
            }
        }
    }

    IEnumerator stayIdle()
    {
        isWaiting = true;
        idleTime = Random.Range(minIdleTime, maxIdleTime);
        yield return new WaitForSeconds(idleTime);
        isWaiting = false;
        walking = true;
        randNum = Random.Range(0, destinationAmount);
        currentDest = destinations[randNum];
    }

    IEnumerator chaseRoutine()
    {
        chaseTime = Random.Range(minChaseTime, maxChaseTime);
        yield return new WaitForSeconds(chaseTime);
        walking = true;
        chasing = false;
        // Resume walking animation or any other animation you want here
        randNum = Random.Range(0, destinationAmount);
        currentDest = destinations[randNum];
    }
}
