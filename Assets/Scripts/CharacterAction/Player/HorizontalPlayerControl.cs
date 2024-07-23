using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

public class HorizontalPlayerControl : HorizontalMovement, IHit
{
    /* ==================== Fields ==================== */

    [SerializeField] private SpriteRenderer _sprite = null;
    [SerializeField] private Transform _cameraPos = null;
    [SerializeField] private ParticleSystem _charChangeEft = null;
    [SerializeField] private GameObject _bloodEftPrefab = null;
    //[SerializeField] private AudioSource _audioSource = null;
    //[SerializeField] private AudioClip[] _playerHitSounds = null;
    private List<GameDelegate> _onInteract = new List<GameDelegate>();
    private PlayerWeaponBase[] _weapons = new PlayerWeaponBase[(int)CharacterWeapons.None];
    private CurrentCharacter[] _characters = null;
    private byte _curChar = byte.MaxValue;
    private float _knockback = 0.0f;
    private float _immuneTimer = 0.0f;
    private float _attackTimer = 0.0f;
    private bool _interaction = false;
    private bool _immune = false;
    private bool _jumpAvailable = true;

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
            
            // Sprite flip
            _sprite.flipX = value;

            // Camera position
            _cameraPos.localPosition = new Vector3(
                HOR_CAM_OFFSET.x * IsFlipNum,
                HOR_CAM_OFFSET.y,
                0.0f
            );
        }
    }



    /* ==================== Public Methods ==================== */

    public void Hit(ushort damage, sbyte direction)
    {
        // Hit sound
        //if (!_audioSource.isPlaying)
        //{
        //    _audioSource.clip = _playerHitSounds[Random.Range(0, _playerHitSounds.Length)];
        //    _audioSource.Play();
        //}

        // Immune
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

                // Sprite change
                _sprite.sprite = _characters[_curChar].SpriteDead;

                // KnockBack
                if (direction >= 0.0f)
                {
                    _knockback = PLAYER_DEATH_KNOCKBACK_AMOUNT;
                }
                else
                {
                    _knockback = -PLAYER_DEATH_KNOCKBACK_AMOUNT;
                }

                // Camera effect
                CameraHorizontalMovement.Instance.SetTargetSize(HOR_CAM_DEATH_SIZE);
                CameraHorizontalMovement.Instance.TargetChange(_sprite.transform);

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
            IsFlipNum
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
        Paused = pause;
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


    public void SetCharacters(Characters[] characters, CharacterWeapons[] weapons)
    {
        // Get character list
        CharacterData.Character[] data = GameManager.CharacterData.GetCharacterList();

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
            _characters[i].SpriteDead = data[(int)characters[i]].SpriteDead;
            _characters[i].Type = data[(int)characters[i]].Type;
            _characters[i].Weapon = weapons[i];

            // Weapon set
            if (weapons[i] != CharacterWeapons.None)
            {
                SetWeapon(weapons[i]);
            }
        }

        // Set default character
        CharacterChange(0);
        if (_characters[0].Weapon != CharacterWeapons.None)
        {
            _attackTimer = _weapons[(int)_characters[0].Weapon].AttackTime;
        }
    }


    public void CharacterChange(byte index)
    {
        // Not change
        if (_curChar == index || _characters[index].Health <= 0)
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
            // Button unavailable
            CanvasPlayController.Instance.SetBtnActive(BUTTON_ATTACK, false);
        }
        else
        {
            // Set weapon
            _weapons[(int)_characters[_curChar].Weapon].WeaponSet(
                _characters[_curChar].Damage,
                _characters[_curChar].Range
            );

            // Button available
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

        // Singleton pattern
        Instance = this;

        // Camera position
        _cameraPos.localPosition = new Vector3(
            HOR_CAM_OFFSET.x * IsFlipNum,
            HOR_CAM_OFFSET.y,
            0.0f
        );
    }


    protected override void Update()
    {
        base.Update();

        #region Always Functioning
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
        #endregion

        // Not functioning while game is paused
        if (Paused)
        {
            return;
        }

        #region Input
        // Character actions
        if (UserControl.GetKey(TouchInput.F) && _characters[_curChar].Weapon != CharacterWeapons.None)
        {
            Attack();
        }
        else if (UserControl.GetKeyUp(TouchInput.F) && _characters[_curChar].Weapon != CharacterWeapons.None)
        {
            AttackKeyUp();
        }
        else if (UserControl.GetKeyDown(TouchInput.Q))
        {
            Extra();
        }
        else if (_interaction && UserControl.GetKeyDown(TouchInput.E))
        {
            Interact();
        }

        // Switch character
        else if (UserControl.GetKeyDown(TouchInput.Num1)
            && _characters[0].Health > 0)
        {
            CharacterChange(0);
        }
        else if (UserControl.GetKeyDown(TouchInput.Num2)
            && _characters.Length > 1
            && _characters[1].Health > 0)
        {
            CharacterChange(1);
        }
        else if (UserControl.GetKeyDown(TouchInput.Num3)
            && _characters.Length > 2
            && _characters[2].Health > 0)
        {
            CharacterChange(2);
        }

        // Camera zoom in, out
        else if (UserControl.GetKeyDown(TouchInput.Z))
        {
            CameraHorizontalMovement.Instance.SetTargetSize(HOR_CAM_ZOOMOUT_SIZE);
        }
        else if (UserControl.GetKeyUp(TouchInput.Z))
        {
            CameraHorizontalMovement.Instance.SetTargetSize(HOR_CAM_DEFAULT_SIZE);
        }
        else if (UserControl.GetKeyDown(TouchInput.X))
        {
            CameraHorizontalMovement.Instance.SetTargetSize(HOR_CAM_ZOOMIN_SIZE);
        }
        else if (UserControl.GetKeyUp(TouchInput.X))
        {
            CameraHorizontalMovement.Instance.SetTargetSize(HOR_CAM_DEFAULT_SIZE);
        }
        #endregion

        // Jump
        float yWeight = UserControl.JoystickWeight.y;
        if (_jumpAvailable)
        {
            if (JOYSTICK_JUMP_WEIGHT <= yWeight)
            {
                Jump(true);
                _jumpAvailable = false;
            }
            else if (-JOYSTICK_JUMP_WEIGHT >= yWeight)
            {
                Jump(false);
                _jumpAvailable = false;
            }
        }
        else if (JOYSTICK_JUMP_WEIGHT > yWeight && -JOYSTICK_JUMP_WEIGHT < yWeight)
        {
            _jumpAvailable = true;
        }

        // Attack timer
        _attackTimer += Time.fixedDeltaTime;

        // Immune timer
        if (_immune)
        {
            _immuneTimer += Time.fixedDeltaTime;
            if (_immuneTimer >= PLAYER_IMMUNE_TIME)
            {
                _immune = false;
            }
        }
    }



    /* ==================== Private Methods ==================== */

    private void SetWeapon(CharacterWeapons type)
    {
        // Doesn't need to double construct
        if (_weapons[(int)type] != null)
        {
            return;
        }

        // Weapon set
        switch (type)
        {
            case CharacterWeapons.Pistol:
                _weapons[(int)CharacterWeapons.Pistol] = new WeaponPistol();
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

        // Character switch
        CharacterChange(index);
        StageManagerBase.Instance.PauseGame(false);

        // Camera effect
        CameraHorizontalMovement.Instance.SetTargetSize(HOR_CAM_DEFAULT_SIZE);
        CameraHorizontalMovement.Instance.TargetChange(_cameraPos);
    }


    private void Start()
    {
        // ObjectPool Prepare
        StageManagerBase.ObjectPool.PoolPreparing(_bloodEftPrefab);
    }


    private void FixedUpdate()
    {
        // Sprite flip, position
        SetPositionWithFlip(UserControl.JoystickWeight.x);
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
        public Sprite SpriteDead;
        public CharacterTypes Type;
        public CharacterWeapons Weapon;
    }
}
