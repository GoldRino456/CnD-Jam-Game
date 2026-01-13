using UnityEngine;
using UnityEngine.InputSystem;

//Great character controller by Sasquatch B Studios: https://www.youtube.com/watch?v=zHSWG05byEc

public class PlayerMovement : MonoBehaviour
{
    public PlayerMoveSettings moveSettings;
    [SerializeField] private Collider2D bodyCollider;
    [SerializeField] private Collider2D groundCollider;

    private Rigidbody2D rb;

    private Vector2 moveVelocity;
    private bool isFacingRight;

    private RaycastHit2D groundHit;
    private RaycastHit2D headHit;
    private bool isGrounded;
    private bool isHeadBumped;

    public float VerticalVelocity { get; private set; }
    private bool isJumping;
    private bool isFastFalling;
    private bool isFalling;
    private float fastFallTime;
    private float fastFallReleaseSpeed;
    private int numberOfJumpsUsed;

    private float apexPoint;
    private float timePastApexThreshold;
    private bool isPastApexThreshold;

    private float jumpBufferTimer;
    private bool jumpReleasedDuringBuffer;

    private float coyoteTimer;

    private void Awake()
    {
        isFacingRight = true;
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        CountTimers();
        JumpCheck();
    }

    private void FixedUpdate()
    {
        ProcessCollisionChecks();
        Jump();

        if(isGrounded)
        {
            Move(moveSettings.groundAcceleration, moveSettings.groundDeceleration, InputManager.moveDirection);
        }
        else
        {
            Move(moveSettings.airAcceleration, moveSettings.airDeceleration, InputManager.moveDirection);
        }
    }

    private void Move(float acceleration, float deceleration, Vector2 moveInput)
    {
        if (moveInput != Vector2.zero)
        {
            TurnCheck(moveInput);
            Vector2 targetVelocity = new Vector2(moveInput.x, 0f) * moveSettings.maxMoveSpeed;
            moveVelocity = Vector2.Lerp(moveVelocity, targetVelocity, acceleration * Time.deltaTime);
            rb.linearVelocity = new Vector2(moveVelocity.x, rb.linearVelocity.y);
        }
        else if(moveInput == Vector2.zero)
        {
            moveVelocity = Vector2.Lerp(moveVelocity, Vector2.zero, deceleration * Time.deltaTime);
            rb.linearVelocity = new Vector2(moveVelocity.x, rb.linearVelocity.y);
        }
    }

    private void Jump()
    {
        //Apply gravity while jumping
        if(isJumping)
        {
            //Check for head bump
            if(isHeadBumped)
            {
                isFastFalling = true;
            }

            //Gravity on Ascend
            if(VerticalVelocity >= 0f)
            {
                //Apex Controls
                apexPoint = Mathf.InverseLerp(moveSettings.InitialJumpVelocity, 0f, VerticalVelocity);

                if(apexPoint > moveSettings.apexThreshold)
                {
                    if(!isPastApexThreshold)
                    {
                        isPastApexThreshold = true;
                        timePastApexThreshold = 0f;
                    }

                    if(isPastApexThreshold)
                    {
                        timePastApexThreshold += Time.fixedDeltaTime;
                        if(timePastApexThreshold < moveSettings.apexHangTime)
                        {
                            VerticalVelocity = 0f;
                        }
                        else
                        {
                            VerticalVelocity = -0.01f;
                        }
                    }
                }

                //Gravity on Ascend but not past apex threshold
                else
                {
                    VerticalVelocity += moveSettings.Gravity * Time.fixedDeltaTime;
                    if(isPastApexThreshold)
                    {
                        isPastApexThreshold = false;
                    }
                }
            }

            else if (!isFastFalling)
            {
                VerticalVelocity += moveSettings.Gravity * moveSettings.gravityOnReleaseMultiplier * Time.fixedDeltaTime;
            }

            else if (VerticalVelocity < 0f)
            {
                if(!isFalling)
                {
                    isFalling = true;
                }
            }
        }

        if(isFastFalling)
        {
            if(fastFallTime >= moveSettings.timeForUpwardsCancel)
            {
                VerticalVelocity += moveSettings.Gravity * moveSettings.gravityOnReleaseMultiplier * Time.fixedDeltaTime;
            }
            else if (fastFallTime < moveSettings.timeForUpwardsCancel)
            {
                VerticalVelocity = Mathf.Lerp(fastFallReleaseSpeed, 0f, (fastFallTime / moveSettings.timeForUpwardsCancel));
            }

            fastFallTime += Time.fixedDeltaTime;
        }
        

        if(!isGrounded && !isJumping)
        {
            if(!isFalling)
            {
                isFalling = true;
            }

            VerticalVelocity += moveSettings.Gravity * Time.fixedDeltaTime;
        }

        VerticalVelocity = Mathf.Clamp(VerticalVelocity, -moveSettings.maxFallSpeed, 50f);

        rb.linearVelocity = new Vector2(rb.linearVelocity.x, VerticalVelocity);
    }

