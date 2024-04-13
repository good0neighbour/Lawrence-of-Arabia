using UnityEngine;
using static Constants;

public class HorizontalMovement : MonoBehaviour
{
    /* ==================== Fields ==================== */

    [SerializeField, HideInInspector] protected sbyte IsFlipNum = 1;
    protected float DeltaTime = 0.0f;
    protected bool IsGrounded = true;
    private GameDelegate _jumpState = null;
    private Collider2D _ignoredTerrain = null;
    private float _velocityY = 0.0f;
    [SerializeField, HideInInspector] private bool _isFlip = false;

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
        // Delta time limit
        if (Time.deltaTime >= DELTA_TIME_LIMIT)
        {
            DeltaTime = DELTA_TIME_LIMIT;
        }
        else
        {
            DeltaTime = Time.deltaTime;
        }

        // Position
        _jumpState.Invoke();
        transform.localPosition = new Vector3(
            transform.localPosition.x + weightX * DeltaTime * PLAYER_VEL,
            transform.localPosition.y + _velocityY * DeltaTime,
            0.0f
        );

        // Wall detecting
        switch (Physics2D.OverlapArea(
                (Vector2)transform.position + new Vector2(-0.48f, 0.5f),
                (Vector2)transform.position + new Vector2(0.48f, 0.4f),
                LAYER_B_WALL))
        {
            case null:
                return;

            default:
                transform.localPosition = new Vector3(
                    Mathf.Round(transform.localPosition.x + 0.5f) - 0.5f,
                    transform.localPosition.y,
                    0.0f
                );
                break;
        }
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
                if (DetectIsCeiled())
                {
                    // Cannot up jump under BaseGround
                    return;
                }
                _velocityY = PLAYER_JUMP_SPEED;
                _jumpState = JumpUp;
                IsGrounded = false;
            }
            else
            {
                _ignoredTerrain = DetectGround();
                //if (_ignoredTerrain.tag.Equals("BaseGround"))
                if (_ignoredTerrain.CompareTag("BaseGround"))
                {
                    // Cannot down jump on BaseGround
                    _ignoredTerrain = null;
                    return;
                }
                _jumpState = JumpDown;
                IsGrounded = false;
            }
        }
    }


    protected virtual void Awake()
    {
        _jumpState = JumpNone;
    }



    /* ==================== Private Methods ==================== */


    private bool DetectIsCeiled()
    {
        Collider2D ceiling = Physics2D.OverlapArea(
            (Vector2)transform.position + new Vector2(-PLAYER_FEET_SIZE, 1.01f),
            (Vector2)transform.position + new Vector2(PLAYER_FEET_SIZE, 0.99f),
            LAYER_B_TERRAIN
        );
        if (ceiling == null)
        {
            return false;
        }
        else if (ceiling.CompareTag("BaseGround"))
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    private Collider2D DetectGround()
    {
        return Physics2D.OverlapArea(
            (Vector2)transform.position + new Vector2(-PLAYER_FEET_SIZE, 0.1f),
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
            IsGrounded = false;
        }
    }


    private void JumpUp()
    {
        if (_velocityY <= 0.0f)
        {
            _jumpState = JumpDown;
            JumpDown();
            _velocityY = 0.0f;
            return;
        }
        _velocityY = _velocityY + GRAVITY_ACCELERATION * DeltaTime;
    }


    private void JumpDown()
    {
        if (DetectIsGrounded())
        {
            _jumpState = JumpNone;
            _velocityY = 0.0f;
            _ignoredTerrain = null;
            transform.position = new Vector3(
                transform.position.x,
                Mathf.Round(transform.position.y),
                0.0f
            );
            IsGrounded = true;
            return;
        }
        else if (_velocityY > PLAYER_MAX_FALLING_VEL)
        {
            _velocityY = _velocityY + GRAVITY_ACCELERATION * DeltaTime;
        }
    }
}
