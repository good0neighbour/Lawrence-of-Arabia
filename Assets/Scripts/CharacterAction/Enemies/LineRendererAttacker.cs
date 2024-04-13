using UnityEngine;
using static Constants;

public class LineRendererAttacker : EnemyBase
{
    /* ==================== Fields ==================== */

    [Header("Attack Effect")]
    [SerializeField] private GameObject _atkEftPrefab = null;
    [SerializeField] private Vector2 _attackPos = new Vector2(0.5f, 0.5f);



    /* ==================== Protected Methods ==================== */

    protected override byte Attack()
    {
        // Effect start
        StageManagerBase.ObjectPool.GetObject(_atkEftPrefab).GetComponent<LineRendererAttackEffect>().Begin(
            new Vector2(
                transform.position.x + _attackPos.x * IsFlipNum,
                transform.position.y + _attackPos.y
            ),
            new Vector2(
                Player.position.x,
                Player.position.y + PLAYER_RADIUS
            )
        );

        // Hit player
        HorizontalPlayerControl.Instance.Hit(AttackDamage, IsFlipNum);

        // Return
        return SUCCESS;
    }


    protected override void Start()
    {
        base.Start();
        StageManagerBase.ObjectPool.PoolPreparing(_atkEftPrefab);
    }
}
