using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeAI : MonoBehaviour
{
    [Header("General Settings")]
    [SerializeField] private float moveSpeed = 2f;

    [Header("Patrol Settings")]
    [SerializeField] private Transform leftBoundary;
    [SerializeField] private Transform rightBoundary;
    private bool movingRight = true;

    [Header("Player Detection")]
    [SerializeField] private float detectionRange = 5f;
    private Transform player;
    
    [Header("Attack Settings")]
    [SerializeField] private float attackRange = 1f;
    [SerializeField] private float attackCooldown = 1f;
    [SerializeField] private int attackDamage = 10;
    [SerializeField] private Vector3 offset;
    [SerializeField] private float knockbackDistance = 1f;
    [SerializeField] private float knockbackDuration = 0.2f;
    [SerializeField] private float stunDuration = 0.5f;

    private float lastAttackTime = -Mathf.Infinity;
    private bool allowFlip = true;
    
    private void Update()
    {
        if (PlayerInRange())
        {
            float distanceToPlayer = Vector2.Distance(transform.position + offset, player.position);

            if (distanceToPlayer <= attackRange)
            {
                AttackPlayer();
            }
            else
            {
                ChasePlayer();
            }
        }
        else
        {
            Patrol();
        }
    }
    
    private void Patrol()
    {
        float step = moveSpeed * Time.deltaTime;
        if (movingRight)
        {
            transform.Translate(Vector2.right * step);
            Flip(-1);
            if (transform.position.x >= rightBoundary.position.x)
                movingRight = false;
        }
        else
        {
            transform.Translate(Vector2.left * step);
            Flip(1);
            if (transform.position.x <= leftBoundary.position.x)
                movingRight = true;
        }
    }
    
    private bool PlayerInRange()
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (player == null) return false;

        return Vector2.Distance(transform.position, player.position) <= detectionRange;
    }

    private void ChasePlayer()
    {
        if (player == null) return;

        float step = moveSpeed * Time.deltaTime;
        Vector2 direction = (player.position - transform.position).normalized;
        transform.Translate(direction * step);
        
        Flip(-direction.x);
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + offset, attackRange);

        if (leftBoundary != null && rightBoundary != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(leftBoundary.position, rightBoundary.position);
        }
    }
    
    private void AttackPlayer()
    {
        if (Time.time - lastAttackTime >= attackCooldown)
        {
            Debug.Log("Slime attacks player!");

            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(attackDamage, transform.position + offset, knockbackDistance, knockbackDuration, stunDuration);
            }

            lastAttackTime = Time.time;
        }
    }
    
    private void Flip(float direction)
    {
        if (!allowFlip) return;
        
        if (direction > 0)
            transform.localScale = new Vector3(1, 1, 1);  // Facing right
        else if (direction < 0)
            transform.localScale = new Vector3(-1, 1, 1); // Facing left
    }
}
