using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class SlimeAI : MonoBehaviour
{
    [Header("General Settings")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private GameObject visual;
    [SerializeField] protected EnemyHealth enemyHealth;

    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    [Header("Patrol Settings")]
    [SerializeField] private Transform leftBoundary;
    [SerializeField] private Transform rightBoundary;
    private bool movingRight = true;

    [Header("Player Detection")]
    [SerializeField] private float detectionRange = 5f;
    [SerializeField] private Vector3 detectionOffset;

    protected Transform player;
    
    [Header("Attack Settings")]
    [SerializeField] private float attackRange = 1f;
    [SerializeField] private float attackCooldown = 1f;
    [SerializeField] private int attackDamage = 10;
    [SerializeField] private Vector3 attackRangeoffset;
    [SerializeField] private float knockbackDistance = 1f;
    [SerializeField] private float knockbackDuration = 0.2f;
    [SerializeField] private float stunDuration = 0.5f;
    [SerializeField] private float reactionDelay = 0.5f;
    [SerializeField] private Color angryColor;
    private bool playerJustEnteredRange = true;
    private float lastAttackTime = -Mathf.Infinity;
    private bool allowFlip = true;
    private Tween colorTween;
    private Tween shakeTween;
    public Animator animator { get; private set; }
    private Vector3 lastPosition;
    private bool isMoving;

    private void Awake()
    {
        spriteRenderer = visual.GetComponent<SpriteRenderer>();
        animator = GetComponentInChildren<Animator>();
        originalColor = spriteRenderer.color;
    }
    
    private void OnEnable()
    {
        lastPosition = transform.position;
    }
    
    private void LateUpdate()
    {
        if (enemyHealth.IsDying) return;
        
        isMoving = Vector3.Distance(transform.position, lastPosition) > 0.001f;
        animator.SetBool("IsWalking", isMoving);
        lastPosition = transform.position;
    }

    protected virtual void Update()
    {
        if (enemyHealth.IsDying) return;
        
        if (PlayerInRange())
        {
            float distanceToPlayer = Vector2.Distance(transform.position + attackRangeoffset, player.position);

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

    protected bool PlayerInRange()
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (player == null) return false;
        
        bool inRange = Vector2.Distance(transform.position, player.position) <= detectionRange;
        
        if (!inRange)
        {
            playerJustEnteredRange = true;
            colorTween?.Kill();
            shakeTween?.Kill();
            visual.transform.localPosition = Vector3.zero;
            spriteRenderer.color = originalColor;
        }
        else if (playerJustEnteredRange)
        {
            playerJustEnteredRange = false;

            // Color Blending
            colorTween?.Kill();
            colorTween = spriteRenderer.DOColor(angryColor, reactionDelay * 0.5f)
                .SetLoops(2, LoopType.Yoyo)
                .OnComplete(() => spriteRenderer.color = originalColor);

            // Shake Effect
            shakeTween?.Kill();
            shakeTween = transform.DOShakePosition(reactionDelay, 0.2f, 10, 90, false, true)
                .SetEase(Ease.Linear)
                .OnComplete(() => visual.transform.localPosition = Vector3.zero);

            lastAttackTime = Time.time + reactionDelay - attackCooldown;
        }

        return inRange;
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
        Gizmos.DrawWireSphere(transform.position + detectionOffset, detectionRange);
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + attackRangeoffset, attackRange);

        if (leftBoundary != null && rightBoundary != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(leftBoundary.position, rightBoundary.position);
        }
    }
    
    private void AttackPlayer()
    {
        if (playerJustEnteredRange)
        {
            playerJustEnteredRange = false;
            lastAttackTime = Time.time + reactionDelay - attackCooldown;  // Start delay before first attack
            return;
        }
        
        if (Time.time - lastAttackTime >= attackCooldown)
        {
            Debug.Log("Slime attacks player!");
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(attackDamage, transform.position + attackRangeoffset, knockbackDistance, knockbackDuration, stunDuration);
                animator.SetTrigger("Attack");
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

    public void SetBoundary(Transform newLeftBoundary, Transform newRightBoundary)
    {
        leftBoundary = newLeftBoundary;
        rightBoundary = newRightBoundary;
    }
    
    public Transform GetBoundary(bool isRight)
    {
        if (isRight)
        {
            return rightBoundary;
        }
        else
        {
            return leftBoundary;
        }
    }
}
