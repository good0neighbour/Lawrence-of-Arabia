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

    public CharacterData.Character[] SelectedCharacters
    {
        get;
        set;
    }



    /* ==================== Private Methods ==================== */

    private GameManager()
    {
        _characterData = Resources.Load<CharacterData>("Resources/CharacterData");
        _characterData.CharacterDataPreparation();
    }
}
