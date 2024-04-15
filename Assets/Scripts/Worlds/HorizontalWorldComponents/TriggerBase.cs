using System;
using System.Collections.Generic;
using UnityEngine;

public class TriggerBase : MonoBehaviour
{
    /* ==================== Fields ==================== */

    [SerializeField, HideInInspector] protected TriggerAction[] Actions = new TriggerAction[0];



    /* ==================== Private Methods ==================== */

    protected void ActiveTrigger()
    {
        foreach (TriggerAction act in Actions)
        {
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
                    act.TargetObject.GetComponent<IEventScene>().StartEventScene();
                    break;

                case ActionTypes.StageClear:
                    StageManagerBase.Instance.StageClear();
                    break;

                case ActionTypes.CustomAction:
                    if (StageManagerBase.Instance == null)
                    {
                        TownManagerBase.Instance.CustomAction(act.Text);
                    }
                    else
                    {
                        StageManagerBase.Instance.CustomAction(act.Text);
                    }
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
        public string Text;
    }
}
