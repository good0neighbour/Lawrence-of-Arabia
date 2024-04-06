using UnityEngine;

public class FixedInteractableObject : MonoBehaviour, IInteractableObject
{
    /* ==================== Fields ==================== */

    [SerializeField] private Transform[] _fixedPosition = null;
    [Tooltip("It squares when game starts.")]
    [SerializeField] private float _interacionRadius = 1.5f;
    [Header("Interaction Options")]
    [SerializeField] private FDInteractionTypes _interactionType = FDInteractionTypes.Enable;
    [SerializeField] private GameObject _targetObject = null;
    private Transform _playerTrans = null;
    private Vector2 _thisPos = Vector2.zero;
    private bool _isPlayerIn = false;



    /* ==================== Public Methods ==================== */

    public Vector2 GetFixedPosition()
    {
        byte index = 0;
        float dis = float.MaxValue;
        Vector3 playerPos = FourDirectionPlayerControl.Instance.transform.position;
        for (byte i = 0; i < _fixedPosition.Length; ++i)
        {
            Vector2 temp = new Vector2(
                _fixedPosition[i].position.x - playerPos.x,
                _fixedPosition[i].position.z - playerPos.z
            );
            float newDis = temp.x * temp.x + temp.y * temp.y;
            if (newDis < dis)
            {
                index = i;
                dis = newDis;
            }
        }
        return new Vector2(_fixedPosition[index].position.x, _fixedPosition[index].position.z);
    }


    public void Interaction()
    {
        switch (_interactionType)
        {
            case FDInteractionTypes.Enable:
                _targetObject.SetActive(true);
                return;

            case FDInteractionTypes.StartEventScene:
#if UNITY_EDITOR
                FourDirectionEventScene eventScene = _targetObject.GetComponent<FourDirectionEventScene>();
                if (eventScene == null)
                {
                    Debug.LogError($"{gameObject.name} : Even scene is missing.");
                    return;
                }
                else
                {
                    eventScene.StartEventScene();
                }
#else
                    _targetObject.GetComponent<HoriaontalEvenScene>().StartEventScene();
#endif
                return;
        }
    }



    /* ==================== Private Methods ==================== */

    private void Awake()
    {
        _thisPos = new Vector2(transform.position.x, transform.position.z);
        _interacionRadius = _interacionRadius * _interacionRadius;
    }


    private void Start()
    {
        _playerTrans = FourDirectionPlayerControl.Instance.transform;
    }


    private void Update()
    {
        Vector2 dis = new Vector2(
            _thisPos.x - _playerTrans.position.x,
            _thisPos.y - _playerTrans.position.z
        );
        if (_isPlayerIn)
        {
            if ((dis.x * dis.x + dis.y * dis.y) > _interacionRadius)
            {
                FourDirectionPlayerControl.Instance.SetInteractionInactive();
                _isPlayerIn = false;
            }
        }
        else
        {
            if ((dis.x * dis.x + dis.y * dis.y) <= _interacionRadius)
            {
                FourDirectionPlayerControl.Instance.SetInteractionActive(this);
                _isPlayerIn = true;
            }
        }
    }


#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _interacionRadius);
    }
#endif
}
