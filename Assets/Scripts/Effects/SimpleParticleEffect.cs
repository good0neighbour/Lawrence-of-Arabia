using UnityEngine;

public class SimpleParticleEffect : ObjectPool.PoolObject
{
    /* ==================== Fields ==================== */

    [SerializeField] private ParticleSystem _particleSystem = null;



    /* ==================== Private Methods ==================== */

    private void Update()
    {
        if (_particleSystem.isStopped)
        {
            ReturnObject();
        }
    }
}
