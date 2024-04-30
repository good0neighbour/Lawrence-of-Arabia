using UnityEngine;
using static Constants;

public class CameraHorizontalMovement : MonoBehaviour
{
    /* ==================== Fields ==================== */

    [SerializeField] private Transform _target = null;
    [SerializeField] private Transform _background = null;
    [SerializeField] private Camera _camera = null;
    [SerializeField] private Vector2 _mapTopLeft = Vector2.zero;
    [SerializeField] private Vector2 _mapRightBottom = Vector2.zero;
    [SerializeField] private Vector2 _backgroundTopLeft = Vector2.zero;
    [SerializeField] private Vector2 _backgroundRightBottom = Vector2.zero;
    private Vector2 _mapLength = Vector2.zero;
    private Vector2 _backgroundLength = Vector2.zero;
    private float _targetSize = HOR_CAM_DEFAULT_SIZE;

    public static CameraHorizontalMovement Instance
    {
        get;
        private set;
    }



    /* ==================== Public Methods ==================== */

    public void TargetChange(Transform target)
    {
        _target = target;
    }


    public void DeleteInstance()
    {
        Instance = null;
    }


    public void SetTargetSize(float targetSize)
    {
        _targetSize = targetSize;
    }



    /* ==================== Private Methods ==================== */

    private void Awake()
    {
        Instance = this;
        _mapLength.x = _mapRightBottom.x - _mapTopLeft.x;
        _mapLength.y = _mapRightBottom.y - _mapTopLeft.y;
        _backgroundLength.x = _backgroundRightBottom.x - _backgroundTopLeft.x;
        _backgroundLength.y = _backgroundRightBottom.y - _backgroundTopLeft.y;
    }


    private void Update()
    {
        // Camera Size
        _camera.orthographicSize += (_targetSize - _camera.orthographicSize) * Time.deltaTime * HOR_CAM_SIZE_SPEED;
    }


    private void FixedUpdate()
    {
        // Camer position
        transform.position = Vector3.Lerp(transform.position, _target.position, HOR_CAM_SPEED);

        // Background position
        Vector2 camPos = (Vector2)transform.position;
        _background.localPosition = new Vector3(
            _backgroundTopLeft.x + (_mapRightBottom.x - camPos.x) / _mapLength.x * _backgroundLength.x,
            _backgroundRightBottom.y + (_mapTopLeft.y - camPos.y) / _mapLength.y * _backgroundLength.y,
            0.0f
        );
    }
}
