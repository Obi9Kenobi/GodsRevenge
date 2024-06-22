using UnityEngine;

public class MovePlayer : MonoBehaviour
{
    public Animator animator;

    [SerializeField] float groundMoveSpeed = 16f;
    [SerializeField] float airMoveSpeed = 0.3f;
    [SerializeField] float jumpingPower = 18f;
    [SerializeField] Transform groundCheck;
    [SerializeField] Transform wallCheck;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] Transform backWallCheck;
    [SerializeField] LayerMask backWallLayer;
    [SerializeField] float groundCheckRadius = 0.2f;
    [SerializeField] float wallCheckRadius = 1.2f;
    [SerializeField] float backWallCheckRadius = 0.6f;

    private Rigidbody2D rb;
    private float horizontalInput;
    private bool isFacingRight = true;
    private float currentHorizontalSpeed;
    private float currentVerticalSpeed;
    private GameObject lastCollidedWall = null;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        HandleInput();
        HandleJump();
        HandleBackWallJump();
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

    public bool IsGrounded()
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

    private GameObject GetCollidedBackWall()
    {
        if (IsWalled())
        {
            return (Physics2D.OverlapCircleAll(wallCheck.position, wallCheckRadius, backWallLayer)[0]).gameObject;
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

            spriteRenderer.flipX = !isFacingRight;
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
        if (Input.GetButtonDown("Jump") && (IsGrounded() || (IsWalled() && (GetCollidedWall() != lastCollidedWall))))
        {
            lastCollidedWall = GetCollidedWall();
            rb.velocity = new Vector2(horizontalInput * groundMoveSpeed, jumpingPower);
        }

        if (Input.GetButtonUp("Jump") && currentVerticalSpeed > 0f)
        {
            rb.velocity = new Vector2(currentHorizontalSpeed, currentVerticalSpeed * 0.5f);
        }
    }

    private void HandleWallJump()
    {
        if (Input.GetButtonDown("Jump") && (IsGrounded() || (IsWalled() && (GetCollidedWall() != lastCollidedWall))))
        {
            lastCollidedWall = GetCollidedWall();
            rb.velocity = new Vector2(horizontalInput * groundMoveSpeed, jumpingPower);
        }

        if (Input.GetButtonUp("Jump") && currentVerticalSpeed > 0f)
        {
            rb.velocity = new Vector2(currentHorizontalSpeed, currentVerticalSpeed * 0.5f);
        }
    }

    private void HandleBackWallJump()
    {
        if (Input.GetButtonDown("Jump") && IsBackWalled() && lastCollidedWall)
        {
            lastCollidedWall = GetCollidedBackWall();
            rb.velocity = new Vector2(currentHorizontalSpeed, jumpingPower);
        }
    }
}

