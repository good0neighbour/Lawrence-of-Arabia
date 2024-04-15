using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEditor.EditorGUILayout;

[CustomEditor(typeof(StageTrigger))]
public class StageTriggerEditor : ActionEditorBase
{
    private StageTrigger _trigger = null;
    private List<StageTrigger.ConditionInfo> _conditions = null;
    private List<TriggerBase.TriggerAction> _actions = null;
    private byte _current = 0;
    private byte _switchFrom = 0;
    private byte _switchTo = 0;


    private void OnEnable()
    {
        _trigger = (StageTrigger)target;
        _actions = _trigger.GetActions();
        _conditions = _trigger.GetConditions();
        _current = (byte)(_conditions.Count - 1);
    }


    public override void OnInspectorGUI()
    {
        LabelField("Conditions");
        for (byte i = 0; i < _conditions.Count; ++i)
        {
            StageTrigger.ConditionInfo con = _conditions[i];

            BeginHorizontal();
            LabelField($"Index {i}:", GUILayout.MaxWidth(100.0f));
            LabelField("Condition Type", GUILayout.MaxWidth(90.0f));
            EditorGUI.BeginChangeCheck();
            con.Condition = (ConditionTypes)EnumPopup(con.Condition, GUILayout.MaxWidth(130.0f));
            EndHorizontal();

            for (byte j = 0; j < con.Targets.Count; ++j)
            {
                BeginHorizontal();
                Space(20.0f, false);
                LabelField($"Target {j}:", GUILayout.MaxWidth(80.0f));
                con.Targets[j] = (GameObject)ObjectField(con.Targets[j], typeof(GameObject), true);
                EndHorizontal();
            }

            if (EditorGUI.EndChangeCheck())
            {
                _conditions[i] = con;
                _trigger.SetConditions(_conditions.ToArray());
                EditorUtility.SetDirty(_trigger);
            }

            BeginHorizontal();
            Space(20.0f, false);
            if (GUILayout.Button("Add a target to end"))
            {
                Undo.RecordObject(_trigger, $"{_trigger.name}: MapTrigger condition target added");
                con.AddTarget();
                _conditions[i] = con;
                _trigger.SetConditions(_conditions.ToArray());
            }
            if (GUILayout.Button("Delete target at end"))
            {
                Undo.RecordObject(_trigger, $"{_trigger.name}: MapTrigger condition target deleted");
                con.DeleteTarget();
                _conditions[i] = con;
                _trigger.SetConditions(_conditions.ToArray());
            }
            EndHorizontal();

            Space(10.0f);
        }

        Space(5.0f);

        if (GUILayout.Button("Add a Condition to end", GUILayout.MinHeight(30.0f)))
        {
            Undo.RecordObject(_trigger, $"{_trigger.name}: MapTrigger condition added");
            StageTrigger.ConditionInfo temp = new StageTrigger.ConditionInfo();
            temp.Targets = new List<GameObject>();
            _conditions.Add(temp);
            _current = (byte)(_conditions.Count - 1);
            _trigger.SetConditions(_conditions.ToArray());
        }

        BeginHorizontal();
        LabelField("Index", GUILayout.MaxWidth(40.0f));
        _current = (byte)IntField(_current, GUILayout.MaxWidth(30.0f));
        if (_current <= _conditions.Count && GUILayout.Button("Add here", GUILayout.MaxWidth(100.0f)))
        {
            Undo.RecordObject(_trigger, $"{_trigger.name}: MapTrigger condition added");
            StageTrigger.ConditionInfo temp = new StageTrigger.ConditionInfo();
            temp.Targets = new List<GameObject>();
            _conditions.Insert(_current, temp);
            _current = (byte)(_conditions.Count - 1);
            _trigger.SetConditions(_conditions.ToArray());
        }
        if (_current < _conditions.Count && GUILayout.Button("Delete here", GUILayout.MaxWidth(100.0f)))
        {
            Undo.RecordObject(_trigger, $"{_trigger.name}: MapTrigger condition deleted");
            _conditions.RemoveAt(_current);
            _current = (byte)(_conditions.Count - 1);
            EditorUtility.SetDirty(_trigger);
            _trigger.SetConditions(_conditions.ToArray());
        }

        Space(20.0f);

        LabelField("Move From", GUILayout.MaxWidth(70.0f));
        _switchFrom = (byte)IntField(_switchFrom, GUILayout.MaxWidth(30.0f));
        LabelField("To", GUILayout.MaxWidth(20.0f));
        _switchTo = (byte)IntField(_switchTo, GUILayout.MaxWidth(30.0f));
        if (_switchFrom < _conditions.Count && _switchTo < _conditions.Count && GUILayout.Button("Move", GUILayout.MaxWidth(100.0f)))
        {
            Undo.RecordObject(_trigger, $"{_trigger.name}: MapTrigger condition moved");
            StageTrigger.ConditionInfo temp = _conditions[_switchFrom];
            _conditions.RemoveAt(_switchFrom);
            _conditions.Insert(_switchTo, temp);
            _trigger.SetConditions(_conditions.ToArray());
        }
        EndHorizontal();

        Space(50.0f);

        ActionField(_actions, _trigger, 5.0f);
    }
}
