using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class FourDirectionPlayerControl : MonoBehaviour
{
    /* ==================== Fields ==================== */

    [SerializeField] private Transform _character = null;
    [SerializeField] private Joystick _joystick = null;
    private FourDirectionMovement _movement = null;
    private Animator _animator = null;
    private Transform _camera = null;



    /* ==================== Private Methods ==================== */

    private void Awake()
    {
        _movement = new FourDirectionMovement(GetComponent<Rigidbody>());
        _animator = GetComponent<Animator>();
    }


    private void Start()
    {
        _camera = Camera.main.transform;
        _camera.localRotation = Quaternion.Euler(
            Constants.FD_CAM_ROT_OFFSET - Mathf.Atan(Constants.FD_CAM_OFFSET.x / Constants.FD_CAM_OFFSET.y) / Mathf.PI * 180.0f,
            0.0f,
            0.0f
        );
    }


    private void Update()
    {
        // Animation
        _animator.SetFloat("Velocity", _joystick.JoystickDistance);
    }


    private void FixedUpdate()
    {
        // Rotation of direction
        Vector2 joystick = _joystick.JoystickWeight;
        _character.localRotation = Quaternion.Euler(0.0f, _movement.SetPosition(joystick.x, joystick.y), 0.0f);

        // Camera position
        _camera.position = Vector3.Lerp(
            _camera.position,
            transform.position + new Vector3(0.0f, Constants.FD_CAM_OFFSET.x, Constants.FD_CAM_OFFSET.y),
            Constants.FD_CAM_SPEED
        );
    }


    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
    }
}
