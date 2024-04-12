using System;
using System.Collections.Generic;
using UnityEngine;

public class TriggerBase : MonoBehaviour
{
    /* ==================== Fields ==================== */

    [SerializeField] protected TriggerAction[] Actions = null;



    /* ==================== Private Methods ==================== */

    protected void ActiveTrigger()
    {
        foreach (TriggerAction act in Actions)
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

                case ActionTypes.PlayerTeleport:
                    if (HorizontalPlayerControl.Instance == null)
                    {
                        FourDirectionPlayerControl.Instance.transform.position = act.TargetObject.transform.position;
                    }
                    else
                    {
                        HorizontalPlayerControl.Instance.transform.position = act.TargetObject.transform.position;
                    }
                    break;

                case ActionTypes.StartEventScene:
#if UNITY_EDITOR
                    IEventScene eventScene = act.TargetObject.GetComponent<IEventScene>();
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
                    act.TargetObject.GetComponent<IEventScene>().StartEventScene();
#endif
                    break;

                    case ActionTypes.LoadNextScene:
                        StageManagerBase.Instance.LoadNextScene();
                        break;
            }
        }
    }


#if UNITY_EDITOR
    public List<TriggerAction> GetActions()
    {
        List<TriggerAction> result = new List<TriggerAction>();
        foreach (TriggerAction action in Actions)
        {
            result.Add(action);
        }
        return result;
    }


    public void SetActions(TriggerAction[] actions)
    {
        Actions = actions;
    }
#endif



    /* ==================== Struct ==================== */

    [Serializable]
    public struct TriggerAction
    {
        public ActionTypes ActionType;
        public GameObject TargetObject;
    }
}
