using System.Data.Common;
using UnityEditor.Rendering.LookDev;
using UnityEngine;

public class PlayerMovement_platformer : MonoBehaviour, IPlayerMovementHandler
{    
    [Header("References")]
    [SerializeField] private Collider2D _feetCollider;
    [SerializeField] private Collider2D _bodyCollider;
    private Rigidbody2D _rb;
    private InputReader _input;
    private PlayerMovementStats _moveStats;

    //movement
    private Vector2 moveVelocity;
    private bool isFacingRight;

    //collision
    private RaycastHit2D groundHit;
    private RaycastHit2D headHit;
    private bool isGrounded;
    private bool bumpedHead;

    //jump
    public float verticalVelocity {get; private set;}
    private bool isJumping;
    private bool isFastFalling;
    private bool isFalling;
    private float fastFallTime;
    private float fastFallReleaseSpeed;
    private int numberOfJumpsUsed;

    //apex vars
    private float apexPoint;
    private float timePastApexThreshold;
    private bool isPastApexThreshold;

    //jump buffer vars
    private float jumpBufferTimer;
    private bool jumpReleasedDuringBuffer;

    //coyote time vars
    private float coyoteTimer;

    public void Initialize(InputReader _i, PlayerMovementStats _move, Collider2D _body, Collider2D _feet)
    {
        isFacingRight = true;
        _input = _i;
        _moveStats = _move;
        _rb = GetComponent<Rigidbody2D>();
        _bodyCollider = _body;
        _feetCollider = _feet;
    }
    void Update()
    {
        JumpChecks();
        UpdateTimers();
    }
    private void FixedUpdate()
    {
        CollisionChecks();
    }
    #region Movement
    private void Move(Vector2 moveInput)
    {
        
        if(GameManager.i.pauseManager.isPaused)
        {
            _rb.linearVelocity = Vector2.zero;
            return;
        }

        if(moveInput != Vector2.zero)
        {
            TurnCheck(moveInput);
            Vector2 velocity = new Vector2(moveInput.x, 0f) * _moveStats.moveSpeed;
            _rb.linearVelocity = new Vector2(velocity.x, _rb.linearVelocity.y);
        }
        else _rb.linearVelocity = Vector2.zero;
    }
    #endregion
    #region Jump
    private void JumpChecks()
    {
        if(_input.jumpPressed)
        {
            jumpBufferTimer = _moveStats.jumpBufferLength;
            jumpReleasedDuringBuffer = false;
        }
        
        if(_input.jumpReleased)
        {
            if(jumpBufferTimer > 0)
            {
                jumpReleasedDuringBuffer = true;
            }
            if(isJumping && verticalVelocity > 0f)
            {
                if(isPastApexThreshold)
                {
                    isPastApexThreshold = false;
                    isFastFalling = true;
                    fastFallTime = _moveStats.timeForUpwardsCancel;
                    verticalVelocity = 0f;
                }
                else
                {
                    isFastFalling = true;
                    fastFallReleaseSpeed = verticalVelocity;
                }
            }
        } 
        if(jumpBufferTimer > 0f && !isJumping && (isGrounded || coyoteTimer > 0f))
        {
            InitiateJump(1);
            if(jumpReleasedDuringBuffer)
            {
                isFastFalling = true;
                fastFallReleaseSpeed = verticalVelocity;
            }
        }
        //double jump if being used
        else if (jumpBufferTimer > 0f && isJumping && numberOfJumpsUsed < _moveStats.numberOfJumpsAllowed)
        {
            isFastFalling = false;
            InitiateJump(1);
        }
        //air jump after coyote time
        else if (jumpBufferTimer > 0f && isFalling && numberOfJumpsUsed < _moveStats.numberOfJumpsAllowed)
        {
            InitiateJump(2);
            isFastFalling = false;
        } 

        if((isJumping || isFalling) && isGrounded && verticalVelocity <= 0)
        {
            isJumping = false;
            isFalling = false;
            isFastFalling = false;
            fastFallTime = 0f;
            isPastApexThreshold = false;
            numberOfJumpsUsed = 0;

            verticalVelocity = Physics2D.gravity.y;
        }  
    }
    private void InitiateJump(int jumpsUsed)
    {
        if(!isJumping) isJumping = true;
        
        jumpBufferTimer = 0f;
        numberOfJumpsUsed += jumpsUsed;
        verticalVelocity = _moveStats.InitialJumpVelocity;
    }

