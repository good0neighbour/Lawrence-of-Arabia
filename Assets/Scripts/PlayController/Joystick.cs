using UnityEngine;

public class Joystick : MonoBehaviour
{
    /* ==================== Fields ==================== */

    [SerializeField] private Transform _handle = null;
    [SerializeField] private float _radius = 150.0f;
    private GameDelegate _joystickStateDel = null;
    private Vector2 _initialPosition = Vector2.zero;
    private Vector2 _initialHandlePosition = Vector2.zero;
    private Vector2 _joystickControl = Vector2.zero;
    private sbyte _directionX = 0;
    private sbyte _directionY = 0;
    private byte _joystickState = Constants.JOYSTICK_STANDINGBY;
    private float _handleDistance = 0.0f;

    public Vector2 JoystickWeight
    {
        get;
        private set;
    }

    public float JoystickDistance
    {
        get;
        private set;
    }



    /* ==================== Public Methods ==================== */

    public void OnTouchDown()
    {
        _initialPosition = (Vector2)Input.mousePosition;
        _initialHandlePosition = _handle.localPosition;
        _joystickStateDel = JoystickDrag;
        _joystickState = Constants.JOYSTICK_DRAGGING;
    }


    public void OnTouchUp()
    {
        // Not dragged
        if (_handleDistance < Mathf.Epsilon)
        {
            _handle.localPosition = Vector3.zero;
            JoystickWeight = Vector2.zero;
            _joystickStateDel = JoystickStandby;
        }
        // Dragged
        else
        {
            _initialPosition = (Vector2)_handle.localPosition * -1.0f;
            _joystickStateDel = JoystickRelease;
        }

        _joystickState = Constants.JOYSTICK_STANDINGBY;
    }


    /// <summary>
    /// Locks or unlocks keyboard and game pad control
    /// </summary>
    public void SetJoystickActive(bool active)
    {
        if (active)
        {
            _joystickState = Constants.JOYSTICK_STANDINGBY;
        }
        else
        {
            _joystickState = Constants.JOYSTICK_STANDINGBY;
            SetJoystickDirection(0, 0);
            _joystickState = Constants.JOYSTICK_UNAVAILABLE;
        }
    }


    /// <summary>
    /// Functions in Update
    /// </summary>
    public void JoystickUpdate()
    {
        #region Joystick controlled by Keyboard
        if (Input.GetKeyDown(KeyCode.A))
        {
            --_directionX;
            SetJoystickDirection(_directionX, _directionY);
        }
        else if (Input.GetKeyUp(KeyCode.A))
        {
            ++_directionX;
            SetJoystickDirection(_directionX, _directionY);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            ++_directionX;
            SetJoystickDirection(_directionX, _directionY);
        }
        else if (Input.GetKeyUp(KeyCode.D))
        {
            --_directionX;
            SetJoystickDirection(_directionX, _directionY);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            ++_directionY;
            SetJoystickDirection(_directionX, _directionY);
        }
        else if (Input.GetKeyUp(KeyCode.W))
        {
            --_directionY;
            SetJoystickDirection(_directionX, _directionY);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            --_directionY;
            SetJoystickDirection(_directionX, _directionY);
        }
        else if (Input.GetKeyUp(KeyCode.S))
        {
            ++_directionY;
            SetJoystickDirection(_directionX, _directionY);
        }
        #endregion

        _joystickStateDel.Invoke();
        JoystickDistance = Mathf.Sqrt(JoystickWeight.x * JoystickWeight.x + JoystickWeight.y * JoystickWeight.y);
    }



    /* ==================== Private Methods ==================== */

    private void JoystickDrag()
    {
        // User darg
        JoystickWeight = (Vector2)Input.mousePosition - _initialPosition + _initialHandlePosition;
        _handleDistance = Mathf.Sqrt(JoystickWeight.x * JoystickWeight.x + JoystickWeight.y * JoystickWeight.y);

        // Drag limit
        if (_handleDistance > _radius)
        {
            JoystickWeight = JoystickWeight / _handleDistance * _radius;
            _handleDistance = _radius;
        }

        // Set handle position;
        _handle.localPosition = JoystickWeight;

        // Normalize joystick weight
        JoystickWeight /= _radius;
    }


    private void JoystickRelease()
    {
        switch (_joystickState)
        {
            case Constants.JOYSTICK_KEYBOARD:
                _joystickStateDel = JoystickStandby;
                JoystickStandby();
                return;

            default:
                // Handle returning to the origin
                _handle.localPosition = new Vector3(
                    _handle.localPosition.x + _initialPosition.x / _handleDistance * Constants.JOYSTICK_RELEASING_SPEED * Time.deltaTime,
                    _handle.localPosition.y + _initialPosition.y / _handleDistance * Constants.JOYSTICK_RELEASING_SPEED * Time.deltaTime,
                    0.0f
                );

                // Returning end
                if (_initialPosition.x * _handle.localPosition.x > 0.0f || _initialPosition.y * _handle.localPosition.y > 0.0f)
                {
                    _handle.localPosition = Vector3.zero;
                    _joystickStateDel = JoystickStandby;
                }

                // Joystick weight change
                JoystickWeight = (Vector2)_handle.localPosition / _radius;
                return;
        }
    }


    private void JoystickStandby()
    {
        switch (_joystickState)
        {
            case Constants.JOYSTICK_KEYBOARD:
                // Joystick weight change
                JoystickWeight = new Vector3(
                    JoystickWeight.x + _initialPosition.x / _handleDistance * Constants.JOYSTICK_CONTROL_SPEED * Time.deltaTime,
                    JoystickWeight.y + _initialPosition.y / _handleDistance * Constants.JOYSTICK_CONTROL_SPEED * Time.deltaTime,
                    0.0f
                );

                // Change end
                if (_initialPosition.x * (_joystickControl.x - JoystickWeight.x) < 0.0f || _initialPosition.y * (_joystickControl.y - JoystickWeight.y) < 0.0f)
                {
                    JoystickWeight = _joystickControl;
                    _joystickState = Constants.JOYSTICK_STANDINGBY;
                }

                // Handle Position
                _handle.localPosition = (Vector3)(JoystickWeight * _radius);
                return;
        }
    }


    private void SetJoystickDirection(sbyte x, sbyte y)
    {
        switch (_joystickState)
        {
            case Constants.JOYSTICK_DRAGGING:
            case Constants.JOYSTICK_UNAVAILABLE:
                return;

            default:
                if (x == 0 && y == 0)
                {
                    _handleDistance = Mathf.Sqrt(JoystickWeight.x * JoystickWeight.x + JoystickWeight.y * JoystickWeight.y) * _radius;
                    OnTouchUp();
                    return;
                }

                float length = x * x + y * y;
                _joystickControl = new Vector2(x, y);
                if (length > 1)
                {
                    length = Mathf.Sqrt(length);
                    _joystickControl /= length;
                }

                _initialPosition = _joystickControl - JoystickWeight;
                _handleDistance = length;
                _joystickState = Constants.JOYSTICK_KEYBOARD;
                return;
        }
    }


    private void Awake()
    {
        _joystickStateDel = JoystickStandby;
    }
}
