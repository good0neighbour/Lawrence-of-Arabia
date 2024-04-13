using UnityEngine;
using static Constants;

public abstract class InteractableObjectBase : TriggerBase
{
    /* ==================== Fields ==================== */

    [Header("Player Control")]
    [Tooltip("It squares when game starts.")]
    [SerializeField] private float _interactableRadius = 1.5f;
    protected Transform PlayerTrans = null;
    private bool _isPlayerIn = false;



    /* ==================== Public Methods ==================== */

    public void Interaction()
    {
        ActiveTrigger();
    }



    /* ==================== Abstract Methods ==================== */

    public abstract Vector2 GetInteractionPos();



    /* ==================== Private Methods ==================== */

    private void Awake()
    {
        GetComponent<Collider>().includeLayers = LAYER_B_PLATFORM;
        _interactableRadius = _interactableRadius * _interactableRadius;
    }


    private void Start()
    {
        PlayerTrans = FourDirectionPlayerControl.Instance.transform;
    }
    

    private void Update()
    {
        Vector2 dis = new Vector2(
            transform.position.x - PlayerTrans.position.x,
            transform.position.z - PlayerTrans.position.z
        );
        if (_isPlayerIn)
        {
            if ((dis.x * dis.x + dis.y * dis.y) > _interactableRadius)
            {
                FourDirectionPlayerControl.Instance.SetInteractionActive(this, false);
                _isPlayerIn = false;
            }
        }
        else
        {
            if ((dis.x * dis.x + dis.y * dis.y) <= _interactableRadius)
            {
                FourDirectionPlayerControl.Instance.SetInteractionActive(this, true);
                _isPlayerIn = true;
            }
        }
    }


#if UNITY_EDITOR
    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _interactableRadius);
    }
#endif
}
