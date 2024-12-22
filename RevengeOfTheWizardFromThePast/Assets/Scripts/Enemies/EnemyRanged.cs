using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyRanged : MonoBehaviour
{
    [SerializeField] private float detectionRange = 5f;
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float attackRange = 13f;
    [SerializeField] private float attackCooldown = 3f;
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float bulletSpeed = 10f;

    private float currentHealth;
    private Transform playerTransform;
    private NavMeshAgent agent;
    private float attackTime;
    private bool hasLineOfSight = false;

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

        if (hasLineOfSight)
        {
            if (distanceToPlayer <= attackRange)
            {
                agent.ResetPath();
                Attack();
            }
            else if (distanceToPlayer <= detectionRange)
            {
                ChasePlayer();
            }
        }
    }

    private void FixedUpdate()
    {
        Vector2 directionToPlayer = playerTransform.position - transform.position;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToPlayer);

        if (hit.collider != null)
        {
            hasLineOfSight = hit.collider.CompareTag("Player");
            if (hasLineOfSight)
            {
                Debug.DrawRay(transform.position, directionToPlayer, Color.green);
            }
            else
            {
                Debug.DrawRay(transform.position, directionToPlayer, Color.red);
            }
        }
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
            Vector2 directionToPlayer = (playerTransform.position - transform.position).normalized;

            float angle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;
            GameObject bulletObj = Instantiate(projectilePrefab, transform.position, Quaternion.Euler(0, 0, angle));


            Rigidbody2D rb = bulletObj.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                print("shoot");
                rb.linearVelocity = directionToPlayer * bulletSpeed;
            }

            attackTime = Time.time + attackCooldown;
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Death();
        }
    }

    private void Death()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying || playerTransform == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}