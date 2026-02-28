// EnemyAI.cs

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public Transform[] patrolPoints;
    public float speed;
    public float chaseSpeed;
    public float attackRange;
    public Transform player;
    private int currentPatrolIndex;
    private bool isChasing = false;

    void Start()
    {
        if (patrolPoints.Length > 0)
        {
            transform.position = patrolPoints[0].position;
        }
    }

    void Update()
    {
        if (isChasing)
        {
            ChasePlayer();
        }
        else
        {
            Patrol();
        }

        CheckForPlayer();
    }

    void Patrol()
    {
        if (patrolPoints.Length == 0) return;

        Transform targetPoint = patrolPoints[currentPatrolIndex];
        transform.position = Vector3.MoveTowards(transform.position, targetPoint.position, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPoint.position) < 0.1f)
        {
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
        }
    }

    void CheckForPlayer()
    {
        if (Vector3.Distance(transform.position, player.position) < attackRange)
        {
            AttackPlayer();
        }
        else if (Vector3.Distance(transform.position, player.position) < 10f) // Example chase distance
        {
            isChasing = true;
        }
    }

    void ChasePlayer()
    {
        transform.position = Vector3.MoveTowards(transform.position, player.position, chaseSpeed * Time.deltaTime);
        if (Vector3.Distance(transform.position, player.position) >= 10f)
        {
            isChasing = false;
        }
    }

    void AttackPlayer()
    {
        // Add attack logic here
        Debug.Log("Attacking player!");
    }
}

// Time loop integration (pseudo-code)
// The enemy AI logic responds to the time loop system to reset or change behavior based on time events.