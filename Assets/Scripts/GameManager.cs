using UnityEngine;

public delegate void GameDelegate();
public delegate byte BehaviourDelegate();

public class GameManager
{
    /* ==================== Fields ==================== */

    private static GameManager _instance = null;

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

    public CharacterData CharacterData
    {
        get;
        private set;
    }

    public NPCData NPCData
    {
        get;
        private set;
    }

    public GameData GameData
    {
        get;
        private set;
    }

    public Characters[] SelectedCharacters
    {
        get;
        set;
    }

    public Factions CurrentMission
    {
        get;
        set;
    }



    /* ==================== Private Methods ==================== */

    private GameManager()
    {
        CharacterData = Resources.Load<CharacterData>("CharacterData");
        CharacterData.CharacterDataPreparation();
        NPCData = Resources.Load<NPCData>("NPCData");
        NPCData.NPCDataPreparation();
        GameData = Resources.Load<GameData>("GameData");
    }
}
