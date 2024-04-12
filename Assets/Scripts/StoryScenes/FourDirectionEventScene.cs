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
            case EventSceneActions.CameraMove:
                CameraFourDirectionMovement.Instance.TargetChange(Actions[Current].TargetTransform);
                return;

            case EventSceneActions.NPCMove:
                Actions[Current].TargetObject.GetComponent<EventNPCMovementFD>().SetGoal(Actions[Current].TargetTransform.position);
                return;

            case EventSceneActions.NPCLookAt:
                Actions[Current].TargetObject.GetComponent<EventNPCMovementFD>().NPCLookAt(Actions[Current].TargetTransform);
                return;

            case EventSceneActions.PlayerLookAt:
                FourDirectionPlayerControl.Instance.PlayerLookAt(Actions[Current].TargetTransform);
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
                TownManager.Instance.LoadNextScene();
                return;

#if UNITY_EDITOR
            case EventSceneActions.NPCJump:
                Debug.LogError("Cannot jump in FourDirection map.");
                return;
#endif
        }
    }
}
