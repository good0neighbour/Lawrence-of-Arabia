using UnityEngine;

public class CanvasPlayController : MonoBehaviour
{
    /* ==================== Fields ==================== */

    [SerializeField] private Joystick _joystick = null;
    [SerializeField] private GameObject[] _buttons = null;
    private GameDelegate _onInteract = null;

    public static CanvasPlayController Instance
    {
        get;
        private set;
    }



    /* ==================== Public Methods ==================== */

    public void SetControllerActive(bool active)
    {
        gameObject.SetActive(active);
        _joystick.SetJoystickActive(active);
    }


    public void ButtonClick(int index)
    {
        switch (index)
        {
            case Constants.BUTTON_ATTACK:
                Debug.Log("Player Attack");
                return;

            case Constants.BUTTON_INTERACT:
                _onInteract.Invoke();
                return;

            case Constants.BUTTON_EXTRA:
                Debug.Log("Extra Function");
                return;
        }
    }


    public void SetInteractBtnActive(GameDelegate action)
    {
        _buttons[Constants.BUTTON_INTERACT].SetActive(true);
        _onInteract = action;
    }


    public void SetInteractBtnInactive()
    {
        _buttons[Constants.BUTTON_INTERACT].SetActive(false);
        _onInteract = null;
    }



    /* ==================== Private Methods ==================== */

    private void Awake()
    {
        Instance = this;
    }


    private void Start()
    {
        gameObject.SetActive(false);
    }
}
