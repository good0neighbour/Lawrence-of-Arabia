using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlayerTrigger))]
public class PlayerTriggerEditor : ListEditorBase
{
    private PlayerTrigger _trigger = null;
    private List<TriggerBase.TriggerAction> _actions = null;


    private void OnEnable()
    {
        _trigger = (PlayerTrigger)target;
        _actions = _trigger.GetActions();
        Current = (byte)(_actions.Count - 1);
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

                case ActionTypes.StageClear:
                    break;
            }
            if (EditorGUI.EndChangeCheck())
            {
                _actions[i] = act;
                _trigger.SetActions(_actions.ToArray());
            }
            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.Space(20.0f);

        ListEditor(_trigger, _actions, () => _trigger.SetActions(_actions.ToArray()), "MapTrigger action");
    }
}
