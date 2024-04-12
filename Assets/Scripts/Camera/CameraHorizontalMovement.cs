using UnityEngine;

public class CameraHorizontalMovement : MonoBehaviour
{
    /* ==================== Fields ==================== */

    [SerializeField] private Transform _target = null;
    [SerializeField] private Transform _background = null;
    [SerializeField] private Vector2 _mapTopLeft = Vector2.zero;
    [SerializeField] private Vector2 _mapRightBottom = Vector2.zero;
    [SerializeField] private Vector2 _backgroundTopLeft = Vector2.zero;
    [SerializeField] private Vector2 _backgroundRightBottom = Vector2.zero;
    private Vector2 _mapLength = Vector2.zero;
    private Vector2 _backgroundLength = Vector2.zero;

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
        // Camer position
        transform.position = Vector3.Lerp(transform.position, _target.position, Constants.HOR_CAM_SPEED);

        // Background position
        Vector2 camPos = (Vector2)transform.position;
        _background.localPosition = new Vector3(
            _backgroundTopLeft.x + (_mapRightBottom.x - camPos.x) / _mapLength.x * _backgroundLength.x,
            _backgroundRightBottom.y + (_mapTopLeft.y - camPos.y) / _mapLength.y * _backgroundLength.y,
            0.0f
        );
    }
}