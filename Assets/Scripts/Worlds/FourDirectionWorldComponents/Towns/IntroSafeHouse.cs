using UnityEngine;

public class IntroSafeHouse : TownManagerBase
{
    /* ==================== Fields ==================== */

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
                    PauseGame(true);
                    _heistScreen.SetActive(true);
                }
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
            case "WeaponHeist":
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
