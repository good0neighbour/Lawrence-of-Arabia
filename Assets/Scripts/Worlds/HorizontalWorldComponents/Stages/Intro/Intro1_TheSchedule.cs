public class Intro1_TheSchedule : StageManagerBase
{
    /* ==================== Public Methods ==================== */

    public override void StageClear()
    {
        // Game data
        GameManager.GameData.CurrentPreperation |= (byte)(1 << GameManager.Instance.CurrentPlanIndex);

        // Next scene
        LoadScene("FD_Intro_SafeHouse");
    }


    public override void StageReturn()
    {
        LoadScene("FD_Intro_SafeHouse");
    }


    public override void CustomAction(string action)
    {
        
    }
}
