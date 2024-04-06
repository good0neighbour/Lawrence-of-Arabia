using UnityEngine;
using UnityEngine.AI;

public class FourDirectionPlayerControl : MonoBehaviour
{
    /* ==================== Fields ==================== */

    [SerializeField] private Animator _animator = null;
    [SerializeField] private NavMeshAgent _agent = null;
    [SerializeField] private Joystick _joystick = null;
    [SerializeField] private Transform _sprite = null;
    [SerializeField] private Transform _camPos = null;
    private GameDelegate _interaction = null;
    private IInteractableObject _interactableObject = null;
    private Vector2 _interactionPoint = Vector2.zero;
    private float _defaultSpeed = 0.0f;
    private float _defaultAcc = 0.0f;
    private float _defaultRot = 0.0f;
    private bool _joystickControl = false;
    private bool _isReserved = false;
    private bool _isInteractable = false;
    private bool _pause = true;

    public static FourDirectionPlayerControl Instance
    {
        get;
        private set;
    }

    public Transform CameraPos
    {
        get
        {
            return _camPos;
        }
    }



    /* ==================== Public Methods ==================== */

    public void PlayerPause(bool pause)
    {
        _pause = pause;
    }


    public void PlayerSetDestination(float x, float z)
    {
        _agent.speed = _defaultSpeed;
        _agent.acceleration = _defaultAcc;
        _agent.ResetPath();
        _agent.SetDestination(new Vector3(x, 0.0f, z));
    }


    public void Interact()
    {
        _interactionPoint = _interactableObject.GetFixedPosition();
        PlayerSetDestination(_interactionPoint.x, _interactionPoint.y);
        _interaction = _interactableObject.Interaction;
        _isReserved = true;
    }


    public void SetInteractionActive(IInteractableObject target)
    {
        CanvasPlayController.Instance.SetInteractBtnActive(true);
        _interactableObject = target;
        _isInteractable = true;
    }


    public void SetInteractionInactive()
    {
        CanvasPlayController.Instance.SetInteractBtnActive(false);
        _interactableObject = null;
        _isInteractable = false;
    }


    public void DeleteInstance()
    {
        Instance = null;
    }



    /* ==================== Private Methods ==================== */

    private void Awake()
    {
        Instance = this;
        _defaultSpeed = _agent.speed;
        _defaultAcc = _agent.acceleration;
    }


    private void Start()
    {
        _defaultRot = CameraFourDirectionMovement.Instance.transform.eulerAngles.x;
        _sprite.localRotation = Quaternion.Euler(_defaultRot, 0f, 0f);
    }


    private void Update()
    {
        // Animation
        _animator.SetFloat("Velocity", _agent.velocity.magnitude);
        if (_agent.velocity.x > 0.0f)
        {
            _sprite.localRotation = Quaternion.Euler(_defaultRot, 0f, 0f);
        }
        else if (_agent.velocity.x < 0.0f)
        {
            _sprite.localRotation = Quaternion.Euler(-_defaultRot, 180f, 0f);
        }

        // Reserved interaction
        if (_isReserved)
        {
            Vector2 temp = new Vector2(
                _interactionPoint.x - transform.position.x,
                _interactionPoint.y - transform.position.z
            );
            if (temp.x * temp.x + temp.y * temp.y <= Constants.CHAR_INTERACTION_DISTANCE)
            {
                _interaction.Invoke();
                _interaction = null;
                _isReserved = false;
            }
        }

        if (_pause)
        {
            return;
        }

        // Destination
        if (_joystickControl)
        {
            if (_joystick.JoystickDistance > 0.0f)
            {
                if (!_isReserved)
                {
                    // Joystick control
                    Vector2 weight = _joystick.JoystickWeight.normalized;
                    _agent.ResetPath();
                    _agent.SetDestination(new Vector3(
                        weight.x + transform.position.x,
                        0.0f,
                        weight.y + transform.position.z
                    ));
                    _agent.speed = _joystick.JoystickDistance * _defaultSpeed;
                }
            }
            else
            {
                // Stop setting destination
                _joystickControl = false;
                _agent.ResetPath();
            }
        }
        else if (_joystick.JoystickPressing)
        {
            // Start joystick control
            _joystickControl = true;
            _agent.acceleration = 128.0f;
            _interaction = null;
            _isReserved = false;
        }
        else if (Input.GetMouseButtonDown(0))
        {
            // Mouse control
            RaycastHit hit;
            _interaction = null;
            _isReserved = false;
            Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit);
            if (hit.collider.includeLayers == Constants.LAYER_B_WALL)
            {
                // Move
                PlayerSetDestination(hit.point.x, hit.point.z);
            }
            else if (hit.collider.includeLayers == Constants.LAYER_B_PLATFORM)
            {
                // Interact
                _interactableObject = hit.collider.GetComponent<IInteractableObject>();
                Interact();
            }
        }

        // Interaction
        if (_isInteractable && Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
    }
}
