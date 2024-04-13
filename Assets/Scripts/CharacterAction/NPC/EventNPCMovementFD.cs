using UnityEngine;
using UnityEngine.AI;
using static Constants;

public class EventNPCMovementFD : MonoBehaviour
{
    /* ==================== Fields ==================== */

    [SerializeField] private Animator _animator = null;
    [SerializeField] private NavMeshAgent _agent = null;
    [SerializeField] private Transform _sprite = null;
    private float _defaultRot = 0.0f;
    private bool _isMoving = false;



    /* ==================== Public Methods ==================== */

    public void SetGoal(Vector3 destination)
    {
        _agent.SetDestination(destination);
        _isMoving = true;
    }


    public void NPCLookAt(Transform target)
    {
        _agent.velocity = Vector3.zero;
        if (target.position.x >= transform.position.x)
        {
            _sprite.localRotation = Quaternion.Euler(_defaultRot, 0f, 0f);
        }
        else
        {
            _sprite.localRotation = Quaternion.Euler(-_defaultRot, 180.0f, 0f);
        }
    }


#if UNITY_EDITOR
    public void Flip(bool flip)
    {
        if (flip)
        {
            _sprite.localRotation = Quaternion.Euler(-_sprite.localEulerAngles.x, 180.0f, 0f);
        }
        else
        {
            _sprite.localRotation = Quaternion.Euler(-_sprite.localEulerAngles.x, 0f, 0f);
        }
    }


    public bool Flip()
    {
        if (_sprite.localEulerAngles.y == 180.0f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
#endif



    /* ==================== Private Methods ==================== */

    private void Start()
    {
        _defaultRot = CameraFourDirectionMovement.Instance.transform.eulerAngles.x;
        if (_sprite.localEulerAngles.y == 0.0f)
        {
            _sprite.localRotation = Quaternion.Euler(
                _defaultRot,
                0.0f,
                0.0f
            );
        }
        else
        {
            _sprite.localRotation = Quaternion.Euler(
                -_defaultRot,
                180.0f,
                0.0f
            );
        }
        
    }


    private void Update()
    {
        // Animation
        _animator.SetFloat("Velocity", Mathf.Abs(_agent.velocity.magnitude * ENEMY_ANIM_MULT));
        if (_agent.velocity.x > 0.0f)
        {
            _sprite.localRotation = Quaternion.Euler(_defaultRot, 0f, 0f);
        }
        else if (_agent.velocity.x < 0.0f)
        {
            _sprite.localRotation = Quaternion.Euler(-_defaultRot, 180f, 0f);
        }

        if (_isMoving && _agent.hasPath)
        {
            Vector2 temp = new Vector2(_agent.destination.x - transform.position.x, _agent.destination.z - transform.position.z);
            if (temp.x * temp.x + temp.y * temp.y < PLAYER_INTERACTION_DISTANCE)
            {
                _agent.ResetPath();
                _isMoving = false;
            }
        }
    }
}
