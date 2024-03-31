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
#if UNITY_EDITOR
            if (act.TargetObject == null)
            {
                Debug.LogError($"{gameObject.name} : Target object is missing.");
                return;
            }
#endif
            switch (act.ActionType)
            {
                case ActionTypes.Enable:
                    act.TargetObject.SetActive(true);
                    break;

                case ActionTypes.Disable:
                    act.TargetObject.SetActive(false);
                    break;

                case ActionTypes.Delete:
                    Destroy(act.TargetObject);
                    break;

                case ActionTypes.StartEventScene:
#if UNITY_EDITOR
                    HoriaontalEvenScene eventScene = act.TargetObject.GetComponent<HoriaontalEvenScene>();
                    if (eventScene == null)
                    {
                        Debug.LogError($"{gameObject.name} : Even scene is missing.");
                        return;
                    }
                    else
                    {
                        eventScene.StartEventScene();
                    }
#else
                    act.TargetObject.GetComponent<HoriaontalEvenScene>().StartEventScene();
#endif
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
        public ActionTypes ActionType;
        public GameObject TargetObject;
    }
}
