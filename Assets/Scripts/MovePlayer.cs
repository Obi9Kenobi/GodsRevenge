using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using UnityEngine;

public class MovePlayer : MonoBehaviour
{
    public Animator animator;

    [SerializeField] float GroundMoveSpeed = 16f;
    [SerializeField] float jumpingPower = 18f;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Transform groundCheck;
    [SerializeField] Transform wallCheck;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] Transform backWallCheck;
    [SerializeField] LayerMask backWallLayer;
    [SerializeField] float groundCheckRadius = 0.2f;
    [SerializeField] float wallCheckRadius = 0.2f;
    [SerializeField] float backWallCheckRadius = 0.6f;

    private float horizontalInput;
    private bool isFacingRight = true;
    private float currentHorizontalSpeed;
    private float currentVerticalSpeed;
    private bool jumpLocked = false;

    private void Update()
    {
        HandleInput();
        UnlockJumpIfNeeded();
        HandleJump();
        UpdateAnimation();
        FlipIfNeeded();
    }

    private void FixedUpdate()
    {
        currentHorizontalSpeed = rb.velocity.x;
        currentVerticalSpeed = rb.velocity.y;
        HandleMovement();
    }

    private void HandleInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
    }

    private void UpdateAnimation()
    {
        animator.SetFloat("Speed_Horizontal", Mathf.Abs(horizontalInput));
        animator.SetFloat("Speed_Vertical", currentVerticalSpeed);
        animator.SetBool("Is_Grounded", IsGrounded());
    }

    private void HandleMovement()
    {
        rb.velocity = new Vector2(horizontalInput * GroundMoveSpeed, currentVerticalSpeed);
    }

    private void HandleJump()
    {
        if (Input.GetButtonDown("Jump") && (IsGrounded() || IsWalled() || IsBackWalled()) && (!jumpLocked))
        {
            rb.velocity = new Vector2(currentHorizontalSpeed, jumpingPower);
            LockJump();  
        }

        if (Input.GetButtonUp("Jump") && currentVerticalSpeed > 0f)
        {
            rb.velocity = new Vector2(currentHorizontalSpeed, currentVerticalSpeed * 0.5f);
        }
    }

    private void LockJump()
    {
        jumpLocked = true;
    }
    private void UnlockJumpIfNeeded()        
    {
        if (IsGrounded() || currentHorizontalSpeed != 0)
        {
            jumpLocked = false;
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }
    private bool IsWalled()
    {
        return Physics2D.OverlapCircle(wallCheck.position, wallCheckRadius, groundLayer);
    }
    private bool IsBackWalled()
    {
        return Physics2D.OverlapCircle(backWallCheck.position, backWallCheckRadius, backWallLayer);
    }

    private void FlipIfNeeded()
    {
        if (isFacingRight && horizontalInput < 0f || !isFacingRight && horizontalInput > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }
}
