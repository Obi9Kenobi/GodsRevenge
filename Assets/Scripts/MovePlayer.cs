using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour
{
    public Animator animator;

    [SerializeField] float moveSpeed = 8f;
    [SerializeField] float jumpingPower = 16f;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] float GroundCheckRadius = 0.2f;

    private float horizontalInput;
    private bool jumpInput;
    private bool isFacingRight = true;
    private float currentHorizontalSpeed;
    private float currentVerticalSpeed;

    private void Update()
    {
        HandleInput();
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
        rb.velocity = new Vector2(horizontalInput * moveSpeed, currentVerticalSpeed);
    }

    private void HandleJump()
    {
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            rb.velocity = new Vector2(currentHorizontalSpeed, jumpingPower);
        }

        if (Input.GetButtonUp("Jump") && currentVerticalSpeed > 0f)
        {
            rb.velocity = new Vector2(currentHorizontalSpeed, currentVerticalSpeed * 0.5f);
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, GroundCheckRadius, groundLayer);
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
