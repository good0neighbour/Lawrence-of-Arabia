using System;
using System.Collections.Generic;
using UnityEngine;

public class TriggerBase : MonoBehaviour
{
    /* ==================== Fields ==================== */

    [SerializeField] protected List<TriggerAction> Actions = null;

#if UNITY_EDITOR
    public List<TriggerAction> ActionsList
    {
        set
        {
            Actions = value;
        }
    }
#endif



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
                    HorizontalPlayerControl.Instance.transform.position = act.TargetObject.transform.position;
                    break;

                case ActionTypes.StartEventScene:
#if UNITY_EDITOR
                    HoriaontalEventScene eventScene = act.TargetObject.GetComponent<HoriaontalEventScene>();
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

                    case ActionTypes.LoadNextScene:
                        MapManager.Instance.LoadNextScene();
                        break;
            }
        }
    }


#if UNITY_EDITOR
    public List<TriggerAction> GetActions()
    {
        return Actions;
    }


    public void AddAction(byte index)
    {
        Actions.Insert(index, new TriggerAction());
    }


    public void DeleteAction(byte index)
    {
        Actions.RemoveAt(index);
    }


    public void MoveAction(byte from, byte to)
    {
        TriggerAction temp = Actions[from];
        Actions.RemoveAt(from);
        Actions.Insert(to, temp);
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
