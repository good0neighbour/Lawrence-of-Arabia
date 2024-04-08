using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterData", menuName = "Lawrence of Arabia/CharacterData")]
public class CharacterData : ScriptableObject
{
    /* ==================== Fields ==================== */

    [SerializeField] private Character[] _baseData = null;
    [NonSerialized] private List<Character> _activeCharacters = new List<Character>();



    /* ==================== Public Methods ==================== */

    public void CharacterDataPreparation()
    {
        _baseData = _baseData.OrderBy(c => c.Name).ToArray();
        for (byte i = 0; i < _baseData.Length; ++i)
        {
            switch (_baseData[i].Status)
            {
                case CharacterStatus.None:
                    break;

                default:
                    _activeCharacters.Add(_baseData[i]);
                    break;
            }
            _baseData[i].CurHealth = _baseData[i].BaseHealth + _baseData[i].HealthIncrease * _baseData[i].Level;
            _baseData[i].CurDamage = (ushort)(_baseData[i].BaseDamage + _baseData[i].DamageIncrease * _baseData[i].Level);
            _baseData[i].CurArmor = (ushort)(_baseData[i].BaseArmor + _baseData[i].Level / _baseData[i].LvForArmorIncrease);
        }
    }


    public List<Character> GetActiveCharacterList()
    {
        return _activeCharacters;
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
        if (_baseData[(int)character].Status == CharacterStatus.None
            && status != CharacterStatus.None)
        {
            _activeCharacters.Add(_baseData[(int)character]);
        }
        _baseData[(int)character].Status = status;
    }



    /* ==================== Struct ==================== */

    [Serializable]
    public struct Character
    {
        public Characters Name;
        public int BaseHealth;
        public ushort BaseArmor;
        public ushort BaseDamage;
        public byte HealthIncrease;
        public byte DamageIncrease;
        [Tooltip("Armor gains 1 increase for every this level.")]
        public byte LvForArmorIncrease;
        public Sprite FullImage;
        public Sprite Sprite;

        public ushort CurArmor;
        public ushort CurDamage;
        public int CurHealth;

        [Header("Game Play Data")]
        public CharacterStatus Status;
        public CharacterTypes Type;
        public CharacterWeapons Weapons;
        public byte Level;
    }
}
