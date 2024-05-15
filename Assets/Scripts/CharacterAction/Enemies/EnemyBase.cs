using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using static Constants;

public abstract class EnemyBase : HorizontalMovement, IHit
{
    /* ==================== Fields ==================== */

    [Header("Movement")]
    [SerializeField] private float _moveSpeed = 0.1f;
    [SerializeField] private float _acceleration = 0.05f;
    [Header("Sight")]
    [Tooltip("It squares when game starts.")]
    [SerializeField] private float _silenceSightRange = 2.0f;
    [Tooltip("Degree. It becomes half radian when game starts.")]
    [SerializeField] private float _silenceSightAngle = 45.0f;
    [Tooltip("It squares when game starts.")]
    [SerializeField] private float _urgentSightRange = 4.0f;
    [SerializeField] private float _sightHight = 0.6f;
    [Header("Defence")]
    [SerializeField] private int _health = 10;
    [SerializeField] private ushort _armor = 1;
    [Header("Attack")]
    [Tooltip("It squares when game starts.")]
    [SerializeField] private float _attackRange = 2.0f;
    [SerializeField] private float _attackTimer = 1.0f;
    [SerializeField] protected byte AttackDamage = 1;
    [Tooltip("Degree. It becomes half radian when game starts.")]
    [SerializeField] private float _attackAngle = 90.0f;
    [Header("Misc")]
    [Tooltip("Reports to other enemies when it sees player.")]
    [SerializeField] private bool _sendUrgentReport = true;
    [Tooltip("Receives report from other enemies.")]
    [SerializeField] private bool _receiveUrgentReport = true;
    [Tooltip("It pushes when player approches in this range. It squares when game starts.")]
    [SerializeField] private float _pushingRange = 0.5f;
    [SerializeField] private GameObject _hitEffect = null;
    [Header("References")]
    [SerializeField] private SpriteRenderer _sprite = null;
    [SerializeField] private RectTransform _sightUI = null;
    [SerializeField] private Image _notice = null;
    protected Transform Player = null;
    private GameDelegate _behavDel = null;
    private BehaviourTree _behav = new BehaviourTree();
    private Transform _cam = null;
    private Vector2 _playerDir = Vector2.zero;
    private byte _enemyState = ENEMY_SILENCE;
    private float _playerDis = 0.0f;
    private float _velocity = 0.0f;
    private float _urgentMeter = 0.0f;
    private float _knockback = 0.0f;
    private float _suspiciousTimer = 0.0f;
    private float _timer = 0.0f;

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
            switch (_enemyState)
            {
                case ENEMY_SILENCE:
                case ENEMY_SUSPICIOUS:
                    _sightUI.localRotation = Quaternion.Euler(0.0f, -90.0f * (-1.0f + IsFlipNum), 0.0f);
                    break;
            }
        }
    }



    /* ==================== Public Methods ==================== */

    public void Hit(ushort damage, sbyte direction)
    {
        int deal = damage - _armor;
        if (deal > 0)
        {
            // Deal damage
            _health = _health - deal;

            // Hit effect
            Transform hitEft = StageManagerBase.ObjectPool.GetObject(_hitEffect);
            hitEft.position = new Vector3(
                transform.position.x,
                transform.position.y + PLAYER_RADIUS,
                0.0f
            );
            hitEft.rotation = Quaternion.Euler(0.0f, 0.0f, 90.0f * direction);

            // Death
            if (_health <= 0)
            {
                StageManagerBase.Instance.EnemyDeathReport(this);
                Destroy(gameObject);
                return;
            }
        }

        // KnockBack
        if (direction >= 0.0f)
        {
            _knockback = PLAYER_KNOCKBACK_AMOUNT;
        }
        else
        {
            _knockback = -PLAYER_KNOCKBACK_AMOUNT;
        }

        // Suspicious start
        if (_enemyState == ENEMY_SILENCE)
        {
            foreach (Collider2D enemy in Physics2D.OverlapCircleAll((Vector2)transform.position, ENEMY_SUSPICIOUS_RANGE, LAYER_B_ENEMY))
            {
                enemy.GetComponent<EnemyBase>().SuspiciousStart();
            }
            SuspiciousStart();
        }
    }


    public void SetEnemyPause(bool pause)
    {
        if (pause)
        {
            Paused = true;
            Animator.SetFloat("Velocity", 0.0f);
        }
        else
        {
            Paused = false;
        }
    }


    public void UrgentStart()
    {
        StateChange(ENEMY_URGENT);
        if (_sightUI != null)
        {
            Destroy(_sightUI.gameObject);
        }
        _acceleration *= ENEMY_URGENT_ACC_MULT;
    }


    public void ReceiveUrgentAlert()
    {
        if (_receiveUrgentReport)
        {
            UrgentStart();
        }
    }


    public void SuspiciousStart()
    {
        StateChange(ENEMY_SUSPICIOUS);
        _sightUI.gameObject.SetActive(false);
        _acceleration *= ENEMY_URGENT_ACC_MULT;
    }



    /* ==================== Protected Methods ==================== */

    protected override void Awake()
    {
        base.Awake();

        // Precalculation
        _silenceSightRange += PLAYER_RADIUS;
        _silenceSightRange = _silenceSightRange * _silenceSightRange;
        _urgentSightRange = _urgentSightRange * _urgentSightRange;
        _attackRange = _attackRange * _attackRange;
        _pushingRange = _pushingRange * _pushingRange;
        _silenceSightAngle = _silenceSightAngle / 360.0f * Mathf.PI;
        _attackAngle = _attackAngle / 360.0f * Mathf.PI;

        // Behaviour Tree
        #region BehaviourTree
        _behav.Selector()
            .Action(GetOutOfMyFace)
            .Sequence() // Silence
                .Action(() =>
                {
                    switch (_enemyState)
                    {
                        case ENEMY_SILENCE:
                            return SUCCESS;
                        default:
                            return FAILURE;
                    }
                })
                .Action(RandomMoving)
                .SubSequence(SUCCESS)
                    .Action(DetectPlayer)
                    .Action(UrgentAlert)
                    .Back()
                .Back()
            .SubSequence(FAILURE) // Suspicious
                .Action(() =>
                {
                    switch (_enemyState)
                    {
                        case ENEMY_SUSPICIOUS:
                            return SUCCESS;
                        default:
                            return FAILURE;
                    }
                })
                .Action(() =>
                {
                    _suspiciousTimer += Time.deltaTime;
                    if (_suspiciousTimer >= ENEMY_SUSPICIOUS_TIME)
                    {
                        StateChange(ENEMY_SILENCE);
                        _sightUI.gameObject.SetActive(true);
                        _acceleration /= ENEMY_URGENT_ACC_MULT;
                    }
                    return SUCCESS;
                })
                .Back()
            .Sequence() // Attack
                .Action(() =>
                {
                    if (_playerDis <= _attackRange)
                        switch (_enemyState)
                        {
                            case ENEMY_SUSPICIOUS:
                                StateChange(ENEMY_ATTACK);
                                UrgentAlert();
                                return SUCCESS;
                            case ENEMY_ATTACK:
                                return SUCCESS;
                            default:
                                StateChange(ENEMY_ATTACK);
                                return SUCCESS;
                        }
                    else
                        switch (_enemyState)
                        {
                            case ENEMY_ATTACK:
                                StateChange(ENEMY_URGENT);
                                return FAILURE;
                            default:
                                return FAILURE;
                        }
                })
                .Action(LookAtPlayer)
                .Action(DetectPlayer)
                .SubSequence(SUCCESS)
                    .Action(() =>
                    {
                        _timer += Time.deltaTime;
                        if (_timer >= _attackTimer)
                        {
                            _timer -= _attackTimer;
                            return SUCCESS;
                        }
                        return FAILURE;
                    })
                    .Action(Attack)
                    .Back()
                .Back()
            .Sequence() // Player in Sight
                .Action(() =>
                {
                    if (_playerDis <= _urgentSightRange)
                        return SUCCESS;
                    else
                        return FAILURE;
                })
                .Action(Searching)
                .Back()
            .Action(RandomMoving) // Urgent
        .End();
        #endregion
    }


    protected override void Start()
    {
        base.Start();

        // Player Transform
        Player = HorizontalPlayerControl.Instance.transform;

        // Camera Transform
        _cam = CameraHorizontalMovement.Instance.transform;

        // ObjectPool Prepare
        StageManagerBase.ObjectPool.PoolPreparing(_hitEffect);
    }


    protected override void Update()
    {
        // Active check
        if (Paused)
        {
            return;
        }

        // Distance check
        Vector2 dis = (Vector2)_cam.position - (Vector2)transform.position;
        if (dis.x * dis.x + dis.y * dis.y > ENEMY_ACTIVE_DIS_SQR)
        {
            return;
        }

        base.Update();

        // Enemy behaviour
        _behav.Execute();
    }


    private void FixedUpdate()
    {
        // Active check
        if (Paused)
        {
            return;
        }

        // Move direction deligate
        _behavDel?.Invoke();

        // Position, Flip
        SetPositionWithFlip(_velocity);

        // KnockBack
        switch (_knockback)
        {
            case 0.0f:
                break;

            default:
                transform.localPosition = new Vector3(
                    transform.localPosition.x + _knockback * Time.fixedDeltaTime,
                    transform.localPosition.y,
                    0.0f
                );
                if (_knockback > 0.0f)
                {
                    _knockback -= PLAYER_KNOCKBACK_ACC * Time.fixedDeltaTime;
                    if (_knockback < 0.0f)
                    {
                        _knockback = 0.0f;
                    }
                }
                else
                {
                    _knockback += PLAYER_KNOCKBACK_ACC * Time.fixedDeltaTime;
                    if (_knockback > 0.0f)
                    {
                        _knockback = 0.0f;
                    }
                }
                break;
        }
    }



    /* ==================== Abstract Methods ==================== */

    protected abstract byte Attack();



    /* ==================== Private Methods ==================== */

    private byte GetOutOfMyFace()
    {
        // Player position
        _playerDir.x = Player.position.x - transform.position.x;
        _playerDir.y = Player.position.y - transform.position.y;
        _playerDis = _playerDir.x * _playerDir.x + _playerDir.y * _playerDir.y;

        // Pushing
        if (_playerDis < _pushingRange)
        {
            if (_playerDir.x > 0.0f)
            {
                transform.localPosition = new Vector3(
                    transform.localPosition.x + (_playerDis - _pushingRange) * Time.deltaTime,
                    transform.localPosition.y,
                    0.0f
                );
            }
            else if (_playerDir.x < 0.0f)
            {
                transform.localPosition = new Vector3(
                    transform.localPosition.x + (_pushingRange - _playerDis) * Time.deltaTime,
                    transform.localPosition.y,
                    0.0f
                );
            }
        }

        return FAILURE;
    }


    private byte RandomMoving()
    {
        // Moving Direction
        if (_timer >= ENEMY_SILENCE_TIMER && _behavDel == null)
        {
            float ran = Random.Range(0.0f, 1.0f);
            if (_velocity == 0.0f)
            {
                if (ran < ENEMY_MOVE_POSI)
                {
                    _behavDel = MoveLeft;
                }
                else if (ran < ENEMY_MOVE_POSI_DOUBLE)
                {
                    _behavDel = MoveRight;
                }
                else if (IsGrounded)
                {
                    if (ran < ENEMY_JUMP_UP_POSI)
                    {
                        if (Physics2D.OverlapCircle(
                            new Vector2(
                                transform.position.x,
                                transform.position.y + 1.0f
                            ), 0.1f, LAYER_B_PLATFORM))
                        {
                            Jump(true);
                        }
                    }
                    else if (ran < ENEMY_JUMP_DOWN_POSI)
                    {
                        if (Physics2D.OverlapCircle(
                            new Vector2(
                                transform.position.x,
                                transform.position.y - 1.0f
                            ), 0.1f, LAYER_B_TERRAIN))
                        {
                            Jump(false);
                        }
                    }
                }
            }
            else if (_velocity > 0.0f)
            {
                if (ran < ENEMY_MOVESTOP_POSI)
                {
                    _behavDel = StopMovingRight;
                }
            }
            else if (_velocity < 0.0f)
            {
                if (ran < ENEMY_MOVESTOP_POSI)
                {
                    _behavDel = StopMovingLeft;
                }
            }
            _timer -= ENEMY_SILENCE_TIMER;
        }
        else
        {
            _timer += Time.deltaTime;
        }

        //Return
        return SUCCESS;
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
                    case LAYER_D_GROUND:
                    case LAYER_D_WALL:
                        return FAILURE;

                    case LAYER_D_PLAYER:
                        return SUCCESS;

                    default:
                        break;
                }
            }
        }

        return FAILURE;
    }


    private byte DetectPlayer()
    {
        switch (_enemyState)
        {
            case ENEMY_SILENCE:
                if (_playerDis < _silenceSightRange)
                {
                    switch (DetectPlayer(_silenceSightAngle, _silenceSightRange))
                    {
                        case SUCCESS:
                            UrgentMeterChange(Time.deltaTime * ENEMY_URGENT_SPEED);
                            if (_urgentMeter >= 1.0f)
                            {
                                return SUCCESS;
                            }
                            else
                            {
                                return FAILURE;
                            }

                        case FAILURE:
                            if (_urgentMeter > 0.0f)
                            {
                                UrgentMeterChange(Time.deltaTime * -ENEMY_URGENT_SPEED);
                            }
                            return FAILURE;
                    }
                }
                else if (_urgentMeter > 0.0f)
                {
                    UrgentMeterChange(Time.deltaTime * -ENEMY_URGENT_SPEED);
                }
                return FAILURE;

            case ENEMY_ATTACK:
                return DetectPlayer(_attackAngle, _attackRange);

            default:
                return FAILURE;
        }
    }


    private byte Searching()
    {
        // Moving Direction
        if (_timer >= ENEMY_URGENT_TIMER && _behavDel == null)
        {
            float ran = Random.Range(0.0f, 1.0f);
            if (_velocity == 0.0f)
            {
                if (ran < ENEMY_URGENT_MOVE_POSI)
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
                else if (IsGrounded && ran < ENEMY_URGENT_JUMP_POSI_ACTUAL)
                {
                    if (_playerDir.y > 0.0f)
                    {
                        if (Physics2D.OverlapCircle(
                        new Vector2(
                            transform.position.x,
                            transform.position.y + 1.0f
                        ), 0.1f, LAYER_B_PLATFORM))
                        {
                            Jump(true);
                        }
                    }
                    else if (_playerDir.y < 0.0f)
                    {
                        if (Physics2D.OverlapCircle(
                        new Vector2(
                            transform.position.x,
                            transform.position.y - 1.5f
                        ), 0.6f, LAYER_B_TERRAIN))
                        {
                            Jump(false);
                        }
                    }
                }
            }
            else if (_velocity > 0.0f)
            {
                if (ran < ENEMY_URGENT_MOVESTOP_POSI || _playerDir.x < 0.0f)
                {
                    _behavDel = StopMovingRight;
                }
            }
            else if (_velocity < 0.0f)
            {
                if (ran < ENEMY_URGENT_MOVESTOP_POSI || _playerDir.x > 0.0f)
                {
                    _behavDel = StopMovingLeft;
                }
            }
            _timer -= ENEMY_URGENT_TIMER;
        }
        else
        {
            _timer += Time.deltaTime;
        }

        // Return
        return SUCCESS;
    }


    private byte UrgentAlert()
    {
        if (_sendUrgentReport)
        {
            StageManagerBase.Instance.UrgentAlert();
        }
        else
        {
            UrgentStart();
        }
        return SUCCESS;
    }


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


    private byte LookAtPlayer()
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

        return SUCCESS;
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
        _urgentMeter += value;
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
}
