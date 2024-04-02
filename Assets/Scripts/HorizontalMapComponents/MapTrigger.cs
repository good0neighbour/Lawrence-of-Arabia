using System;
using System.Collections.Generic;
using UnityEngine;

public class MapTrigger : TriggerBase
{
    /* ==================== Fields ==================== */

    [SerializeField] private List<ConditionInfo> _conditions = new List<ConditionInfo>();



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



    /* ==================== Private Methods ==================== */

    private bool CheckConditions(List<ConditionInfo> conditionList)
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
        for (byte i = 0; i < _conditions.Count; ++i)
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
    private struct ConditionInfo
    {
        public ConditionTypes Condition;
        public List<GameObject> Targets;
    }



    /* ==================== Class ==================== */

    private class ConditionCheck : MonoBehaviour
    {
        private MapTrigger _belong;
        private ConditionTypes _condition;
        private byte _index;


        public void InitializeCondition(MapTrigger belong, ConditionTypes condition, byte index)
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
