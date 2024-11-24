using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMelee : MonoBehaviour
{
    [SerializeField] private float detectionRange = 5f;
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float attackRange = 3f;
    [SerializeField] private float attackCooldown = 3f;

    private Transform playerTransform;
    private NavMeshAgent agent;
    private float attackTime;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        agent.speed = moveSpeed;
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    private void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

        if (distanceToPlayer <= attackRange)
        {
            agent.ResetPath();

            Attack();
        }
        else
        {
            ChasePlayer();
        }
        // float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // if (distanceToPlayer <= detectionRange)
        // {
        // }
    }

    private void ChasePlayer()
    {
        if (Time.time >= attackTime)
        {
            agent.SetDestination(playerTransform.position);
        }
    }

    private void Attack()
    {
        agent.ResetPath();

        if (Time.time >= attackTime)
        {
            print("attack");
            attackTime = Time.time + attackCooldown;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}