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

    float horizontal;
    bool isFacingRight = true;
    float speed_x;
    float speed_y;
    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        speed_x = rb.velocity.x;
        speed_y = rb.velocity.y;

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            rb.velocity = new Vector2(speed_x, jumpingPower);
        }

        if (Input.GetButtonUp("Jump") && speed_y > 0f)
        {
            rb.velocity = new Vector2(speed_x, speed_y * 0.5f);
        }

        Flip();
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(horizontal * moveSpeed, speed_y);
        animator.SetFloat("Speed_Horizontal", Mathf.Abs(horizontal));
        animator.SetFloat("Speed_Vertical", speed_y);
        animator.SetBool("Is_Grounded", IsGrounded());
    }

    bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }
}
