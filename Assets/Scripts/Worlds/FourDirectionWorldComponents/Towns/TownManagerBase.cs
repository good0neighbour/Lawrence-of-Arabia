using System.Collections.Generic;

public abstract class TownManagerBase : WorldManagerBase
{
    /* ==================== Fields ==================== */

    private CharacterData.Character[] _charsGot = null;
    private CharacterData.Character[] _activeChars = null;

    public static TownManagerBase Instance
    {
        get;
        private set;
    }



    /* ==================== Public Methods ==================== */

    public override void PauseGame(bool pause)
    {
        base.PauseGame(pause);

        // Player
        FourDirectionPlayerControl.Instance.PlayerPause(pause);
    }


    public CharacterData.Character[] GetCharList()
    {
        return _charsGot;
    }


    public CharacterData.Character[] GetActiveCharList()
    {
        return _activeChars;
    }



    /* ==================== Protected Methods ==================== */

    protected override void DeleteInstance()
    {
        base.DeleteInstance();

        Instance = null;
        FourDirectionPlayerControl.Instance.DeleteInstance();
        CameraFourDirectionMovement.Instance.DeleteInstance();
    }


    protected override void Awake()
    {
        base.Awake();

        // Singleton pattern
        Instance = this;

        // Characters got
        CharacterData.Character[] allChar = GameManager.CharacterData.GetCharacterList();
        List<CharacterData.Character> got = new List<CharacterData.Character>();
        List<CharacterData.Character> active = new List<CharacterData.Character>();
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
                    got.Add(allChar[i]);
                    break;

                case CharacterStatus.Active:
                    got.Add(allChar[i]);
                    active.Add(allChar[i]);
                    break;
            }
        }
        _charsGot = got.ToArray();
        _activeChars = active.ToArray();
    }
}
