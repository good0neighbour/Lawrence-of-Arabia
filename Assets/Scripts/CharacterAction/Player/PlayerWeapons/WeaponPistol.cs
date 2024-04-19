using UnityEngine;
using static Constants;

public class WeaponPistol : PlayerWeaponBase
{
    /* ==================== Fields ==================== */

    private GameObject _prefab = null;



    /* ==================== Public Methods ==================== */

    public WeaponPistol()
    {
        _prefab = Resources.Load<GameObject>("PlayerWeapons/PistolEffect");
        StageManagerBase.ObjectPool.PoolPreparing(_prefab);
        WeaponData.Weapon[] weapons = GameManager.WeaponData.GetWeaponList();
        SetBaseData(
            weapons[(int)CharacterWeapons.Pistol].Damage,
            weapons[(int)CharacterWeapons.Pistol].Range,
            weapons[(int)CharacterWeapons.Pistol].AttackTime
        );
    }


    public override void Attack(Vector2 pos, sbyte direction)
    {
        if (IsPressed)
        {
            return;
        }

        IsPressed = true;

        // Hit scan
        RaycastHit2D hit = Physics2D.Raycast(pos, new Vector2(direction, 0.0f), Range, LAYER_B_ENEMY + LAYER_B_TERRAIN);

        // Attack effect
        LineRendererAttackEffect eft = StageManagerBase.ObjectPool.GetObject(_prefab).GetComponent<LineRendererAttackEffect>();
        if (hit.collider == null)
        {
            eft.Begin(
                pos,
                new Vector2(
                    pos.x + Range * direction,
                    pos.y
                )
            );
        }
        else
        {
            eft.Begin(
                pos,
                new Vector2(
                    hit.transform.position.x,
                    pos.y
                )
            );

            // Deal Damage
            GameObject target = hit.collider.gameObject;
            if (target.layer == LAYER_D_ENEMY)
            {
                target.GetComponent<IHit>().Hit(Damage, direction);
            }
        }
    }
}
