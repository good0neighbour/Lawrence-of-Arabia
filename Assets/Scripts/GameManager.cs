using System.Collections.Generic;
using UnityEngine;

public delegate void GameDelegate();
public delegate byte BehaviourDelegate();

public class GameManager
{
    /* ==================== Fields ==================== */

    private static GameManager _instance = null;
    private CharacterData _characterData = null;

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
        private set
        {
            _instance = value;
        }
    }

    public Characters[] SelectedCharacters
    {
        get;
        set;
    }



    /* ==================== Public Methods ==================== */

    public CharacterData.Character[] GetCharacterList()
    {
        return _characterData.GetCharacterList();
    }


    public List<CharacterData.Character> GetActiveCharacterList()
    {
        return _characterData.GetActiveCharacterList();
    }



    /* ==================== Private Methods ==================== */

    private GameManager()
    {
        _characterData = Resources.Load<CharacterData>("CharacterData");
        _characterData.CharacterDataPreparation();
    }
}
