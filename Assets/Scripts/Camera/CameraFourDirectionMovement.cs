using UnityEngine;

public class CameraFourDirectionMovement : MonoBehaviour
{
    /* ==================== Fields ==================== */

    [SerializeField] private Transform _target = null;

    public static CameraFourDirectionMovement Instance
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
        // Singleton pattern
        Instance = this;

        // Camera rotation
        transform.localRotation = Quaternion.Euler(
            Constants.FD_CAM_ROT_OFFSET - Mathf.Atan(Constants.FD_CAM_OFFSET.x / Constants.FD_CAM_OFFSET.y) / Mathf.PI * 180.0f,
            0.0f,
            0.0f
        );
    }


    private void Update()
    {
        // Camera position
        transform.position = Vector3.Lerp(
            transform.position,
            _target.position + new Vector3(0.0f, Constants.FD_CAM_OFFSET.x, Constants.FD_CAM_OFFSET.y),
            Constants.FD_CAM_SPEED
        );
    }
}
