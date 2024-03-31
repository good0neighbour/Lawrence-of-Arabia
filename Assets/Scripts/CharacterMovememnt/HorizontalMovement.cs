using UnityEngine;

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



    /* ==================== Protected Methods ==================== */

    public virtual void Flip(bool flip)
    {
        _isFlip = flip;
        if (flip)
        {
            IsFlipNum = -1;
        }
        else
        {
            IsFlipNum = 1;
        }
    }


    public bool Flip()
    {
        return _isFlip;
    }



    /* ==================== Protected Methods ==================== */

    /// <summary>
    /// Sets local position of character
    /// </summary>
    protected void SetPosition(float weightX)
    {
        Vector2 pos;

        // Delta time limit
        if (Time.deltaTime >= Constants.DELTA_TIME_LIMIT)
        {
            DeltaTime = Constants.DELTA_TIME_LIMIT;
        }
        else
        {
            DeltaTime = Time.deltaTime;
        }

        // Position
        _jumpState.Invoke();
        pos = (Vector2)transform.localPosition;
        transform.localPosition = new Vector3(
            pos.x + weightX * DeltaTime * Constants.CHAR_VEL,
            pos.y + _velocityY * DeltaTime,
            0.0f
        );

        // Wall detecting
        pos = (Vector2)transform.position;
        if (null != Physics2D.OverlapArea(
            pos + new Vector2(-0.48f, 0.1f),
            pos + new Vector2(0.48f, 0.05f),
            Constants.LAYER_B_WALL
        ))
        {
            pos = (Vector2)transform.localPosition;
            transform.localPosition = new Vector3(
                Mathf.Round(pos.x + 0.5f) - 0.5f,
                pos.y,
                0.0f
            );
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
            Flip(false);
        }
        else if (weightX < 0.0f)
        {
            Flip(true);
        }
    }


    protected void Jump(bool isUpJump)
    {
        if (IsGrounded)
        {
            if (isUpJump)
            {
                _velocityY = Constants.CHAR_JUMP_SPEED;
                _jumpState = JumpUp;
                IsGrounded = false;
            }
            else
            {
                _ignoredTerrain = DetectGround();
                if (_ignoredTerrain.tag.Equals("BaseGround"))
                {
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

    private Collider2D DetectGround()
    {
        return Physics2D.OverlapArea(
            (Vector2)transform.position + new Vector2(-Constants.CHAR_FEET_SIZE, 0.1f),
            (Vector2)transform.position + new Vector2(Constants.CHAR_FEET_SIZE, -0.01f),
            Constants.LAYER_B_TERRAIN
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
        _velocityY = _velocityY + Constants.GRAVITY_ACCELERATION * DeltaTime;
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
        else if (_velocityY > Constants.CHAR_MAX_FALLING_VEL)
        {
            _velocityY = _velocityY + Constants.GRAVITY_ACCELERATION * DeltaTime;
        }
    }
}
