using System.Collections.Generic;
using UnityEngine;

public delegate void GameDelegate();
public delegate byte BehaviourDelegate();

public class GameManager
{
    /* ==================== Fields ==================== */

    private static GameManager _instance = null;
    private static CharacterData _characterData = null;
    private static NPCData _npcData = null;
    private static GameData _gameData = null;
    private List<CharacterData.Character> _charsGot = new List<CharacterData.Character>();
    private List<CharacterData.Character> _activeChars = new List<CharacterData.Character>();

    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameManager();
            }
            return _instance;
        }
    }

    public static CharacterData CharacterData
    {
        get
        {
            if (_characterData == null)
            {
                _characterData = Resources.Load<CharacterData>("CharacterData");
                _characterData.CharacterDataPreparation();
            }
            return _characterData;
        }
    }

    public static NPCData NPCData
    {
        get
        {
            if (_npcData == null)
            {
                _npcData = Resources.Load<NPCData>("NPCData");
                _npcData.NPCDataPreparation();
            }
            return _npcData;
        }
    }

    public static GameData GameData
    {
        get
        {
            if (_gameData == null)
            {
                _gameData = Resources.Load<GameData>("GameData");
            }
            return _gameData;
        }
    }

    public Characters[] SelectedCharacters
    {
        get;
        set;
    }

    public CharacterWeapons[] SelectedWeapons
    {
        get;
        set;
    }



    /* ==================== Public Methods ==================== */

    public GameManager()
    {
        // Characters got
        CharacterData.Character[] allChar = CharacterData.GetCharacterList();
        for (byte i = 0; i < allChar.Length; ++i)
        {
            switch (allChar[i].Status)
            {
                case CharacterStatus.None:
                    break;

                case CharacterStatus.Away:
                case CharacterStatus.Injury:
                case CharacterStatus.MIA:
                case CharacterStatus.KIA:
                    _charsGot.Add(allChar[i]);
                    break;

                case CharacterStatus.Active:
                    _charsGot.Add(allChar[i]);
                    _activeChars.Add(allChar[i]);
                    break;
            }
        }
    }


    public CharacterData.Character[] GetCharactersGot()
    {
        return _charsGot.ToArray();
    }


    public CharacterData.Character[] GetActiveCharList()
    {
        return _activeChars.ToArray();
    }
}
