public class HoriaontalEventScene : EventSceneBase
{
    /* ==================== Public Methods ==================== */

    public override void StartEventScene()
    {
        base.StartEventScene();
        StageManager.Instance.PauseGame(true);
    }


    /* ==================== Protected Methods ==================== */

    protected override void EndEventScene()
    {
        CameraHorizontalMovement.Instance.TargetChange(HorizontalPlayerControl.Instance.CameraPos);
        StageManager.Instance.PauseGame(false);
    }


    protected override void NextAction()
    {
        switch (Actions[Current].Action)
        {
            case EvenSceneActions.CameraMove:
                CameraHorizontalMovement.Instance.TargetChange(Actions[Current].TargetTransform);
                return;

            case EvenSceneActions.NPCMove:
                Actions[Current].TargetObject.GetComponent<EventNPCMovement>().SetGoal(Actions[Current].TargetTransform.position.x);
                return;

            case EvenSceneActions.NPCJump:
                Actions[Current].TargetObject.GetComponent<EventNPCMovement>().NPCJump(Actions[Current].TargetTransform.position.y);
                return;

            case EvenSceneActions.NPCLookAt:
                Actions[Current].TargetObject.GetComponent<EventNPCMovement>().NPCLookAt(Actions[Current].TargetTransform);
                return;

            case EvenSceneActions.PlayerLookAt:
                HorizontalPlayerControl.Instance.PlayerLookAt(Actions[Current].TargetTransform);
                return;

            case EvenSceneActions.Enable:
                Actions[Current].TargetObject.SetActive(true);
                return;

            case EvenSceneActions.Disable:
                Actions[Current].TargetObject.SetActive(false);
                return;

            case EvenSceneActions.Destroy:
                Destroy(Actions[Current].TargetObject);
                return;

            case EvenSceneActions.StartDialogue:
                DialogueScreen.Instance.StartDialogue(Actions[Current].DialogueScript, this);
                ResumeEventScene(false);
                return;

            case EvenSceneActions.CloseDialogue:
                DialogueScreen.Instance.CloseDialogueScreen();
                return;

            case EvenSceneActions.LoadNextScene:
                StageManager.Instance.LoadNextScene();
                return;
        }
    }
}
