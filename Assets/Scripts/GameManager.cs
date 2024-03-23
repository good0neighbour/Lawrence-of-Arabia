using UnityEngine;

public delegate void GameDelegate();
public delegate byte BehaviourDelegate();

public class GameManager
{
    /* ==================== Fields ==================== */

    private static GameManager _instance = null;
    private CharacterData[] _characterArray;

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



    /* ==================== Public Methods ==================== */

    public GameManager()
    {
        // Character data load
        _characterArray = new CharacterData[(int)Characters.None];
        for (Characters i = Characters.Player + 1; i < Characters.None; ++i)
        {
            _characterArray[(int)i].FullImage = Resources.Load<Sprite>($"Characters/{i.ToString()}FullImage");
            _characterArray[(int)i].Sprite = Resources.Load<Sprite>($"Characters/{i.ToString()}Sprite");
        }
    }


    public CharacterData[] GetCharacterData()
    {
        return _characterArray;
    }



    /* ==================== Struct ==================== */

    public struct CharacterData
    {
        public Sprite FullImage;
        public Sprite Sprite;
    }
}
