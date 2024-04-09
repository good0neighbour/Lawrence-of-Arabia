using UnityEngine;

public abstract class PoolObjectBase : MonoBehaviour
{
    /* ==================== Fields ==================== */

    private GameObject _prefab = null;



    /* ==================== Public Methods ==================== */

    public void InitializeEffect(GameObject prefab)
    {
        _prefab = prefab;
    }



    /* ==================== Protected Methods ==================== */

    protected void ReturnObject()
    {
        StageManagerBase.ObjectPool.ReturnObject(_prefab, gameObject);
    }
}
