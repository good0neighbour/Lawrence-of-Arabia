public class HoriaontalEventScene : EventSceneBase
{
    /* ==================== Public Methods ==================== */

    public override void StartEventScene()
    {
        base.StartEventScene();
        StageManagerBase.Instance.PauseGame(true);
    }


    /* ==================== Protected Methods ==================== */

    protected override void EndEventScene()
    {
        CameraHorizontalMovement.Instance.TargetChange(HorizontalPlayerControl.Instance.CameraPos);
        StageManagerBase.Instance.PauseGame(false);
    }


    protected override void NextAction()
    {
        switch (Actions[Current].Action)
        {
            case EventSceneActions.CameraMove:
                CameraHorizontalMovement.Instance.TargetChange(Actions[Current].TargetTransform);
                return;

            case EventSceneActions.NPCMove:
                Actions[Current].TargetObject.GetComponent<EventNPCMovement>().SetGoal(Actions[Current].TargetTransform.position.x);
                return;

            case EventSceneActions.NPCJump:
                Actions[Current].TargetObject.GetComponent<EventNPCMovement>().NPCJump(Actions[Current].TargetTransform.position.y);
                return;

            case EventSceneActions.NPCLookAt:
                Actions[Current].TargetObject.GetComponent<EventNPCMovement>().NPCLookAt(Actions[Current].TargetTransform);
                return;

            case EventSceneActions.PlayerLookAt:
                HorizontalPlayerControl.Instance.PlayerLookAt(Actions[Current].TargetTransform);
                return;

            case EventSceneActions.Enable:
                Actions[Current].TargetObject.SetActive(true);
                return;

            case EventSceneActions.Disable:
                Actions[Current].TargetObject.SetActive(false);
                return;

            case EventSceneActions.Destroy:
                Destroy(Actions[Current].TargetObject);
                return;

            case EventSceneActions.StartDialogue:
                DialogueScreen.Instance.StartDialogue(Actions[Current].DialogueScript, this);
                ResumeEventScene(false);
                return;

            case EventSceneActions.CloseDialogue:
                DialogueScreen.Instance.CloseDialogueScreen();
                return;

            case EventSceneActions.LoadNextScene:
                StageManagerBase.Instance.LoadNextScene();
                return;
        }
    }
}