    private void JumpCheck()
    {
        if(InputManager.isJumpPressed)
        {
            jumpBufferTimer = moveSettings.jumpBufferTime;
            jumpReleasedDuringBuffer = false;
        }

        if(InputManager.isJumpReleased)
        {
            if(jumpBufferTimer > 0f)
            {
                jumpReleasedDuringBuffer = true;
            }

            if(isJumping && VerticalVelocity > 0f)
            {
                if(isPastApexThreshold)
                {
                    isPastApexThreshold = false;
                    isFastFalling = true;
                    fastFallTime = moveSettings.timeForUpwardsCancel;
                    VerticalVelocity = 0f;
                }
                else
                {
                    isFastFalling = true;
                    fastFallReleaseSpeed = VerticalVelocity;
                }
            }
        }
        //init jump with buffer and coyote time
        if(jumpBufferTimer > 0f && !isJumping && (isGrounded || coyoteTimer > 0f))
        {
            InitiateJump(1);

            if(jumpReleasedDuringBuffer)
            {
                isFastFalling = true;
                fastFallReleaseSpeed = VerticalVelocity;
            }
        }
        //Double jump
        else if (jumpBufferTimer > 0f && isJumping && numberOfJumpsUsed < moveSettings.numJumpsAllowed)
        {
            isFastFalling = false;
            InitiateJump(1);
        }
        //Air jump AFTER coyote time expires
        else if (jumpBufferTimer > 0f && isFalling && numberOfJumpsUsed < moveSettings.numJumpsAllowed - 1)
        {
            InitiateJump(2);
            isFastFalling = false;
        }
        //Landed
        if((isJumping || isFalling) && isGrounded && VerticalVelocity <= 0f)
        {
            isJumping = false;
            isFalling = false;
            isFastFalling = false;
            fastFallTime = 0f;
            isPastApexThreshold = false;
            numberOfJumpsUsed = 0;

            VerticalVelocity = Physics2D.gravity.y;
        }
    }

    private void InitiateJump(int numberOfJumpsUsed)
    {
        if(!isJumping)
        {
            isJumping = true;
        }

        jumpBufferTimer = 0f;
        numberOfJumpsUsed += numberOfJumpsUsed;
        VerticalVelocity = moveSettings.InitialJumpVelocity;
    }

    private void TurnCheck(Vector2 moveInput)
    {
        if(isFacingRight && moveInput.x < 0)
        {
            Turn(false);
        }
        else if(!isFacingRight && moveInput.x > 0)
        {
            Turn(true);
        }
    }

    private void Turn(bool turnRight)
    {
        if(turnRight)
        {
            isFacingRight = true;
            transform.Rotate(0f, 180f, 0f);
        }
        else
        {
            isFacingRight = false;
            transform.Rotate(0f, -180f, 0f);
        }
    }

    private void ProcessCollisionChecks()
    {
        CheckIsGrounded();
        CheckIsHeadBumped();
    }

    private void CheckIsGrounded()
    {
        Vector2 boxCastOrigin = new(groundCollider.bounds.center.x, groundCollider.bounds.min.y);
        Vector2 boxCastSize = new(groundCollider.bounds.size.x, moveSettings.groundDetectionRayLength);

        groundHit = Physics2D.BoxCast(boxCastOrigin, boxCastSize, 0f, Vector2.down, moveSettings.groundDetectionRayLength, moveSettings.groundLayer);

        if(groundHit.collider != null)
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }

    private void CheckIsHeadBumped()
    {
        Vector2 boxCastOrigin = new(groundCollider.bounds.center.x, bodyCollider.bounds.max.y);
        Vector2 boxCastSize = new(groundCollider.bounds.size.x * moveSettings.headWidth, moveSettings.headDetectionRayLength);

        headHit = Physics2D.BoxCast(boxCastOrigin, boxCastSize, 0f, Vector2.up, moveSettings.headDetectionRayLength, moveSettings.groundLayer);

        if(headHit.collider != null)
        {
            isHeadBumped = true;
        }
        else
        {
            isHeadBumped = false;
        }
    }

    private void CountTimers()
    {
        jumpBufferTimer -= Time.deltaTime;

        if(!isGrounded)
        {
            coyoteTimer -= Time.deltaTime;
        }
        else
        {
            coyoteTimer = moveSettings.jumpCoyoteTime;
        }
    }
}
