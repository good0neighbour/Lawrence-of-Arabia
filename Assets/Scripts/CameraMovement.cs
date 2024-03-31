using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    /* ==================== Fields ==================== */

    [SerializeField] private Transform _target = null;



    /* ==================== Private Methods ==================== */

    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, _target.position, Constants.HOR_CAM_SPEED);
    }
}
