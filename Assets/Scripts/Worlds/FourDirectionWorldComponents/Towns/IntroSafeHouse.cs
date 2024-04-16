using UnityEngine;

public class IntroSafeHouse : TownManagerBase
{
    /* ==================== Fields ==================== */

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
                    Instantiate(Resources.Load<GameObject>("HeistPlanScreens/CanvasWeaponHeist"));
                }
                break;
        }
    }



    /* ==================== Protected Methods ==================== */

    protected override void OnStageStart()
    {
        
    }
}
