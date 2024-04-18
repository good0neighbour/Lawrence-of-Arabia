using UnityEngine;

public class IntroSafeHouse : TownManagerBase
{
    /* ==================== Fields ==================== */

    [SerializeField] private GameObject _onTutorial = null;
    [SerializeField] private GameObject _onHeist = null;
    private GameObject _heistScreen = null;

    public static IntroSafeHouse Current
    {
        get;
        private set;
    }



    /* ==================== Public Methods ==================== */

    public override void CustomAction(string action)
    {
        switch (action)
        {
            case "HeistPlan":
                if (GameManager.GameData.CurrentHeist.Equals("WeaponHeist"))
                {
                    _heistScreen.SetActive(true);
                    PauseGame(true);
                }
                break;

            case "EndTutorial":
                _onTutorial.SetActive(false);
                _onHeist.SetActive(true);
                GameManager.GameData.CurrentHeist = "WeaponHeist";
                PauseGame(true);
                _heistScreen.SetActive(true);
                break;
        }
    }



    /* ==================== Protected Methods ==================== */

    protected override void OnStageStart()
    {
        
    }


    protected override void Awake()
    {
        base.Awake();

        // Load heist screen
        switch (GameManager.GameData.CurrentHeist)
        {
            case "Tutorial":
                _onTutorial.SetActive(true);
                _onHeist.SetActive(false);
                _heistScreen = Instantiate(Resources.Load<GameObject>("HeistPlanScreens/CanvasWeaponHeist"));
                break;

            case "WeaponHeist":
                _onTutorial.SetActive(false);
                _onHeist.SetActive(true);
                _heistScreen = Instantiate(Resources.Load<GameObject>("HeistPlanScreens/CanvasWeaponHeist"));
                break;
        }

        // Disable heist screen
        if (_heistScreen != null)
        {
            _heistScreen.SetActive(false);
        }
    }
}
