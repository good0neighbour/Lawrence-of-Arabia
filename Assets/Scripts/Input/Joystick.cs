using UnityEngine;
using static Constants;

public class Joystick : MonoBehaviour
{
    /* ==================== Fields ==================== */

    [SerializeField] private Transform _handle = null;
    [SerializeField] private float _radius = 150.0f;
    private GameDelegate _joystickStateDel = null;
    private Vector2 _initialPosition = Vector2.zero;
    private Vector2 _initialHandlePosition = Vector2.zero;
    private Vector2 _joystickControl = Vector2.zero;
    private Vector2 _weight = Vector2.zero;
    private sbyte _directionX = 0;
    private sbyte _directionY = 0;
    private byte _joystickState = JOYSTICK_UNAVAILABLE;
    private float _handleDistance = 0.0f;
    private bool _isPressing = false;



    /* ==================== Public Methods ==================== */

    public void OnTouchDown()
    {
        _initialPosition = (Vector2)Input.mousePosition;
        _initialHandlePosition = _handle.localPosition;
        _joystickStateDel = JoystickDrag;
        _joystickState = JOYSTICK_DRAGGING;
        _isPressing = true;
    }


    public void OnTouchUp()
    {
        // Not dragged
        if (_handleDistance < Mathf.Epsilon)
        {
            _handle.localPosition = Vector3.zero;
            _weight = Vector2.zero;
            _joystickStateDel = JoystickStandby;
        }
        // Dragged
        else
        {
            _initialPosition = (Vector2)_handle.localPosition * -1.0f;
            _joystickStateDel = JoystickRelease;
        }

        _joystickState = JOYSTICK_STANDINGBY;
        _isPressing = false;
    }


    /// <summary>
    /// Locks or unlocks keyboard and game pad control
    /// </summary>
    public void SetJoystickActive(bool active)
    {
        if (active)
        {
            _joystickState = JOYSTICK_STANDINGBY;
        }
        else
        {
            _joystickState = JOYSTICK_STANDINGBY;
            SetJoystickDirection(0, 0);
            _joystickState = JOYSTICK_UNAVAILABLE;
        }
    }


    /// <summary>
    /// Functions in Update
    /// </summary>
    /// <param name="isPressing">Bool value if it is being pressing</param>
    /// <returns>Normalized joystick weight</returns>
    public Vector2 JoystickUpdate(out bool isPressing)
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

        isPressing = _isPressing;
        return _weight;
    }



    /* ==================== Private Methods ==================== */

    private void JoystickDrag()
    {
        // User darg
        _weight = (Vector2)Input.mousePosition - _initialPosition + _initialHandlePosition;
        _handleDistance = Mathf.Sqrt(_weight.x * _weight.x + _weight.y * _weight.y);

        // Drag limit
        if (_handleDistance > _radius)
        {
            _weight = _weight / _handleDistance * _radius;
            _handleDistance = _radius;
        }

        // Set handle position;
        _handle.localPosition = _weight;

        // Normalize joystick weight
        _weight /= _radius;
    }


    private void JoystickRelease()
    {
        switch (_joystickState)
        {
            case JOYSTICK_KEYBOARD:
                _joystickStateDel = JoystickStandby;
                JoystickStandby();
                return;

            default:
                // Handle returning to the origin
                _handle.localPosition = new Vector3(
                    _handle.localPosition.x + _initialPosition.x / _handleDistance * JOYSTICK_RELEASING_SPEED * Time.deltaTime,
                    _handle.localPosition.y + _initialPosition.y / _handleDistance * JOYSTICK_RELEASING_SPEED * Time.deltaTime,
                    0.0f
                );

                // Returning end
                if (_initialPosition.x * _handle.localPosition.x > 0.0f || _initialPosition.y * _handle.localPosition.y > 0.0f)
                {
                    _handle.localPosition = Vector3.zero;
                    _joystickStateDel = JoystickStandby;
                }

                // Joystick weight change
                _weight = (Vector2)_handle.localPosition / _radius;
                return;
        }
    }


    private void JoystickStandby()
    {
        switch (_joystickState)
        {
            case JOYSTICK_KEYBOARD:
                // Joystick weight change
                _weight = new Vector3(
                    _weight.x + _initialPosition.x / _handleDistance * JOYSTICK_CONTROL_SPEED * Time.deltaTime,
                    _weight.y + _initialPosition.y / _handleDistance * JOYSTICK_CONTROL_SPEED * Time.deltaTime,
                    0.0f
                );

                // Change end
                if (_initialPosition.x * (_joystickControl.x - _weight.x) < 0.0f || _initialPosition.y * (_joystickControl.y - _weight.y) < 0.0f)
                {
                    _weight = _joystickControl;
                    _joystickState = JOYSTICK_STANDINGBY;
                }

                // Handle Position
                _handle.localPosition = (Vector3)(_weight * _radius);
                return;
        }
    }


    private void SetJoystickDirection(sbyte x, sbyte y)
    {
        switch (_joystickState)
        {
            case JOYSTICK_DRAGGING:
            case JOYSTICK_UNAVAILABLE:
                return;

            default:
                if (x == 0 && y == 0)
                {
                    _handleDistance = Mathf.Sqrt(_weight.x * _weight.x + _weight.y * _weight.y) * _radius;
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

                _initialPosition = _joystickControl - _weight;
                _handleDistance = length;
                _joystickState = JOYSTICK_KEYBOARD;
                _isPressing = true;
                return;
        }
    }


    private void Awake()
    {
        _joystickStateDel = JoystickStandby;
    }
}
