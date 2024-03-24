using System;
using UnityEngine;

public class MapTrigger : MonoBehaviour
{
    /* ==================== Fields ==================== */

    [Tooltip("Select condition of this trigger.")]
    [SerializeField] private TriggerTypes _triggerType;
    [Tooltip("Add actions of this trigger.")]
    [SerializeField] private ActionInfo[] _actions = null;



    /* ==================== Private Methods ==================== */

    private void ActiveTrigger()
    {
        foreach (ActionInfo act in _actions)
        {
            switch (act.ActionType)
            {
                case ActionTypes.Enable:
#if UNITY_EDITOR
                    if (act.TargetObject == null)
                        Debug.LogError($"{gameObject.name} : Target object is missing.");
#endif
                    act.TargetObject.SetActive(true);
                    break;

                case ActionTypes.Disable:
#if UNITY_EDITOR
                    if (act.TargetObject == null)
                        Debug.LogError($"{gameObject.name} : Target object is missing.");
#endif
                    act.TargetObject.SetActive(false);
                    break;

                case ActionTypes.Delete:
#if UNITY_EDITOR
                    if (act.TargetObject == null)
                        Debug.LogError($"{gameObject.name} : Target object is missing.");
#endif
                    Destroy(act.TargetObject);
                    break;

                case ActionTypes.StartDialogue:
#if UNITY_EDITOR
                    if (act.DialogueScript == null)
                        Debug.LogError($"{gameObject.name} : Dialogue script is missing.");
#endif
                    FindAnyObjectByType<DialogueScreen>(FindObjectsInactive.Include).StartDialogue(act.DialogueScript);
                    break;

                default:
                    break;
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (_triggerType)
        {
            case TriggerTypes.Enter:
                if (collision.gameObject.layer == Constants.LAYER_D_PLAYER)
                {
                    ActiveTrigger();
                }
                return;

            case TriggerTypes.Interact:
                HorizontalPlayerControl.Instance.SetInteractBtnActive(ActiveTrigger);
                return;

            default:
                return;
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        switch (_triggerType)
        {
            case TriggerTypes.Exit:
                if (collision.gameObject.layer == Constants.LAYER_D_PLAYER)
                {
                    ActiveTrigger();
                }
                return;

            case TriggerTypes.Interact:
                HorizontalPlayerControl.Instance.SetInteractBtnInactive();
                return;

            default:
                return;
        }
    }



    /* ==================== Struct ==================== */

    [Serializable]
    private struct ActionInfo
    {
        [Tooltip("Select action type.")]
        public ActionTypes ActionType;
        [Tooltip("Target object when ActionType is Enter, Exit or Delete.")]
        public GameObject TargetObject;
        [Tooltip("Only available when ActionType is StartDialogue.")]
        public DialogueScript DialogueScript;
    }
}
