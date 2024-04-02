using UnityEngine;

public class AttackEffect : MonoBehaviour
{
    /* ==================== Fields ==================== */

    [SerializeField] private float _velocity = 1.0f;
    [SerializeField] private float _lifeTime = 1.0f;
    [SerializeField] private LayerMask _targetLayer;
    private GameObject _prefab = null;
    private Vector2 _direction = Vector2.zero;
    private float _timer = 0.0f;
    private byte _damage = 0;



    /* ==================== Public Methods ==================== */

    public void InitializeEffect(GameObject prefab)
    {
        _prefab = prefab;
    }


    public void StartEffect(Vector2 direction, byte damage)
    {
        _direction = direction;
        _damage = damage;
    }



    /* ==================== Private Methods ==================== */

    private void Update()
    {
        float deltaTime;

        if (_timer >= _lifeTime)
        {
            MapManager.ObjectPool.ReturnObject(_prefab, gameObject);
            _timer = 0.0f;
            return;
        }

        if (Time.deltaTime > Constants.DELTA_TIME_LIMIT)
        {
            deltaTime = Constants.DELTA_TIME_LIMIT;
        }
        else
        {
            deltaTime = Time.deltaTime;
        }

        transform.localPosition = new Vector3(
            transform.localPosition.x + _velocity * deltaTime * _direction.x,
            transform.localPosition.y + _velocity * deltaTime * _direction.y,
            0.0f
        );

        _timer += deltaTime;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((1 << collision.gameObject.layer & _targetLayer.value) > 0)
        {
            collision.GetComponent<IHit>().Hit(_damage, _direction.x);
            MapManager.ObjectPool.ReturnObject(_prefab, gameObject);
            _timer = 0.0f;
        }
    }
}