    private void Jump()
    {
        if(isJumping)
        {
            if(bumpedHead) isFastFalling = true;

            if(verticalVelocity >= 0)
            {
                apexPoint = Mathf.InverseLerp(_moveStats.InitialJumpVelocity, 0f, verticalVelocity);
                if(apexPoint > _moveStats.apexThreshold)
                {
                    if(!isPastApexThreshold)
                    {
                        isPastApexThreshold = true;
                        timePastApexThreshold = 0f;
                    }

                    if(isPastApexThreshold)
                    {
                        timePastApexThreshold += Time.fixedDeltaTime;
                        if(timePastApexThreshold < _moveStats.apexHangTime) 
                            verticalVelocity = 0f;
                        else
                            verticalVelocity = -.01f;
                    }
                }
                else
                {
                    verticalVelocity += _moveStats.Gravity * Time.fixedDeltaTime;
                    if(isPastApexThreshold) isPastApexThreshold = false;
                }
            }
            else if (!isFastFalling)
            {
                verticalVelocity += _moveStats.Gravity * _moveStats.gravityOnReleaseMultiplier * Time.fixedDeltaTime;
            }
            else if(verticalVelocity < 0)
            {
                if(!isFalling) isFalling = true;
            }
        }

        if(isGrounded & !isJumping)
        {
            if(!isFalling) isFalling = true;
            
            verticalVelocity += _moveStats.Gravity * Time.fixedDeltaTime;
        }
        verticalVelocity = Mathf.Clamp(verticalVelocity, -_moveStats.maxFallSpeed, 50f);
        _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, verticalVelocity);
    }
    #endregion
    #region Checks
    private void TurnCheck(Vector2 moveInput)
    {
        
        if(isFacingRight && moveInput.x < 0) Turn(false); 
        else if(!isFacingRight && moveInput.x > 0) Turn(true);
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
    private void IsGrounded()
    {
        Vector2 boxCastOrigin = new Vector2(_feetCollider.bounds.center.x, 
            _feetCollider.bounds.min.y);
        Vector2 boxCastSize = new Vector2(_feetCollider.bounds.size.x, 
            _moveStats.groundDetectionRayLength);

        groundHit = Physics2D.BoxCast(boxCastOrigin, boxCastSize, 0f, 
            Vector2.down, _moveStats.groundDetectionRayLength, 
            GameManager.i.staticVariables.GetGroundLayer());
        
        if(groundHit.collider != null) isGrounded = true;
        else isGrounded = false;
        
        #region Debug Visualization
        if(_moveStats.debugShowIsGroundedBox)
        {
            Color rayColor;
            if(isGrounded) rayColor = Color.green;
            else rayColor = Color.red;

            Debug.DrawRay(new Vector2(boxCastOrigin.x - boxCastSize.x / 2, boxCastOrigin.y), Vector2.down * _moveStats.groundDetectionRayLength, rayColor);
            Debug.DrawRay(new Vector2(boxCastOrigin.x + boxCastSize.x / 2, boxCastOrigin.y), Vector2.down * _moveStats.groundDetectionRayLength, rayColor);
            Debug.DrawRay(new Vector2(boxCastOrigin.x - boxCastSize.x / 2, boxCastOrigin.y - _moveStats.groundDetectionRayLength), Vector2.right * boxCastSize.x, rayColor);
        }
        #endregion

    }
    private void CollisionChecks()
    {
        IsGrounded();
    }
    #endregion
    #region Timers
    private void UpdateTimers()
    {
        jumpBufferTimer -= Time.deltaTime;
        if(!isGrounded) coyoteTimer -= Time.deltaTime;
        else coyoteTimer = _moveStats.jumpCoyoteTime;
    }
    #endregion

}
