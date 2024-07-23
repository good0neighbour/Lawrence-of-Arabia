using UnityEngine;

public class TouchButton : MonoBehaviour
{
    /* ==================== Fields ==================== */

    [SerializeField] TouchInput TargetKey;



    /* ==================== Public Methods ==================== */

    public void TouchDown()
    {
        UserControl.Instance.TouchDown((byte)TargetKey);
    }


    public void TouchUp()
    {
        UserControl.Instance.TouchUp((byte)TargetKey);
    }
}
