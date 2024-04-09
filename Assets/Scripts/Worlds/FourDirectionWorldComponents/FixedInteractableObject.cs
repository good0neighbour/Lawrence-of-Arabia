using UnityEngine;

public class FixedInteractableObject : InteractableObjectBase
{
    /* ==================== Fields ==================== */

    [Header("Interaction Position")]
    [SerializeField] private Transform[] _fixedPosition = null;



    /* ==================== Public Methods ==================== */

    public override Vector2 GetInteractionPos()
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
}
