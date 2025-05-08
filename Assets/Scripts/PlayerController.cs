using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField]private float moveSpeed = 5f;
    [SerializeField]private float jumpForce = 10f;
    
    [Header("Jump Control")]
    [SerializeField] private float jumpCooldown = 0.5f;
    private bool IsJumpReady => Time.time >= lastJumpTime + jumpCooldown;
    private float lastJumpTime = -Mathf.Infinity;
    
    
    [Header("Advanced Jumping")]
    [SerializeField] private float coyoteTime = 0.15f;    
    [SerializeField] private float jumpBufferTime = 0.1f;
    private float coyoteTimeCounter;
    private float jumpBufferCounter;
    
    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    private bool isGrounded;
    
    // Components
    private Rigidbody2D rb;
    private Animator anim;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        UpdateGroundedStatus();
        
        UpdateCoyoteTime();
        UpdateJumpBuffer();
        
        HandleMovement();
        HandleJump();
        UpdateAnimator();
    }
    
    private void UpdateGroundedStatus()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);
    }

    private void HandleMovement()
    {
        if(!IsGrounded()) return;
        
        float inputX = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(inputX * moveSpeed, rb.velocity.y);
        Flip(inputX);
        
        anim.SetBool("Move", inputX != 0);
    }

    private void HandleJump()
    {
        if (jumpBufferCounter > 0f && coyoteTimeCounter > 0f && IsJumpReady)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpBufferCounter = 0f;
            coyoteTimeCounter = 0f;
            lastJumpTime = Time.time;
        }
    }
    
    private void UpdateAnimator()
    {
        anim.SetBool("IsGround", isGrounded);
        anim.SetFloat("VerticalSpeed", rb.velocity.y);
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);
    }

    private void Flip(float input)
    {
        switch (input)
        {
            //Flip player
            case > 0:
                transform.localScale = Vector3.one; // Facing right
                break;
            case < 0:
                transform.localScale = new Vector3(-1, 1, 1); // Facing left
                break;
        }
    }
    
    private void UpdateCoyoteTime()
    {
        coyoteTimeCounter = isGrounded ? coyoteTime : coyoteTimeCounter - Time.deltaTime;
    }

    private void UpdateJumpBuffer()
    {
        if (Input.GetButtonDown("Jump") || Input.GetAxis("Vertical") > 0f)
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }
            
    }
}
