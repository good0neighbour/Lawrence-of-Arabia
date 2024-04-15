using System;
using System.Collections.Generic;
using UnityEngine;

public class StageTrigger : TriggerBase
{
    /* ==================== Fields ==================== */

    [SerializeField] private ConditionInfo[] _conditions = new ConditionInfo[0];



    /* ==================== Public Methods ==================== */

    public void CheckConditions()
    {
        if (CheckConditions(_conditions))
        {
            ActiveTrigger();
        }
    }


    public void DestroyReport(byte index, GameObject delete)
    {
        _conditions[index].Targets.Remove(delete);
    }


#if UNITY_EDITOR
    public List<ConditionInfo> GetConditions()
    {
        List<ConditionInfo> result = new List<ConditionInfo>();
        foreach (ConditionInfo action in _conditions)
        {
            result.Add(action);
        }
        return result;
    }


    public void SetConditions(ConditionInfo[] actions)
    {
        _conditions = actions;
    }
#endif



    /* ==================== Private Methods ==================== */

    private bool CheckConditions(ConditionInfo[] conditionList)
    {
        foreach (ConditionInfo condition in conditionList)
        {
            switch (condition.Condition)
            {
                case ConditionTypes.Destroyed:
                    if (condition.Targets.Count > 0)
                    {
                        return false;
                    }
                    break;

                case ConditionTypes.Disabled:
                    foreach (GameObject target in condition.Targets)
                    {
                        switch (target.activeSelf)
                        {
                            case true:
                                return false;
                        }
                    }
                    break;

                case ConditionTypes.Enabled:
                    foreach (GameObject target in condition.Targets)
                    {
                        switch (target.activeSelf)
                        {
                            case false:
                                return false;
                        }
                    }
                    break;
            }
        }
        return true;
    }


    private void Awake()
    {
        // Victory conditions
        for (byte i = 0; i < _conditions.Length; ++i)
        {
            foreach (GameObject target in _conditions[i].Targets)
            {
                target.AddComponent<ConditionCheck>().InitializeCondition(
                    this,
                    _conditions[i].Condition,
                    i
                );
            }
        }
    }



    /* ==================== Struct ==================== */

    [Serializable]
    public struct ConditionInfo
    {
        public ConditionTypes Condition;
        public List<GameObject> Targets;


        public void AddTarget()
        {
            Targets.Add(null);
        }


        public void DeleteTarget()
        {
            Targets.RemoveAt(Targets.Count - 1);
        }
    }



    /* ==================== Class ==================== */

    private class ConditionCheck : MonoBehaviour
    {
        private StageTrigger _belong;
        private ConditionTypes _condition;
        private byte _index;


        public void InitializeCondition(StageTrigger belong, ConditionTypes condition, byte index)
        {
            _belong = belong;
            _condition = condition;
            _index = index;
        }


        private void OnDestroy()
        {
            switch (_condition)
            {
                case ConditionTypes.Destroyed:
                    _belong.DestroyReport(_index, gameObject);
                    _belong.CheckConditions();
                    return;

                default:
                    return;
            }
        }


        private void OnDisable()
        {
            switch (_condition)
            {
                case ConditionTypes.Disabled:
                    _belong.CheckConditions();
                    return;

                default:
                    return;
            }
        }


        private void OnEnable()
        {
            switch (_condition)
            {
                case ConditionTypes.Enabled:
                    _belong.CheckConditions();
                    return;

                default:
                    return;
            }
        }
    }
}
