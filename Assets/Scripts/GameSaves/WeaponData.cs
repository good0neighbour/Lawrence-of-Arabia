using System;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "Lawrence of Arabia/WeaponData")]
[PreferBinarySerialization]
public class WeaponData : ScriptableObject
{
    /* ==================== Fields ==================== */

    [SerializeField] private Weapon[] _weapons = new Weapon[0];



    /* ==================== Public Methods ==================== */

    public Weapon[] GetWeaponList()
    {
        return _weapons;
    }


#if UNITY_EDITOR
    public void SetWeaponArray(Weapon[] weaponArray)
    {
        _weapons = weaponArray;
    }
#endif



    /* ==================== Strict ==================== */

    [Serializable]
    public struct Weapon
    {
        public ushort Damage;
        public float Range;
        public float AttackTime;
        public ushort Stock;
        public Sprite Image;
    }
}
