using System.Collections.Generic;
using UnityEngine;

public class HorizontalPlayerControl : HorizontalMovement, IHit
{
    /* ==================== Fields ==================== */

    [SerializeField] private SpriteRenderer _sprite = null;
    [SerializeField] private Joystick _joystick = null;
    [SerializeField] private Animator _animator = null;
    [SerializeField] private Transform _cameraPos = null;
    [SerializeField] private GameObject _atkEftPrefab = null;
    [SerializeField] private ParticleSystem _charChangeEft = null;
    private List<GameDelegate> _onInteract = new List<GameDelegate>();
    private CurrentCharacter[] _characters = null;
    private byte _charIndex = byte.MaxValue;
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

    public void Hit(byte damage, float direction)
    {
        // Deal damage
        byte deal = (byte)(damage - _characters[_charIndex].Armor);
        if (deal > 0)
        {
            _characters[_charIndex].Health = (sbyte)(_characters[_charIndex].Health - deal);
            CanvasPlayController.Instance.SetCharacterHealthGage(
                _charIndex,
                (float)_characters[_charIndex].Health / _characters[_charIndex].MaxHealth
            );
        }

        // Death
        if (_characters[_charIndex].Health <= 0)
        {
            CanvasPlayController.Instance.SetCharacterHealthGage(_charIndex, 0.0f);
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
            Transform eft = StageManagerBase.ObjectPool.GetObject(_atkEftPrefab);
            eft.position = new Vector3(
                transform.position.x + Constants.CHAR_ATKEFT_POS.x * IsFlipNum,
                transform.position.y + Constants.CHAR_ATKEFT_POS.y,
                0.0f
            );

            //eft.GetComponent<AttackEffect>().StartEffect(new Vector2(IsFlipNum, 0.0f), _damage);
        }
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
            CanvasPlayController.Instance.SetInteractBtnActive(true);
            _interaction = true;
            _onInteract.Add(action);
        }
        else
        {
            _onInteract.Remove(action);
            if (_onInteract.Count == 0)
            {
                CanvasPlayController.Instance.SetInteractBtnActive(false);
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
        CharacterData.Character[] data = GameManager.Instance.GetCharacterList();

        // Set character local data
        _characters = new CurrentCharacter[characters.Length];
        for (byte i = 0; i < characters.Length; ++i)
        {
            _characters[i].MaxHealth = data[(int)characters[i]].CurHealth;
            _characters[i].Health = data[(int)characters[i]].CurHealth;
            _characters[i].Armor = data[(int)characters[i]].CurArmor;
            _characters[i].Damage = data[(int)characters[i]].CurDamage;
            _characters[i].Sprite = data[(int)characters[i]].Sprite;
        }

        // Set default character
        CharacterChange(0);
    }


    public void CharacterChange(byte index)
    {
        // Not change
        if (_charIndex == index)
        {
            return;
        }

        // Change character
        _sprite.sprite = _characters[index].Sprite;

        // Current character index
        _charIndex = index;

        // Successfully changed character
        CanvasPlayController.Instance.CharacterChange(index);

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

    private void Start()
    {
        // ObjectPool Prepare
        if (_atkEftPrefab != null)
        {
            StageManagerBase.ObjectPool.PoolPreparing(_atkEftPrefab);
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

        // Not functioning while game is paused
        if (_paused)
        {
            return;
        }

        // Character actions
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
    }



    /* ==================== Struct ==================== */

    private struct CurrentCharacter
    {
        public int MaxHealth;
        public int Health;
        public ushort Armor;
        public ushort Damage;
        public Sprite Sprite;
    }
}
