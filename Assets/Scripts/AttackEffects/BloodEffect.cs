using UnityEngine;

public class BloodEffect : PoolObjectBase
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
