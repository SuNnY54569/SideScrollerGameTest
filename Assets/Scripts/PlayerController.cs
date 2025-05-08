using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]private float moveSpeed = 5f;
    [SerializeField]private float jumpForce = 10f;
    [SerializeField]private bool isGrounded;
    
    [SerializeField]private Transform groundCheck;
    [SerializeField]private LayerMask groundLayer;
    
    private Rigidbody2D rb;
    private Animator anim;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        isGrounded = IsGrounded();
        
        Move();
        Jump();
        
        anim.SetBool("IsGround", isGrounded);
    }

    private void Move()
    {
        if(!IsGrounded()) return;
        
        float inputX = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(inputX * moveSpeed, rb.velocity.y);

        switch (inputX)
        {
            //Flip player
            case > 0:
                transform.localScale = Vector3.one; // Facing right
                break;
            case < 0:
                transform.localScale = new Vector3(-1, 1, 1); // Facing left
                break;
        }
        
        anim.SetBool("Move", inputX != 0);
    }

    private void Jump()
    {
        if (Input.GetButtonDown("Jump") || (Input.GetAxis("Vertical") > 0) && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }
    
    bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);
    }
}
