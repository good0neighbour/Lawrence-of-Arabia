using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

public class HorizontalPlayerControl : HorizontalMovement, IHit
{
    /* ==================== Fields ==================== */

    [SerializeField] private SpriteRenderer _sprite = null;
    [SerializeField] private Joystick _joystick = null;
    [SerializeField] private Animator _animator = null;
    [SerializeField] private Transform _cameraPos = null;
    [SerializeField] private ParticleSystem _charChangeEft = null;
    [SerializeField] private GameObject _bloodEftPrefab = null;
    [SerializeField] private Sprite _graveSprite = null;
    private List<GameDelegate> _onInteract = new List<GameDelegate>();
    private PlayerWeaponBase[] _weapons = new PlayerWeaponBase[(int)CharacterWeapons.None];
    private CurrentCharacter[] _characters = null;
    private byte _curChar = byte.MaxValue;
    private float _knockback = 0.0f;
    private float _immuneTimer = 0.0f;
    private float _attackTimer = 0.0f;
    private bool _jumpAvailable = true;
    private bool _isGroundedMem = true;
    private bool _paused = true;
    private bool _interaction = false;
    private bool _immune = false;

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

    public void Hit(ushort damage, sbyte direction)
    {
        if (_immune)
        {
            return;
        }

        int deal = damage - _characters[_curChar].Armor;
        if (deal > 0)
        {
            // Deal damage
            _characters[_curChar].Health = _characters[_curChar].Health - deal;
            CanvasPlayController.Instance.SetCharacterHealthGage(
                _curChar,
                (float)_characters[_curChar].Health / _characters[_curChar].MaxHealth
            );

            // Blood effect
            Transform bloodTrans = StageManagerBase.ObjectPool.GetObject(_bloodEftPrefab);
            bloodTrans.position = new Vector3(
                transform.position.x,
                transform.position.y + PLAYER_RADIUS,
                0.0f
            );
            bloodTrans.rotation = Quaternion.Euler(0.0f, 0.0f, 90.0f * direction);

            // Death
            if (_characters[_curChar].Health <= 0)
            {
                // Death notice
                CanvasPlayController.Instance.SetCharacterHealthGage(_curChar, 0.0f);

                // Death standby
                StageManagerBase.Instance.PauseGame(true);
                StartCoroutine(DeathStandBy());
                _immune = true;
                _immuneTimer = 0.0f;

                // Animator
                _animator.SetBool("Dead", true);
                _sprite.sprite = _graveSprite;

                // Ends here
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
    }


    public void Attack()
    {
        if (_attackTimer < _weapons[(int)_characters[_curChar].Weapon].AttackTime)
        {
            return;
        }

        _weapons[(int)_characters[_curChar].Weapon].Attack(
            new Vector2(
                transform.position.x + PLAYER_ATKEFT_POS.x * IsFlipNum,
                transform.position.y + PLAYER_ATKEFT_POS.y
            ),
            IsFlipNum,
            _characters[_curChar].Range,
            _characters[_curChar].Damage
        );

        _attackTimer = 0.0f;
    }


    public void AttackKeyUp()
    {
        _weapons[(int)_characters[_curChar].Weapon].StopAttack();
    }


    public void Interact()
    {
        _onInteract[_onInteract.Count - 1].Invoke();
    }


    public void Extra()
    {
        Debug.Log("Extra Function");
    }


    public void PausePlayerControl(bool pause)
    {
        _paused = pause;
    }


    public void SetInteractBtnActive(GameDelegate action, bool active)
    {
        if (active)
        {
            CanvasPlayController.Instance.SetBtnActive(BUTTON_INTERACT, true);
            _interaction = true;
            _onInteract.Add(action);
        }
        else
        {
            _onInteract.Remove(action);
            if (_onInteract.Count == 0)
            {
                CanvasPlayController.Instance.SetBtnActive(BUTTON_INTERACT, false);
                _interaction = false;
            }
        }
    }


    public void PlayerLookAt(Transform target)
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


    public void SetCharacters(Characters[] characters)
    {
        // Get character list
        CharacterData.Character[] data = GameManager.Instance.CharacterData.GetCharacterList();

        _characters = new CurrentCharacter[characters.Length];
        for (byte i = 0; i < characters.Length; ++i)
        {
            // Set character local data
            _characters[i].MaxHealth = data[(int)characters[i]].CurHealth;
            _characters[i].Health = data[(int)characters[i]].CurHealth;
            _characters[i].Armor = data[(int)characters[i]].CurArmor;
            _characters[i].Damage = data[(int)characters[i]].CurDamage;
            _characters[i].Range = data[(int)characters[i]].CurRange / 100.0f;
            _characters[i].Sprite = data[(int)characters[i]].Sprite;
            _characters[i].Type = data[(int)characters[i]].Type;
            _characters[i].Weapon = data[(int)characters[i]].Weapon;

            // Weapon set
            if (_characters[i].Weapon != CharacterWeapons.None)
            {
                SetWeapon(i);
            }
        }

        // Set default character
        CharacterChange(0);
        if (_characters[_curChar].Weapon != CharacterWeapons.None)
        {
            _attackTimer = _weapons[(int)_characters[_curChar].Weapon].AttackTime;
        }
    }


    public void CharacterChange(byte index)
    {
        // Not change
        if (_curChar == index)
        {
            return;
        }

        // Change character
        _sprite.sprite = _characters[index].Sprite;

        // Current character index
        _curChar = index;

        // Successfully changed character
        CanvasPlayController.Instance.CharacterChange(index);
        if (_characters[_curChar].Weapon == CharacterWeapons.None)
        {
            CanvasPlayController.Instance.SetBtnActive(BUTTON_ATTACK, false);
        }
        else
        {
            CanvasPlayController.Instance.SetBtnActive(BUTTON_ATTACK, true);
        }

        // Effect play
        _charChangeEft.Play();
    }


    public void DeleteInstance()
    {
        Instance = null;
    }



    /* ==================== Protected Methods ==================== */

    protected override void Awake()
    {
        base.Awake();
        Instance = this;
    }



    /* ==================== Private Methods ==================== */

    private void SetWeapon(byte index)
    {
        // Doesn't need to double construct
        if (_weapons[(int)_characters[index].Weapon] != null)
        {
            return;
        }

        // Weapon set
        switch (_characters[index].Weapon)
        {
            case CharacterWeapons.Pistol:
                _weapons[(int)_characters[index].Weapon] = new WeaponPistol();
                break;

            case CharacterWeapons.Rifle:
                break;

            case CharacterWeapons.AutomaticRifle:
                break;

            case CharacterWeapons.Shotgun:
                break;

            case CharacterWeapons.SniperRifle:
                break;

            case CharacterWeapons.Machinegun:
                break;

            case CharacterWeapons.None:
                break;
        }
    }


    private IEnumerator DeathStandBy()
    {
        // Stand by
        float timer = 0.0f;
        byte index = byte.MaxValue;

        // Check current situation
        if (_characters[_curChar].Type != CharacterTypes.Essential)
        {
            for (byte i = 0; i < _characters.Length; ++i)
            {
                if (_characters[i].Health > 0)
                {
                    index = i;
                }
            }

            // All characters killed in action
            if (index == byte.MaxValue)
            {
                StageMessage.Instance.EnqueueMessage("All characters has been killed in action.");
                StageManagerBase.Instance.GameFailed();
                yield break;
            }
        }

        // Essential character killed in action
        if (index == byte.MaxValue)
        {
            StageMessage.Instance.EnqueueMessage("Essential character has been killed in action.");
            StageManagerBase.Instance.GameFailed();
            yield break;
        }

        // Character switch stand by
        while (timer < PLAYER_DEATH_STANDBY_TIME)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        // Animator
        _animator.SetBool("Dead", false);

        // Character switch
        CharacterChange(index);
        StageManagerBase.Instance.PauseGame(false);
    }


    private void Start()
    {
        // ObjectPool Prepare
        StageManagerBase.ObjectPool.PoolPreparing(_bloodEftPrefab);
    }


    private void Update()
    {
        #region Always Functioning
        Vector2 joystick = _joystick.JoystickWeight;

        // Jump
        if (_jumpAvailable)
        {
            if (joystick.y > JOYSTICK_JUMP_WEIGHT)
            {
                Jump(true);
                _jumpAvailable = false;
            }
            else if (joystick.y < -JOYSTICK_JUMP_WEIGHT)
            {
                Jump(false);
                _jumpAvailable = false;
            }
        }
        else if (joystick.y < JOYSTICK_JUMP_WEIGHT && joystick.y > -JOYSTICK_JUMP_WEIGHT)
        {
            _jumpAvailable = true;
        }

        // Sprite flip, position
        SetPositionWithFlip(joystick.x);

        // Camera position
        _cameraPos.localPosition = new Vector3(
            HOR_CAM_OFFSET.x * IsFlipNum,
            HOR_CAM_OFFSET.y,
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
                    _knockback -= PLAYER_KNOCKBACK_ACC * DeltaTime;
                    if (_knockback < 0.0f)
                    {
                        _knockback = 0.0f;
                    }
                }
                else
                {
                    _knockback += PLAYER_KNOCKBACK_ACC * DeltaTime;
                    if (_knockback > 0.0f)
                    {
                        _knockback = 0.0f;
                    }
                }
                break;
        }
        #endregion

        // Not functioning while game is paused
        if (_paused)
        {
            return;
        }

        #region Input
        // Character actions
        if (Input.GetKey(KeyCode.F) && _characters[_curChar].Weapon != CharacterWeapons.None)
        {
            Attack();
        }
        else if (Input.GetKeyUp(KeyCode.F) && _characters[_curChar].Weapon != CharacterWeapons.None)
        {
            AttackKeyUp();
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            Extra();
        }
        else if (_interaction && Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }

        // Switch character
        else if (Input.GetKeyDown(KeyCode.Alpha1)
            && _characters[0].Health > 0)
        {
            CharacterChange(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2)
            && _characters.Length > 1
            && _characters[1].Health > 0)
        {
            CharacterChange(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3)
            && _characters.Length > 2
            && _characters[2].Health > 0)
        {
            CharacterChange(2);
        }
        #endregion

        // Attack timer
        _attackTimer += DeltaTime;

        // Immune timer
        if (_immune)
        {
            _immuneTimer += DeltaTime;
            if (_immuneTimer >= PLAYER_IMMUNE_TIME)
            {
                _immune = false;
            }
        }
    }



    /* ==================== Struct ==================== */

    private struct CurrentCharacter
    {
        public int MaxHealth;
        public int Health;
        public ushort Armor;
        public ushort Damage;
        public float Range;
        public Sprite Sprite;
        public CharacterTypes Type;
        public CharacterWeapons Weapon;
    }
}
