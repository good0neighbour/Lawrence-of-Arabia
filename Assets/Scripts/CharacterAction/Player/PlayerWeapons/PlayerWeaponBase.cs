using UnityEngine;

public abstract class PlayerWeaponBase
{
    /* ==================== Fields ==================== */

    protected bool IsPressed = false;

    public float AttackTime
    {
        get;
        set;
    }



    /* ==================== Public Methods ==================== */

    public void StopAttack()
    {
        IsPressed = false;
    }



    /* ==================== Abstract Methods ==================== */

    public abstract void Attack(Vector2 pos, sbyte direction, ushort damage);
}
