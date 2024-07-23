using UnityEngine;

public class SimpleParticleEffect : MonoBehaviour, ObjectPool.PoolObject
{
    /* ==================== Fields ==================== */

    [SerializeField] private ParticleSystem _particleSystem = null;

    public GameObject PrefabType
    {
        get;
        set;
    }



    /* ==================== Private Methods ==================== */

    private void Update()
    {
        if (_particleSystem.isStopped)
        {
            StageManagerBase.ObjectPool.ReturnObject(PrefabType, gameObject);
        }
    }
}
