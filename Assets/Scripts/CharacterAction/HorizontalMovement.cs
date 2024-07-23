using UnityEngine;
using static Constants;

public class HorizontalMovement : MonoBehaviour
{
    /* ==================== Fields ==================== */

    [SerializeField, HideInInspector] protected sbyte IsFlipNum = 1;
    [SerializeField, HideInInspector] private bool _isFlip = false;
    protected Animator Animator = null;
    protected bool IsGrounded = true;
    protected bool Paused = true;
    private GameDelegate _jumpState = null;
    private Collider2D _ignoredTerrain = null;
    private Rigidbody2D _rigidbody = null;
    private bool _isGroundedMem = true;

    public virtual bool Flip
    {
        get
        {
            return _isFlip;
        }
        set
        {
            _isFlip = value;
            if (value)
            {
                IsFlipNum = -1;
            }
            else
            {
                IsFlipNum = 1;
            }
        }
    }



    /* ==================== Protected Methods ==================== */

    /// <summary>
    /// Sets local position of character
    /// </summary>
    protected void SetPosition(float weightX)
    {
        // Jump action
        _jumpState.Invoke();

        // Position
        _rigidbody.velocity = new Vector3(weightX * PLAYER_VEL, _rigidbody.velocity.y, 0.0f);
    }


    /// <summary>
    /// Sets local position of character with flipping
    /// </summary>
    /// <returns>Returns X flip</returns>
    protected void SetPositionWithFlip(float weightX)
    {
        SetPosition(weightX);

        // Flip check
        if (weightX > 0.0f)
        {
            Flip = false;
        }
        else if (weightX < 0.0f)
        {
            Flip = true;
        }
    }


    protected void Jump(bool isUpJump)
    {
        if (IsGrounded)
        {
            if (isUpJump)
            {
                _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, PLAYER_JUMP_SPEED, 0.0f);
                _rigidbody.gravityScale = GRAVITY_SCALE;
                _jumpState = JumpUp;
                IsGrounded = false;
            }
            else
            {
                _ignoredTerrain = DetectGround();
                if (_ignoredTerrain.gameObject.layer == LAYER_D_WALL)
                {
                    // Cannot down jump on Wall layer
                    _ignoredTerrain = null;
                    return;
                }
                _rigidbody.gravityScale = GRAVITY_SCALE;
                _jumpState = JumpDown;
                IsGrounded = false;
            }
        }
    }


    protected virtual void Awake()
    {
        // Reference
        _rigidbody = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();

        // Initial jump action
        _jumpState = JumpNone;
    }


    protected virtual void Update()
    {
        // Animation
        if (_isGroundedMem)
        {
            if (IsGrounded)
            {
                Animator.SetFloat("Velocity", Mathf.Abs(_rigidbody.velocity.x));
            }
            else
            {
                Animator.SetBool("IsGrounded", false);
                _isGroundedMem = false;
            }
        }
        else if (IsGrounded)
        {
            Animator.SetBool("IsGrounded", true);
            _isGroundedMem = true;
        }
    }



    /* ==================== Private Methods ==================== */

    private Collider2D DetectGround()
    {
        return Physics2D.OverlapArea(
            (Vector2)transform.position + new Vector2(-PLAYER_FEET_SIZE, 0.2f),
            (Vector2)transform.position + new Vector2(PLAYER_FEET_SIZE, -0.01f),
            LAYER_B_TERRAIN
        );
    }


    private bool DetectIsGrounded()
    {
        Collider2D terrains = DetectGround();

        if (terrains == null || terrains == _ignoredTerrain)
        {
            return false;
        }
        else
        {
            return true;
        }
    }


    private void JumpNone()
    {
        if (!DetectIsGrounded())
        {
            _jumpState = JumpDown;
            _rigidbody.gravityScale = GRAVITY_SCALE;
            IsGrounded = false;
        }
    }


    private void JumpUp()
    {
        if (_rigidbody.velocity.y <= 0.0f)
        {
            _jumpState = JumpDown;
            JumpDown();
            return;
        }
    }


    private void JumpDown()
    {
        if (DetectIsGrounded())
        {
            _jumpState = JumpNone;
            _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, 0.0f, 0.0f);
            _rigidbody.gravityScale = 0.0f;
            _ignoredTerrain = null;
            transform.position = new Vector3(
                transform.position.x,
                Mathf.Round(transform.position.y),
                0.0f
            );
            IsGrounded = true;
        }
        else if (_rigidbody.velocity.y < PLAYER_MAX_FALLING_VEL)
        {
            _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, PLAYER_MAX_FALLING_VEL, 0.0f);
            _rigidbody.gravityScale = 0.0f;
        }
    }
}
