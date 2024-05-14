using UnityEngine;
using static Constants;

public class EventNPCMovement : HorizontalMovement
{
    /* ==================== Fields ==================== */

    [Header("Movement")]
    [SerializeField] private float _moveSpeed = 0.1f;
    [SerializeField] private float _acceleration = 0.05f;
    [Header("References")]
    [SerializeField] private SpriteRenderer _sprite = null;
    private GameDelegate _behavDel = null;
    private float _velocity = 0.0f;
    private float _goalPosX = 0.0f;

    public override bool Flip
    {
        get
        {
            return base.Flip;
        }
        set
        {
            base.Flip = value;
            _sprite.flipX = value;
        }
    }



    /* ==================== Public Methods ==================== */

    public void SetGoal(float goalPosX)
    {
        _goalPosX = goalPosX;
        if (goalPosX > transform.position.x)
        {
            _behavDel = MoveRight;
        }
        else if (goalPosX < transform.position.x)
        {
            _behavDel = MoveLeft;
        }
    }


    public void NPCJump(float goalPosY)
    {
        if (goalPosY >= transform.position.y)
        {
            Jump(true);
        }
        else
        {
            Jump(false);
        }
    }


    public void NPCLookAt(Transform target)
    {
        if (target.position.x >= transform.position.x)
        {
            Flip = false;
        }
        else
        {
            Flip = true;
        }
    }



    /* ==================== Protected Methods ==================== */

    protected override void Update()
    {
        base.Update();

        // Moving stop
        if (_velocity != 0.0f)
        {
            switch (IsFlipNum)
            {
                case -1:
                    if (transform.position.x <= _goalPosX)
                    {
                        _behavDel = null;
                        StopMovingLeft();
                    }
                    break;

                case 1:
                    if (transform.position.x >= _goalPosX)
                    {
                        _behavDel = null;
                        StopMovingRight();
                    }
                    break;
            }
        }
    }



    /* ==================== Private Methods ==================== */

    private void MoveLeft()
    {
        _velocity -= Time.fixedDeltaTime * _acceleration;
        if (_velocity <= -_moveSpeed)
        {
            _velocity = -_moveSpeed;
            _behavDel = null;
        }
    }


    private void StopMovingLeft()
    {
        _velocity += Time.fixedDeltaTime * _acceleration;
        if (_velocity >= 0.0f)
        {
            _velocity = 0.0f;
            _behavDel = null;
        }
    }


    private void MoveRight()
    {
        _velocity += Time.fixedDeltaTime * _acceleration;
        if (_velocity >= _moveSpeed)
        {
            _velocity = _moveSpeed;
            _behavDel = null;
        }
    }


    private void StopMovingRight()
    {
        _velocity -= Time.fixedDeltaTime * _acceleration;
        if (_velocity <= 0.0f)
        {
            _velocity = 0.0f;
            _behavDel = null;
        }
    }


    private void FixedUpdate()
    {
        // Moving
        _behavDel?.Invoke();
        SetPositionWithFlip(_velocity);
    }
}
