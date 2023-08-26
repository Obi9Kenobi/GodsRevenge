using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.XR;

public class MovePlayer : MonoBehaviour
{
    public Animator animator;

    [SerializeField] float groundMoveSpeed = 16f;
    [SerializeField] float airMoveSpeed = 0.3f;
    [SerializeField] float jumpingPower = 18f;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Transform groundCheck;
    [SerializeField] Transform wallCheck;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] Transform backWallCheck;
    [SerializeField] LayerMask backWallLayer;
    [SerializeField] float groundCheckRadius = 0.2f;
    [SerializeField] float wallCheckRadius = 1.2f;
    [SerializeField] float backWallCheckRadius = 0.6f;

    private float horizontalInput;
    private bool isFacingRight = true;
    private float currentHorizontalSpeed;
    private float currentVerticalSpeed;
    private GameObject lastCollidedWall = null;

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
        Debug.Log(currentHorizontalSpeed);
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

    private GameObject GetCollidedWall()
    {
        if (IsWalled())
        {
            return (Physics2D.OverlapCircleAll(wallCheck.position, wallCheckRadius, groundLayer)[0]).gameObject;
        }
        else
        {
            return null;
        }
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
    private void HandleMovement()
    {
        if (IsGrounded())
        {   
            rb.velocity = new Vector2(horizontalInput * groundMoveSpeed, currentVerticalSpeed);
        }
        else
        {
            if ((currentHorizontalSpeed < groundMoveSpeed) && (horizontalInput > 0))
            {
                rb.velocity = new Vector2(currentHorizontalSpeed + airMoveSpeed, currentVerticalSpeed);
            }
            else if ((currentHorizontalSpeed > -groundMoveSpeed) && (horizontalInput < 0))
            {
                rb.velocity = new Vector2(currentHorizontalSpeed - airMoveSpeed, currentVerticalSpeed);
            }
        }
    }

    private void HandleJump()
    {
        if (Input.GetButtonDown("Jump") && (IsGrounded() || (IsWalled() && (GetCollidedWall() != lastCollidedWall)) || IsBackWalled()))
        {
            lastCollidedWall = GetCollidedWall();
            rb.velocity = new Vector2(horizontalInput * groundMoveSpeed, jumpingPower);
        }

        if (Input.GetButtonUp("Jump") && currentVerticalSpeed > 0f)
        {
            rb.velocity = new Vector2(currentHorizontalSpeed, currentVerticalSpeed * 0.5f);
        }
    }
}

