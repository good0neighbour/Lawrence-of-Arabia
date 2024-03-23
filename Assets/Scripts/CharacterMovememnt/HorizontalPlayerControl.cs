using UnityEngine;

[RequireComponent (typeof(Animator))]
public class HorizontalPlayerControl : HorizontalMovement
{
    /* ==================== Fields ==================== */

    [SerializeField] private SpriteRenderer _sprite = null;
    [SerializeField] private Joystick _joystick = null;
    [SerializeField] private Animator _animator = null;
    [SerializeField] private Transform _camera = null;
    private bool _jumpAvailable = true;
    private bool _isGroundedMem = true;



    /* ==================== Private Methods ==================== */

    private void Update()
    {
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
        _sprite.flipX = SetPositionWithFlip(joystick.x);

        // Camera position
        _camera.position = Vector3.Lerp(
            _camera.position,
            transform.position + new Vector3(
                Constants.HOR_CAM_OFFSET.x * IsFlipNum,
                Constants.HOR_CAM_OFFSET.y,
                0.0f
            ),
            Constants.HOR_CAM_SPEED
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
    }
}
