using UnityEngine;

public class HorizontalPlayerControl : HorizontalMovement
{
    /* ==================== Fields ==================== */

    [SerializeField] private SpriteRenderer _sprite = null;
    [SerializeField] private Joystick _joystick = null;
    [SerializeField] private Animator _animator = null;
    [SerializeField] private Transform _cameraPos = null;
    private GameDelegate _onInteract = null;
    private bool _jumpAvailable = true;
    private bool _isGroundedMem = true;
    private bool _paused = true;
    private bool _interaction = false;

    public static HorizontalPlayerControl Instance
    {
        get;
        private set;
    }

    public Transform CameraPos
    {
        get
        {
            return _cameraPos;
        }
    }



    /* ==================== Public Methods ==================== */

    public void Attack()
    {
        Debug.Log("Player Attack");
    }


    public void Interact()
    {
        _onInteract.Invoke();
    }


    public void Extra()
    {
        Debug.Log("Extra Function");
    }


    public void PausePlayerControl(bool pause)
    {
        _paused = pause;
    }


    public void SetInteractBtnActive(GameDelegate action)
    {
        CanvasPlayController.Instance.SetInteractBtnActive(true);
        _interaction = true;
        _onInteract = action;
    }


    public void SetInteractBtnInactive()
    {
        CanvasPlayController.Instance.SetInteractBtnActive(false);
        _interaction = false;
        _onInteract = null;
    }


    public override void Flip(bool flip)
    {
        base.Flip(flip);
        _sprite.flipX = flip;
    }



    /* ==================== Protected Methods ==================== */

    protected override void Awake()
    {
        base.Awake();
        Instance = this;
    }



    /* ==================== Private Methods ==================== */

    private void Update()
    {
        #region Always Functioning
        Vector2 joystick = _joystick.JoystickWeight;

        // Jump
        if (_jumpAvailable)
        {
            if (joystick.y > Constants.JOYSTICK_JUMP_WEIGHT)
            {
                Jump(true);
                _jumpAvailable = false;
            }
            else if (joystick.y < -Constants.JOYSTICK_JUMP_WEIGHT)
            {
                Jump(false);
                _jumpAvailable = false;
            }
        }
        else if (joystick.y < Constants.JOYSTICK_JUMP_WEIGHT && joystick.y > -Constants.JOYSTICK_JUMP_WEIGHT)
        {
            _jumpAvailable = true;
        }

        // Sprite flip, position
        SetPositionWithFlip(joystick.x);

        // Camera position
        _cameraPos.localPosition = new Vector3(
            Constants.HOR_CAM_OFFSET.x * IsFlipNum,
            Constants.HOR_CAM_OFFSET.y,
            0.0f
        );

        // Animation
        if (_isGroundedMem)
        {
            if (IsGrounded)
            {
                _animator.SetFloat("Velocity", Mathf.Abs(joystick.x));
            }
            else
            {
                _animator.SetBool("IsGrounded", false);
                _isGroundedMem = false;
            }
        }
        else if (IsGrounded)
        {
            _animator.SetBool("IsGrounded", true);
            _isGroundedMem = true;
        }
        #endregion

        if (_paused)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            Attack();
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            Extra();
        }
        else if (_interaction && Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
    }
}
