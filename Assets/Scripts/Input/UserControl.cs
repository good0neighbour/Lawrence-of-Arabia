using UnityEngine;
using static Constants;

public class UserControl : MonoBehaviour
{
    /* ==================== Fields ==================== */

    private static bool _joystickPressing = false;
    private byte[] _touchStatus = new byte[(int)TouchInput.End];
    private Joystick _joystick = null;
    private bool _paused = true;
    private bool _isMobileUIActivated = true;

    public static UserControl Instance
    {
        get;
        private set;
    }

    public static Vector2 JoystickWeight
    {
        get;
        private set;
    }

    /// <summary>
    /// Normalized distance
    /// </summary>
    public static float JoystickDistance
    {
        get;
        private set;
    }


    public static bool JoystickPressing
    {
        get
        {
            return _joystickPressing;
        }
    }



    /* ==================== Public Methods ==================== */

    /// <summary>
    /// UI touch down.
    /// </summary>
    public void TouchDown(byte key)
    {
        _touchStatus[key] = TOUCH_DOWN;
    }


    /// <summary>
    /// UI touch up.
    /// </summary>
    public void TouchUp(byte key)
    {
        _touchStatus[key] = TOUCH_UP;
    }


    public void SetUserControlActive(bool active)
    {
        _paused = !active;
        _joystick.SetJoystickActive(active);
    }


    public static bool GetKey(TouchInput key)
    {
        if (Instance._paused)
        {
            return false;
        }

        // Touch check
        switch (Instance._touchStatus[(int)key])
        {
            case TOUCH_PRESSING:
                return true;
        }

        // Keyboard input check
        switch (key)
        {
            case TouchInput.F:
                if (Input.GetKey(KeyCode.F))
                {
                    return true;
                }
                return false;

            case TouchInput.E:
                if (Input.GetKey(KeyCode.E))
                {
                    return true;
                }
                return false;

            case TouchInput.Q:
                if (Input.GetKey(KeyCode.Q))
                {
                    return true;
                }
                return false;

            case TouchInput.Z:
                if (Input.GetKey(KeyCode.Z))
                {
                    return true;
                }
                return false;

            case TouchInput.X:
                if (Input.GetKey(KeyCode.X))
                {
                    return true;
                }
                return false;

            case TouchInput.Num1:
                if (Input.GetKey(KeyCode.Alpha1))
                {
                    return true;
                }
                return false;

            case TouchInput.Num2:
                if (Input.GetKey(KeyCode.Alpha2))
                {
                    return true;
                }
                return false;

            case TouchInput.Num3:
                if (Input.GetKey(KeyCode.Alpha3))
                {
                    return true;
                }
                return false;

            default:
                return false;
        }
    }


    public static bool GetKeyDown(TouchInput key)
    {
        if (Instance._paused)
        {
            return false;
        }

        // Touch check
        switch (Instance._touchStatus[(int)key])
        {
            case TOUCH_DOWN:
                return true;
        }

        // Keyboard input check
        switch (key)
        {
            case TouchInput.F:
                if (Input.GetKeyDown(KeyCode.F))
                {
                    return true;
                }
                return false;

            case TouchInput.E:
                if (Input.GetKeyDown(KeyCode.E))
                {
                    return true;
                }
                return false;

            case TouchInput.Q:
                if (Input.GetKeyDown(KeyCode.Q))
                {
                    return true;
                }
                return false;

            case TouchInput.Z:
                if (Input.GetKeyDown(KeyCode.Z))
                {
                    return true;
                }
                return false;

            case TouchInput.X:
                if (Input.GetKeyDown(KeyCode.X))
                {
                    return true;
                }
                return false;

            case TouchInput.Num1:
                if (Input.GetKeyDown(KeyCode.Alpha1))
                {
                    return true;
                }
                return false;

            case TouchInput.Num2:
                if (Input.GetKeyDown(KeyCode.Alpha2))
                {
                    return true;
                }
                return false;

            case TouchInput.Num3:
                if (Input.GetKeyDown(KeyCode.Alpha3))
                {
                    return true;
                }
                return false;

            default:
                return false;
        }
    }


    public static bool GetKeyUp(TouchInput key)
    {
        if (Instance._paused)
        {
            return false;
        }

        // Touch check
        switch (Instance._touchStatus[(int)key])
        {
            case TOUCH_UP:
                return true;
        }

        // Keyboard input check
        switch (key)
        {
            case TouchInput.F:
                if (Input.GetKeyUp(KeyCode.F))
                {
                    return true;
                }
                return false;

            case TouchInput.E:
                if (Input.GetKeyUp(KeyCode.E))
                {
                    return true;
                }
                return false;

            case TouchInput.Q:
                if (Input.GetKeyUp(KeyCode.Q))
                {
                    return true;
                }
                return false;

            case TouchInput.Z:
                if (Input.GetKeyUp(KeyCode.Z))
                {
                    return true;
                }
                return false;

            case TouchInput.X:
                if (Input.GetKeyUp(KeyCode.X))
                {
                    return true;
                }
                return false;

            case TouchInput.Num1:
                if (Input.GetKeyUp(KeyCode.Alpha1))
                {
                    return true;
                }
                return false;

            case TouchInput.Num2:
                if (Input.GetKeyUp(KeyCode.Alpha2))
                {
                    return true;
                }
                return false;

            case TouchInput.Num3:
                if (Input.GetKeyUp(KeyCode.Alpha3))
                {
                    return true;
                }
                return false;

            default:
                return false;
        }
    }



    /* ==================== Private Methods ==================== */

    private void Awake()
    {
        // Singleton pattern
        Instance = this;

        // Reference
        _joystick = transform.Find("MobileUI").Find("Joystick").GetComponent<Joystick>();
    }


    private void Update()
    {
        // Joystick update
        JoystickWeight = _joystick.JoystickUpdate(out _joystickPressing);

        // Normalized joystick distance
        JoystickDistance = Mathf.Sqrt(JoystickWeight.x * JoystickWeight.x + JoystickWeight.y * JoystickWeight.y);

        if (_paused)
        {
            return;
        }

        // Touch Check
        if (_isMobileUIActivated)
        {
            for (byte i = 0; i < (byte)TouchInput.End; ++i)
            {
                switch (_touchStatus[i])
                {
                    case TOUCH_NONE:
                    case TOUCH_PRESSING:
                        break;

                    case TOUCH_DOWN:
                        _touchStatus[i] = TOUCH_PRESSING;
                        break;

                    case TOUCH_UP:
                        _touchStatus[i] = TOUCH_NONE;
                        break;
                }
            }
        }
    }
}
