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
}
