using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static Constants;

public class FourDirectionPlayerControl : MonoBehaviour
{
    /* ==================== Fields ==================== */

    [SerializeField] private Animator _animator = null;
    [SerializeField] private NavMeshAgent _agent = null;
    [SerializeField] private Transform _sprite = null;
    [SerializeField] private Transform _camPos = null;
    private GameDelegate _interaction = null;
    private List<InteractableObjectBase> _interactableObject = new List<InteractableObjectBase>();
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
        _interactionPoint = _interactableObject[_interactableObject.Count - 1].GetInteractionPos();
        PlayerSetDestination(_interactionPoint.x, _interactionPoint.y);
        _interaction = _interactableObject[_interactableObject.Count - 1].Interaction;
        _isReserved = true;
    }


    public void Interact(InteractableObjectBase target)
    {
        _interactionPoint = target.GetInteractionPos();
        PlayerSetDestination(_interactionPoint.x, _interactionPoint.y);
        _interaction = target.Interaction;
        _isReserved = true;
    }


    public void SetInteractionActive(InteractableObjectBase target, bool active)
    {
        if (active)
        {
            CanvasPlayController.Instance.SetBtnActive(BUTTON_INTERACT, true);
            _interactableObject.Add(target);
            _isInteractable = true;
        }
        else
        {
            _interactableObject.Remove(target);
            if (_interactableObject.Count == 0)
            {
                CanvasPlayController.Instance.SetBtnActive(BUTTON_INTERACT, false);
                _isInteractable = false;
            }
        }
    }


    public void PlayerLookAt(Transform target)
    {
        _agent.velocity = Vector3.zero;
        if (target.position.x >= transform.position.x)
        {
            _sprite.localRotation = Quaternion.Euler(_defaultRot, 0f, 0f);
        }
        else
        {
            _sprite.localRotation = Quaternion.Euler(-_defaultRot, 180f, 0f);
        }
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
            if (temp.x * temp.x + temp.y * temp.y <= PLAYER_INTERACTION_DISTANCE)
            {
                _agent.ResetPath();
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
            if (UserControl.JoystickDistance > 0.0f)
            {
                if (!_isReserved)
                {
                    // Joystick control
                    Vector2 weight = UserControl.JoystickWeight.normalized;
                    _agent.ResetPath();
                    _agent.SetDestination(new Vector3(
                        weight.x + transform.position.x,
                        0.0f,
                        weight.y + transform.position.z
                    ));
                    _agent.speed = UserControl.JoystickDistance * _defaultSpeed;
                }
            }
            else
            {
                // Stop setting destination
                _joystickControl = false;
                _agent.ResetPath();
            }
        }
        else if (UserControl.JoystickPressing)
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
            if (hit.collider.includeLayers == LAYER_B_WALL)
            {
                // Move
                PlayerSetDestination(hit.point.x, hit.point.z);
            }
            else if (hit.collider.includeLayers == LAYER_B_PLATFORM)
            {
                // Interact
                Interact(hit.collider.GetComponent<InteractableObjectBase>());
            }
        }

        // Interaction
        if (_isInteractable && Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
    }
}
