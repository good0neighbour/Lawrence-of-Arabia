using System;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterData", menuName = "Lawrence of Arabia/CharacterData")]
public class CharacterData : ScriptableObject
{
    /* ==================== Fields ==================== */

    [SerializeField] private Character[] _baseData = null;



    /* ==================== Public Methods ==================== */

    public void CharacterDataPreparation()
    {
        _baseData = _baseData.OrderBy(c => c.Name).ToArray();
        for (byte i = 0; i < _baseData.Length; ++i)
        {
            _baseData[i].CurHealth = _baseData[i].BaseHealth + _baseData[i].HealthIncrease * _baseData[i].Level;
            _baseData[i].CurDamage = (ushort)(_baseData[i].BaseDamage + _baseData[i].DamageIncrease * _baseData[i].Level);
            _baseData[i].CurRange = (byte)(_baseData[i].BaseRangeRate + _baseData[i].RangeRateIncrease * _baseData[i].Level);
            _baseData[i].CurArmor = (ushort)(_baseData[i].BaseArmor + _baseData[i].Level / _baseData[i].LvForArmorIncrease);
        }
    }


    public Character[] GetCharacterList()
    {
        return _baseData;
    }


    public void CharacterLevelUp(Characters character)
    {
        ++_baseData[(int)character].Level;
        _baseData[(int)character].CurHealth += _baseData[(int)character].HealthIncrease;
        _baseData[(int)character].CurDamage += _baseData[(int)character].DamageIncrease;
        _baseData[(int)character].CurArmor = (ushort)(_baseData[(int)character].Level / _baseData[(int)character].LvForArmorIncrease);
    }


    public void SetCharacterStatus(Characters character, CharacterStatus status)
    {
        _baseData[(int)character].Status = status;
    }



    /* ==================== Struct ==================== */

    [Serializable]
    public struct Character
    {
        [Header("Base Data")]
        public Characters Name;
        public int BaseHealth;
        public ushort BaseArmor;
        [Tooltip("Additional damage.")]
        public ushort BaseDamage;
        [Tooltip("Additional range increment percentage.")]
        public byte BaseRangeRate;
        public byte HealthIncrease;
        public byte DamageIncrease;
        public byte RangeRateIncrease;
        [Tooltip("Armor gains 1 increase for every this level.")]
        public byte LvForArmorIncrease;
        public Sprite FullImage;
        public Sprite Sprite;

        [HideInInspector] public byte CurRange;
        [HideInInspector] public int CurHealth;
        [HideInInspector] public ushort CurArmor;
        [HideInInspector] public ushort CurDamage;

        [Header("Game Play Data")]
        public CharacterStatus Status;
        public CharacterTypes Type;
        public CharacterWeapons Weapon;
        public byte Level;
        public byte Trust;
        public DialogueScript RecentDialogue;
    }
}
