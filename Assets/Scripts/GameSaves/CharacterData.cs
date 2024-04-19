using System;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterData", menuName = "Lawrence of Arabia/CharacterData")]
public class CharacterData : ScriptableObject
{
    /* ==================== Fields ==================== */

    [SerializeField] private Character[] _baseData = new Character[(int)Characters.End];



    /* ==================== Public Methods ==================== */

    public void CharacterDataPreparation()
    {
        _baseData = _baseData.OrderBy(c => c.Name).ToArray();
        for (byte i = 0; i < _baseData.Length; ++i)
        {
            _baseData[i].CurHealth = _baseData[i].BaseHealth + _baseData[i].HealthIncrease * _baseData[i].Level;
            _baseData[i].CurDamage = (ushort)(_baseData[i].BaseDamage + _baseData[i].DamageIncrease * _baseData[i].Level);
            _baseData[i].CurArmor = (ushort)(_baseData[i].BaseArmor + _baseData[i].Level / _baseData[i].LvForArmorIncrease);

            int range = _baseData[i].BaseRangeRate + _baseData[i].RangeRateIncrease * _baseData[i].Level;
            if (range >= 200)
            {
                _baseData[i].CurRange = 200;
            }
            else
            {
                _baseData[i].CurRange = (byte)range;
            }
        }
    }


    public Character[] GetCharacterList()
    {
        return _baseData;
    }


    public void CharacterLevelUp(Characters character)
    {
        byte index = (byte)character;
        ++_baseData[index].Level;

        _baseData[index].CurHealth += _baseData[index].HealthIncrease;
        _baseData[index].CurDamage += _baseData[index].DamageIncrease;
        _baseData[index].CurArmor = (ushort)(_baseData[index].Level / _baseData[index].LvForArmorIncrease);

        int range = _baseData[index].BaseRangeRate + _baseData[index].RangeRateIncrease * _baseData[index].Level;
        if (range >= 200)
        {
            _baseData[index].CurRange = 200;
        }
        else
        {
            _baseData[index].CurRange = (byte)range;
        }
    }


    public void SetCharacterStatus(Characters character, CharacterStatus status)
    {
        _baseData[(int)character].Status = status;
    }


#if UNITY_EDITOR
    public void SetCharacterList(Character[] newArray)
    {
        _baseData = newArray;
    }
#endif



    /* ==================== Struct ==================== */

    [Serializable]
    public struct Character
    {
        public Characters Name;
        public int BaseHealth;
        public ushort BaseArmor;
        public ushort BaseDamage;
        public byte BaseRangeRate;
        public byte HealthIncrease;
        public byte DamageIncrease;
        public byte RangeRateIncrease;
        public byte LvForArmorIncrease;
        public Sprite FullImage;
        public Sprite ButtonImage;
        public Sprite ProfileImage;
        public Sprite Sprite;
        public Sprite SpriteDead;
        public Sprite BackgroundImage;
        public string Information;

        public byte CurRange;
        public int CurHealth;
        public ushort CurArmor;
        public ushort CurDamage;

        public CharacterStatus Status;
        public CharacterTypes Type;
        public byte Level;
        public byte Trust;
        public DialogueScript RecentDialogue;
    }
}
