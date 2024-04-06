using UnityEngine;

public class FourDirectionEventScene : EventSceneBase
{
    /* ==================== Public Methods ==================== */

    public override void StartEventScene()
    {
        base.StartEventScene();
        TownManager.Instance.PauseGame(true);
    }


    /* ==================== Protected Methods ==================== */

    protected override void EndEventScene()
    {
        CameraFourDirectionMovement.Instance.TargetChange(FourDirectionPlayerControl.Instance.CameraPos);
        TownManager.Instance.PauseGame(false);
    }


    protected override void NextAction()
    {
        switch (Actions[Current].Action)
        {
            case EvenSceneActions.CameraMove:
                CameraFourDirectionMovement.Instance.TargetChange(Actions[Current].TargetTransform);
                return;

            case EvenSceneActions.NPCMove:
                Actions[Current].TargetObject.GetComponent<EventNPCMovementFD>().SetGoal(Actions[Current].TargetTransform.position);
                return;

            case EvenSceneActions.NPCLookAtPlayer:
                Actions[Current].TargetObject.GetComponent<EventNPCMovementFD>().NPCLookAtPlayer();
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
                TownManager.Instance.LoadNextScene();
                return;

#if UNITY_EDITOR
            case EvenSceneActions.NPCJump:
                Debug.LogError("Cannot jump in FourDirection map.");
                return;
#endif
        }
    }
}
