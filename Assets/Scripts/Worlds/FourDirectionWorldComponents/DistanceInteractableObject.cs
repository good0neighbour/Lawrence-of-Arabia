using UnityEngine;

public class DistanceInteractableObject : InteractableObjectBase
{
    /* ==================== Fields ==================== */

    [Header("Interaction Position")]
    [SerializeField] private float  _activeDistance = 1.0f;



    /* ==================== Public Methods ==================== */

    public override Vector2 GetInteractionPos()
    {
        Vector2 pos = new Vector2(transform.position.x, transform.position.z);
        Vector2 temp = new Vector2(
            PlayerTrans.transform.position.x - pos.x,
            PlayerTrans.transform.position.z - pos.y
        );
        return pos + temp.normalized * _activeDistance;
    }



    /* ==================== Private Methods ==================== */

#if UNITY_EDITOR
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, _activeDistance);
    }
#endif
}
