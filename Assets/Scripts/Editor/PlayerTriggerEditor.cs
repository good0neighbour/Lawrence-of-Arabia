using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlayerTrigger))]
public class PlayerTriggerEditor : Editor
{
    private PlayerTrigger _trigger = null;
    private List<TriggerBase.TriggerAction> _actions = null;
    private byte _current = 0;
    private byte _switchFrom = 0;
    private byte _switchTo = 0;


    private void OnEnable()
    {
        _trigger = (PlayerTrigger)target;
        _actions = _trigger.GetActions();
        _current = (byte)(_actions.Count - 1);
    }


    public override void OnInspectorGUI()
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Trigger Type", GUILayout.MaxWidth(100.0f));
        Undo.RecordObject(_trigger, $"{_trigger.name}: PlayerTrigger property modified");
        _trigger.TriggerType = (TriggerTypes)EditorGUILayout.EnumPopup(_trigger.TriggerType, GUILayout.MaxWidth(130.0f));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space(10.0f);

        EditorGUILayout.LabelField("Actions");
        for (byte i = 0; i < _actions.Count; ++i)
        {
            TriggerBase.TriggerAction act = _actions[i];

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField($"Index {i}:", GUILayout.MaxWidth(100.0f));
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
                _trigger.ActionsList = _actions;
            }
            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.Space(20.0f);

        if (GUILayout.Button("Add an Action to end", GUILayout.MinHeight(30.0f)))
        {
            _trigger.AddAction((byte)_actions.Count);
            _current = (byte)(_actions.Count - 1);
            _trigger.ActionsList = _actions;
        }

        EditorGUILayout.Space(20.0f);

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Index", GUILayout.MaxWidth(40.0f));
        _current = (byte)EditorGUILayout.IntField(_current, GUILayout.MaxWidth(30.0f));
        if (_current <= _actions.Count && GUILayout.Button("Add here", GUILayout.MaxWidth(100.0f)))
        {
            _trigger.AddAction(_current);
            _current = (byte)(_actions.Count - 1);
            _trigger.ActionsList = _actions;
        }
        if (_current < _actions.Count && GUILayout.Button("Delete here", GUILayout.MaxWidth(100.0f)))
        {
            _trigger.DeleteAction(_current);
            _current = (byte)(_actions.Count - 1);
            _trigger.ActionsList = _actions;
        }

        EditorGUILayout.Space(20.0f);

        EditorGUILayout.LabelField("Move From", GUILayout.MaxWidth(70.0f));
        _switchFrom = (byte)EditorGUILayout.IntField(_switchFrom, GUILayout.MaxWidth(30.0f));
        EditorGUILayout.LabelField("To", GUILayout.MaxWidth(20.0f));
        _switchTo = (byte)EditorGUILayout.IntField(_switchTo, GUILayout.MaxWidth(30.0f));
        if (_switchFrom < _actions.Count && _switchTo < _actions.Count && GUILayout.Button("Move", GUILayout.MaxWidth(100.0f)))
        {
            _trigger.MoveAction(_switchFrom, _switchTo);
            _trigger.ActionsList = _actions;
        }
        EditorGUILayout.EndHorizontal();
    }
}
