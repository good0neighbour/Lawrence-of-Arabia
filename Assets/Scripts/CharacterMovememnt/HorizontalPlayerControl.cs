using UnityEngine;

public class HorizontalPlayerControl : HorizontalMovement, IHit
{
    /* ==================== Fields ==================== */

    [SerializeField] private SpriteRenderer _sprite = null;
    [SerializeField] private Joystick _joystick = null;
    [SerializeField] private Animator _animator = null;
    [SerializeField] private Transform _cameraPos = null;
    [SerializeField] private GameObject _atkEftPrefab = null;
    [SerializeField] private sbyte _health = 10;
    [SerializeField] private byte _armor = 1;
    [SerializeField] private byte _damage = 4;
    private GameDelegate _onInteract = null;
    private float _knockback = 0.0f;
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

    public void Hit(byte damage, float direction)
    {
        // Deal damage
        byte deal = (byte)(damage - _armor);
        if (deal > 0)
        {
            _health = (sbyte)(_health - deal);
        }

        // Death
        if (_health <= 0)
        {
            Debug.Log("Player low health");
            return;
        }

        // KnockBack
        if (direction >= 0.0f)
        {
            _knockback = Constants.CHAR_KNOCKBACK_AMOUNT;
        }
        else
        {
            _knockback = -Constants.CHAR_KNOCKBACK_AMOUNT;
        }
    }


    public void Attack()
    {
        if (_atkEftPrefab != null)
        {
            Transform eft = MapManager.ObjectPool.GetObject(_atkEftPrefab);
            eft.position = new Vector3(
                transform.position.x + Constants.CHAR_ATKEFT_POS.x * IsFlipNum,
                transform.position.y + Constants.CHAR_ATKEFT_POS.y,
                0.0f
            );

            eft.GetComponent<AttackEffect>().StartEffect(new Vector2(IsFlipNum, 0.0f), _damage);
        }
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

    private void Start()
    {
        // ObjectPool Prepare
        if (_atkEftPrefab != null)
        {
            MapManager.ObjectPool.PoolPreparing(_atkEftPrefab);
        }
    }


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

        // KnockBack
        switch (_knockback)
        {
            case 0.0f:
                break;

            default:
                transform.localPosition = new Vector3(
                    transform.localPosition.x + _knockback * DeltaTime,
                    transform.localPosition.y,
                    0.0f
                );
                if (_knockback > 0.0f)
                {
                    _knockback -= Constants.CHAR_KNOCKBACK_ACC * DeltaTime;
                    if (_knockback < 0.0f)
                    {
                        _knockback = 0.0f;
                    }
                }
                else
                {
                    _knockback += Constants.CHAR_KNOCKBACK_ACC * DeltaTime;
                    if (_knockback > 0.0f)
                    {
                        _knockback = 0.0f;
                    }
                }
                break;
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
