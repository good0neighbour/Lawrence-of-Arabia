using System;
using UnityEngine;

public class TriggerBase : MonoBehaviour
{
    /* ==================== Fields ==================== */

    [Tooltip("Add actions of this trigger.")]
    [SerializeField] protected TriggerAction[] _actions = null;



    /* ==================== Private Methods ==================== */

    protected void ActiveTrigger()
    {
        foreach (TriggerAction act in _actions)
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



    /* ==================== Struct ==================== */

    [Serializable]
    protected struct TriggerAction
    {
        public ActionTypes ActionType;
        public GameObject TargetObject;
    }
}
