using UnityEngine;

public class TownManager : MonoBehaviour
{
    [SerializeField] private Joystick _joystick = null;


    private void Start()
    {
        CanvasPlayController.Instance.SetControllerActive(true);
    }


    private void Update()
    {
        _joystick.JoystickUpdate();
    }
}
