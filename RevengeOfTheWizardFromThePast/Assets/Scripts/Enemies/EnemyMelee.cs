using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMelee : MonoBehaviour
{
    [SerializeField] private float detectionRange = 5f;
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float attackRange = 3f;
    [SerializeField] private float attackDuration = 0.5f;
    [SerializeField] private float attackCooldown = 3f;
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private AttackArea attackArea;


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

        attackArea.gameObject.SetActive(false);
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
            attackArea.transform.rotation = Quaternion.Euler(0, 0, angle);

            attackArea.gameObject.SetActive(true);
            StartCoroutine(DeactivateAttackArea());
            attackTime = Time.time + attackCooldown;
        }
    }

    private IEnumerator DeactivateAttackArea()
    {
        yield return new WaitForSeconds(attackDuration);
        attackArea.gameObject.SetActive(false);
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