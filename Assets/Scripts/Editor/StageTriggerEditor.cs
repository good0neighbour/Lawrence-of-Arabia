using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(StageTrigger))]
public class StageTriggerEditor : Editor
{
    private StageTrigger _trigger = null;
    private List<StageTrigger.ConditionInfo> _conditions = null;
    private List<TriggerBase.TriggerAction> _actions = null;
    private byte _conCurrent = 0;
    private byte _conSwitchFrom = 0;
    private byte _conSwitchTo = 0;
    private byte _actCurrent = 0;
    private byte _actSwitchFrom = 0;
    private byte _actSwitchTo = 0;


    private void OnEnable()
    {
        _trigger = (StageTrigger)target;
        _actions = _trigger.GetActions();
        _conditions = _trigger.GetConditions();
        _conCurrent = (byte)(_conditions.Count - 1);
        _actCurrent = (byte)(_actions.Count - 1);
    }


    private void ConditionField()
    {
        EditorGUILayout.LabelField("Conditions");
        for (byte i = 0; i < _conditions.Count; ++i)
        {
            StageTrigger.ConditionInfo con = _conditions[i];

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField($"Index {i}:", GUILayout.MaxWidth(100.0f));
            EditorGUILayout.LabelField("Condition Type", GUILayout.MaxWidth(90.0f));
            EditorGUI.BeginChangeCheck();
            con.Condition = (ConditionTypes)EditorGUILayout.EnumPopup(con.Condition, GUILayout.MaxWidth(130.0f));
            EditorGUILayout.EndHorizontal();

            for (byte j = 0; j < con.Targets.Count; ++j)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.Space(20.0f, false);
                EditorGUILayout.LabelField($"Target {j}:", GUILayout.MaxWidth(80.0f));
                con.Targets[j] = (GameObject)EditorGUILayout.ObjectField(con.Targets[j], typeof(GameObject), true);
                EditorGUILayout.EndHorizontal();
            }

            if (EditorGUI.EndChangeCheck())
            {
                _conditions[i] = con;
                EditorUtility.SetDirty(_trigger);
            }

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.Space(20.0f, false);
            if (GUILayout.Button("Add a target to end"))
            {
                Undo.RecordObject(_trigger, $"{_trigger.name}: MapTrigger condition target added");
                con.AddTarget();
                _conditions[i] = con;
                _trigger.Conditions = _conditions;
            }
            if (GUILayout.Button("Delete target at end"))
            {
                Undo.RecordObject(_trigger, $"{_trigger.name}: MapTrigger condition target deleted");
                con.DeleteTarget();
                _conditions[i] = con;
                _trigger.Conditions = _conditions;
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space(10.0f);
        }

        EditorGUILayout.Space(5.0f);

        if (GUILayout.Button("Add a Condition to end", GUILayout.MinHeight(30.0f)))
        {
            Undo.RecordObject(_trigger, $"{_trigger.name}: MapTrigger condition added");
            _trigger.AddConditions((byte)_conditions.Count);
            _conCurrent = (byte)(_conditions.Count - 1);
            _trigger.Conditions = _conditions;
        }

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Index", GUILayout.MaxWidth(40.0f));
        _conCurrent = (byte)EditorGUILayout.IntField(_conCurrent, GUILayout.MaxWidth(30.0f));
        if (_conCurrent <= _conditions.Count && GUILayout.Button("Add here", GUILayout.MaxWidth(100.0f)))
        {
            Undo.RecordObject(_trigger, $"{_trigger.name}: MapTrigger condition added");
            _trigger.AddConditions(_conCurrent);
            _conCurrent = (byte)(_conditions.Count - 1);
            _trigger.Conditions = _conditions;
        }
        if (_conCurrent < _conditions.Count && GUILayout.Button("Delete here", GUILayout.MaxWidth(100.0f)))
        {
            Undo.RecordObject(_trigger, $"{_trigger.name}: MapTrigger condition deleted");
            _trigger.DeleteConditions(_conCurrent);
            _conCurrent = (byte)(_conditions.Count - 1);
            EditorUtility.SetDirty(_trigger);
            _trigger.Conditions = _conditions;
        }

        EditorGUILayout.Space(20.0f);

        EditorGUILayout.LabelField("Move From", GUILayout.MaxWidth(70.0f));
        _conSwitchFrom = (byte)EditorGUILayout.IntField(_conSwitchFrom, GUILayout.MaxWidth(30.0f));
        EditorGUILayout.LabelField("To", GUILayout.MaxWidth(20.0f));
        _conSwitchTo = (byte)EditorGUILayout.IntField(_conSwitchTo, GUILayout.MaxWidth(30.0f));
        if (_conSwitchFrom < _conditions.Count && _conSwitchTo < _conditions.Count && GUILayout.Button("Move", GUILayout.MaxWidth(100.0f)))
        {
            Undo.RecordObject(_trigger, $"{_trigger.name}: MapTrigger condition moved");
            _trigger.MoveConditions(_conSwitchFrom, _conSwitchTo);
            _trigger.Conditions = _conditions;
        }
        EditorGUILayout.EndHorizontal();
    }


