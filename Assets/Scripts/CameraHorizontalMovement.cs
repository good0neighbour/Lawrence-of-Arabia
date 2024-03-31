using UnityEngine;

public class CameraHorizontalMovement : MonoBehaviour
{
    /* ==================== Fields ==================== */

    [SerializeField] private Transform _target = null;

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



    /* ==================== Private Methods ==================== */

    private void Awake()
    {
        Instance = this;
    }


    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, _target.position, Constants.HOR_CAM_SPEED);
    }
}
