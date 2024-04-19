using UnityEngine;

public abstract class PlayerWeaponBase
{
    /* ==================== Fields ==================== */

    protected ushort Damage = 0;
    protected float Range = 0.0f;
    protected bool IsPressed = false;
    private ushort _baseDamage = 0;
    private float _baseRange = 0.0f;

    public float AttackTime
    {
        get;
        private set;
    }



    /* ==================== Public Methods ==================== */

    public void WeaponSet(ushort damage, float range)
    {
        Damage = (ushort)(_baseDamage + damage);
        Range = _baseRange + _baseRange * range;
    }


    public void StopAttack()
    {
        IsPressed = false;
    }



    /* ==================== Protected Methods ==================== */

    protected void SetBaseData(ushort baseDamage, float baseRange, float attackTime)
    {
        _baseDamage = baseDamage;
        _baseRange = baseRange;
        AttackTime = attackTime;
    }



    /* ==================== Abstract Methods ==================== */

    public abstract void Attack(Vector2 pos, sbyte direction);
}