    private void ActionField()
    {
        EditorGUILayout.LabelField("Actions");
        for (byte i = 0; i < _actions.Count; ++i)
        {
            TriggerBase.TriggerAction act = _actions[i];

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField($"Index {i}: Action Type", GUILayout.MaxWidth(100.0f));
            EditorGUI.BeginChangeCheck();
            act.ActionType = (ActionTypes)EditorGUILayout.EnumPopup(act.ActionType, GUILayout.MaxWidth(130.0f));
            switch (act.ActionType)
            {
                case ActionTypes.Enable:
                case ActionTypes.Disable:
                case ActionTypes.Delete:
                case ActionTypes.PlayerTeleport:
                case ActionTypes.StartEventScene:
                    act.TargetObject = (GameObject)EditorGUILayout.ObjectField(act.TargetObject, typeof(GameObject), true);
                    break;

                case ActionTypes.LoadNextScene:
                    break;
            }
            if (EditorGUI.EndChangeCheck())
            {
                _actions[i] = act;
                EditorUtility.SetDirty(_trigger);
            }
            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.Space(5.0f);

        if (GUILayout.Button("Add an Action to end", GUILayout.MinHeight(30.0f)))
        {
            Undo.RecordObject(_trigger, $"{_trigger.name}: MapTrigger action added");
            _trigger.AddAction((byte)_actions.Count);
            _actCurrent = (byte)(_actions.Count - 1);
            _trigger.ActionsList = _actions;
        }

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Index", GUILayout.MaxWidth(40.0f));
        _actCurrent = (byte)EditorGUILayout.IntField(_actCurrent, GUILayout.MaxWidth(30.0f));
        if (_actCurrent <= _actions.Count && GUILayout.Button("Add here", GUILayout.MaxWidth(100.0f)))
        {
            Undo.RecordObject(_trigger, $"{_trigger.name}: MapTrigger action added");
            _trigger.AddAction(_actCurrent);
            _actCurrent = (byte)(_actions.Count - 1);
            _trigger.ActionsList = _actions;
        }
        if (_actCurrent < _actions.Count && GUILayout.Button("Delete here", GUILayout.MaxWidth(100.0f)))
        {
            Undo.RecordObject(_trigger, $"{_trigger.name}: MapTrigger action deleted");
            _trigger.DeleteAction(_actCurrent);
            _actCurrent = (byte)(_actions.Count - 1);
            _trigger.ActionsList = _actions;
        }

        EditorGUILayout.Space(10.0f);

        EditorGUILayout.LabelField("Move From", GUILayout.MaxWidth(70.0f));
        _actSwitchFrom = (byte)EditorGUILayout.IntField(_actSwitchFrom, GUILayout.MaxWidth(30.0f));
        EditorGUILayout.LabelField("To", GUILayout.MaxWidth(20.0f));
        _actSwitchTo = (byte)EditorGUILayout.IntField(_actSwitchTo, GUILayout.MaxWidth(30.0f));
        if (_actSwitchFrom < _actions.Count && _actSwitchTo < _actions.Count && GUILayout.Button("Move", GUILayout.MaxWidth(100.0f)))
        {
            Undo.RecordObject(_trigger, $"{_trigger.name}: MapTrigger action moved");
            _trigger.MoveAction(_actSwitchFrom, _actSwitchTo);
            _trigger.ActionsList = _actions;
        }
        EditorGUILayout.EndHorizontal();
    }


    public override void OnInspectorGUI()
    {
        ConditionField();
        EditorGUILayout.Space(50.0f);
        ActionField();
    }
}
