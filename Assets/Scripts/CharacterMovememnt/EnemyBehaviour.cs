using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class EnemyBehaviour : HorizontalMovement
{
    /* ==================== Fields ==================== */

    [Header("Enemy Settings")]
    [Tooltip("It squares when game starts.")]
    [SerializeField] private float _silenceSight = 3.0f;
    [Tooltip("Degree. It becomes half radian when game starts.")]
    [SerializeField] private float _silenceSightAngle = 45.0f;
    [Tooltip("It squares when game starts.")]
    [SerializeField] private float _urgentSight = 7.0f;
    [Tooltip("It squares when game starts.")]
    [SerializeField] private float _attackRange = 3.0f;
    [SerializeField] private float _attackTimer = 1.0f;
    [Tooltip("Degree. It becomes half radian when game starts.")]
    [SerializeField] private float _attackAngle = 15.0f;
    [SerializeField] private float _moveSpeed = 0.1f;
    [SerializeField] private float _acceleration = 0.01f;
    [SerializeField] private float _sightHight = 0.6f;
    [Tooltip("It pushes when player approches in this range. It squares when game starts.")]
    [SerializeField] private float _pushingRange = 0.5f;
    [Header("References")]
    [SerializeField] private SpriteRenderer _sprite = null;
    [SerializeField] private RectTransform _sightUI = null;
    [SerializeField] private Image _notice = null;
    [SerializeField] private Animator _animator = null;
    private GameDelegate _behavDel = null;
    private BehaviourTree _behav = new BehaviourTree();
    private Transform _player = null;
    private Vector2 _playerDir = Vector2.zero;
    private byte _enemyState = Constants.ENEMY_SILENCE;
    private float _playerDis = 0.0f;
    private float _velocity = 0.0f;
    private float _urgentMeter = 0.0f;
    private float _timer = 0.0f;
    private bool _isGroundedMem = true;
    private bool _paused = true;



    /* ==================== Public Methods ==================== */

    public void SetEnemyPause(bool pause)
    {
        if (pause)
        {
            _paused = true;
            _animator.SetFloat("Velocity", 0.0f);
        }
        else
        {
            _paused = false;
        }
    }


    public void UrgentStart()
    {
        StateChange(Constants.ENEMY_URGENT);
        Destroy(_sightUI.gameObject);
        _acceleration *= Constants.ENEMY_URGENT_ACC_MULT;
    }



    /* ==================== Protected Methods ==================== */

    protected override void Awake()
    {
        base.Awake();

        // Precalculation
        _silenceSight = _silenceSight * _silenceSight;
        _urgentSight = _urgentSight * _urgentSight;
        _attackRange = _attackRange * _attackRange;
        _pushingRange = _pushingRange * _pushingRange;
        _silenceSightAngle = _silenceSightAngle / 360.0f * Mathf.PI;
        _attackAngle = _attackAngle / 360.0f * Mathf.PI;

        // Player Transform
        _player = FindAnyObjectByType<HorizontalPlayerControl>().GetComponent<Transform>();

        // Behaviour Tree
        #region BehaviourTree
        _behav.Selector()
            .Action(GetOutOfMyFace)
            .Sequence() // Silence
                .Action(() =>
                {
                    switch (_enemyState)
                    {
                        case Constants.ENEMY_SILENCE:
                            return Constants.SUCCESS;
                        default:
                            return Constants.FAILURE;
                    }
                })
                .Action(RandomMoving)
                .SubSequence(Constants.SUCCESS)
                    .Action(DetectPlayer)
                    .Action(UrgentAlert)
                    .Back()
                .Back()
            .Sequence() // Urgent
                .Action(() =>
                {
                    if (_playerDis > _urgentSight)
                        switch (_enemyState)
                        {
                            case Constants.ENEMY_ATTACK:
                                StateChange(Constants.ENEMY_URGENT);
                                return Constants.SUCCESS;
                            default:
                                return Constants.SUCCESS;
                        }
                    else
                        return Constants.FAILURE;
                })
                .Action(RandomMoving)
                .Back()
            .Sequence() // Player in Sight
                .Action(() =>
                {
                    if (_playerDis > _attackRange)
                        switch (_enemyState)
                        {
                            case Constants.ENEMY_ATTACK:
                                StateChange(Constants.ENEMY_URGENT);
                                return Constants.SUCCESS;
                            default:
                                return Constants.SUCCESS;
                        }
                    else
                        switch (_enemyState)
                        {
                            case Constants.ENEMY_ATTACK:
                                return Constants.FAILURE;
                            default:
                                StateChange(Constants.ENEMY_ATTACK);
                                return Constants.FAILURE;
                        }
                })
                .Action(Searching)
                .Back()
            .SubSequence(Constants.SUCCESS) // Attack
                .Action(() =>
                {
                    LookAtPlayer();
                    _timer += DeltaTime;
                    if (_timer >= _attackTimer)
                    {
                        _timer -= _attackTimer;
                        return Constants.SUCCESS;
                    }
                    return Constants.FAILURE;
                })
                .Action(DetectPlayer)
                .Action(Attack)
        .End();
        #endregion
    }



    /* ==================== Private Methods ==================== */

    private byte GetOutOfMyFace()
    {
        // Player position
        _playerDir.x = _player.position.x - transform.position.x;
        _playerDir.y = _player.position.y - transform.position.y;
        _playerDis = _playerDir.x * _playerDir.x + _playerDir.y * _playerDir.y;

        // Pushing
        if (_playerDis < _pushingRange)
        {
            if (_playerDir.x > 0.0f)
            {
                transform.localPosition = new Vector3(
                    transform.localPosition.x + (_playerDis - _pushingRange) * DeltaTime,
                    transform.localPosition.y,
                    0.0f
                );
            }
            else if (_playerDir.x < 0.0f)
            {
                transform.localPosition = new Vector3(
                    transform.localPosition.x + (_pushingRange - _playerDis) * DeltaTime,
                    transform.localPosition.y,
                    0.0f
                );
            }
        }

        return Constants.FAILURE;
    }


    private byte RandomMoving()
    {
        // Moving Direction
        if (_timer >= Constants.ENEMY_SILENCE_TIMER && _behavDel == null)
        {
            float ran = Random.Range(0.0f, 1.0f);
            if (_velocity == 0.0f)
            {
                if (ran < Constants.ENEMY_MOVE_POSI)
                {
                    _behavDel = MoveLeft;
                }
                else if (ran < Constants.ENEMY_MOVE_POSI_DOUBLE)
                {
                    _behavDel = MoveRight;
                }
                else if (IsGrounded)
                {
                    if (ran < Constants.ENEMY_JUMP_UP_POSI)
                    {
                        if (Physics2D.OverlapCircle(
                            new Vector2(
                                transform.position.x,
                                transform.position.y + 1.0f
                            ), 0.1f, Constants.LAYER_B_GROUND))
                        {
                            Jump(true);
                        }
                    }
                    else if (ran < Constants.ENEMY_JUMP_DOWN_POSI)
                    {
                        if (Physics2D.OverlapCircle(
                            new Vector2(
                                transform.position.x,
                                transform.position.y - 1.0f
                            ), 0.1f, Constants.LAYER_B_GROUND))
                        {
                            Jump(false);
                        }
                    }
                }
            }
            else if (_velocity > 0.0f)
            {
                if (ran < Constants.ENEMY_MOVESTOP_POSI)
                {
                    _behavDel = StopMovingRight;
                }
            }
            else if (_velocity < 0.0f)
            {
                if (ran < Constants.ENEMY_MOVESTOP_POSI)
                {
                    _behavDel = StopMovingLeft;
                }
            }
            _timer -= Constants.ENEMY_SILENCE_TIMER;
        }
        else
        {
            _timer += DeltaTime;
        }

        // Position, Flip
        _sprite.flipX = SetPositionWithFlip(_velocity);
        switch (_enemyState)
        {
            case Constants.ENEMY_SILENCE:
                _sightUI.localRotation = Quaternion.Euler(0.0f, -90.0f * (-1.0f + IsFlipNum), 0.0f);
                break;
        }

        //Return
        return Constants.SUCCESS;
    }


    private byte DetectPlayer(float angle, float distance)
    {
        float atan = Mathf.Atan(_playerDir.y / _playerDir.x);

        if (_playerDir.x * IsFlipNum > 0.0f && atan < angle && atan > -angle)
        {
            RaycastHit2D[] hits = Physics2D.RaycastAll(
                new Vector2(transform.position.x, transform.position.y + _sightHight),
                _playerDir,
                Mathf.Sqrt(distance)
            );
            foreach (RaycastHit2D hit in hits)
            {
                switch (hit.collider.gameObject.layer)
                {
                    case Constants.LAYER_D_GROUND:
                    case Constants.LAYER_D_WALL:
                        return Constants.FAILURE;

                    case Constants.LAYER_D_PLAYER:
                        return Constants.SUCCESS;

                    default:
                        break;
                }
            }
        }

        return Constants.FAILURE;
    }


    private byte DetectPlayer()
    {
        switch (_enemyState)
        {
            case Constants.ENEMY_SILENCE:
                if (_playerDis < _silenceSight)
                {
                    switch (DetectPlayer(_silenceSightAngle, _silenceSight))
                    {
                        case Constants.SUCCESS:
                            UrgentMeterChange(_urgentMeter + DeltaTime * Constants.ENEMY_URGENT_SPEED);
                            if (_urgentMeter >= 1.0f)
                            {
                                return Constants.SUCCESS;
                            }
                            else
                            {
                                return Constants.FAILURE;
                            }

                        case Constants.FAILURE:
                            if (_urgentMeter > 0.0f)
                            {
                                UrgentMeterChange(_urgentMeter - DeltaTime * Constants.ENEMY_URGENT_SPEED);
                            }
                            return Constants.FAILURE;
                    }
                }
                if (_urgentMeter > 0.0f)
                {
                    UrgentMeterChange(_urgentMeter - DeltaTime * Constants.ENEMY_URGENT_SPEED);
                }
                return Constants.FAILURE;

            case Constants.ENEMY_ATTACK:
                return DetectPlayer(_attackAngle, _attackRange);

            default:
                return Constants.FAILURE;
        }
    }


    private byte Searching()
    {
        // Moving Direction
        if (_timer >= Constants.ENEMY_URGENT_TIMER && _behavDel == null)
        {
            float ran = Random.Range(0.0f, 1.0f);
            if (_velocity == 0.0f)
            {
                if (ran < Constants.ENEMY_URGENT_MOVE_POSI)
                {
                    if (_playerDir.x > 0.0f)
                    {
                        _behavDel = MoveRight;
                    }
                    else if (_playerDir.x < 0.0f)
                    {
                        _behavDel = MoveLeft;
                    }
                }
                else if (IsGrounded && ran < Constants.ENEMY_URGENT_JUMP_POSI_ACTUAL)
                {
                    if (_playerDir.y > 0.0f)
                    {
                        if (Physics2D.OverlapCircle(
                        new Vector2(
                            transform.position.x,
                            transform.position.y + 1.0f
                        ), 0.1f, Constants.LAYER_B_GROUND))
                        {
                            Jump(true);
                        }
                    }
                    else if (_playerDir.y < 0.0f)
                    {
                        if (Physics2D.OverlapCircle(
                        new Vector2(
                            transform.position.x,
                            transform.position.y - 1.0f
                        ), 0.1f, Constants.LAYER_B_GROUND))
                        {
                            Jump(false);
                        }
                    }
                }
            }
            else if (_velocity > 0.0f)
            {
                if (ran < Constants.ENEMY_URGENT_MOVESTOP_POSI || _playerDir.x < 0.0f)
                {
                    _behavDel = StopMovingRight;
                }
            }
            else if (_velocity < 0.0f)
            {
                if (ran < Constants.ENEMY_URGENT_MOVESTOP_POSI || _playerDir.x > 0.0f)
                {
                    _behavDel = StopMovingLeft;
                }
            }
            _timer -= Constants.ENEMY_URGENT_TIMER;
        }
        else
        {
            _timer += DeltaTime;
        }

        // Look at Player
        LookAtPlayer();

        // Return
        return Constants.SUCCESS;
    }


    private byte UrgentAlert()
    {
        MapManager.Instance.UrgentAlert();
        return Constants.SUCCESS;
    }


    private byte Attack()
    {
        Debug.Log("Attack");
        return Constants.SUCCESS;
    }


    private void MoveLeft()
    {
        _velocity -= DeltaTime * _acceleration;
        if (_velocity <= -_moveSpeed)
        {
            _velocity = -_moveSpeed;
            _behavDel = null;
        }
    }


    private void StopMovingLeft()
    {
        _velocity += DeltaTime * _acceleration;
        if (_velocity >= 0.0f)
        {
            _velocity = 0.0f;
            _behavDel = null;
        }
    }


    private void MoveRight()
    {
        _velocity += DeltaTime * _acceleration;
        if (_velocity >= _moveSpeed)
        {
            _velocity = _moveSpeed;
            _behavDel = null;
        }
    }


    private void StopMovingRight()
    {
        _velocity -= DeltaTime * _acceleration;
        if (_velocity <= 0.0f)
        {
            _velocity = 0.0f;
            _behavDel = null;
        }
    }


    private void LookAtPlayer()
    {
        if (_playerDir.x > 0.0f)
        {
            _sprite.flipX = false;
            IsFlipNum = 1;
        }
        else
        {
            _sprite.flipX = true;
            IsFlipNum = -1;
        }

        SetPosition(_velocity);
    }


    private void StateChange(byte state)
    {
        _behavDel = null;
        if (_velocity > 0.0f)
        {
            _behavDel = StopMovingRight;
        }
        else if (_velocity < 0.0f)
        {
            _behavDel = StopMovingLeft;
        }
        _enemyState = state;
        _timer = 0.0f;
    }


    private void UrgentMeterChange(float value)
    {
        _urgentMeter = value;
        if (_urgentMeter < 0.0f)
        {
            _urgentMeter = 0.0f;
        }
        else if (_urgentMeter > 1.0f)
        {
            _urgentMeter = 1.0f;
        }
        _notice.fillAmount = _urgentMeter;
    }


    private void Update()
    {
        // Active check
        if (_paused)
        {
            return;
        }

        // Enemy behaviour
        _behav.Execute();
        _behavDel?.Invoke();

        // Animation
        if (_isGroundedMem)
        {
            if (IsGrounded)
            {
                _animator.SetFloat("Velocity", Mathf.Abs(_velocity * Constants.ENEMY_ANIM_MULT));
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
    }
}
