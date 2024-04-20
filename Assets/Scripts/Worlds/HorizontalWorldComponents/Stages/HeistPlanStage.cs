public class HeistPlanStage : StageManagerBase
{
    /* ==================== Public Methods ==================== */

    public override void StageClear()
    {
        // Game data
        GameManager.GameData.CurrentPreperation |= (byte)(1 << GameManager.Instance.CurrentPlanIndex);

        // Next scene
        LoatStageClearScene();
    }


    public override void CustomAction(string action)
    {
        
    }
}
