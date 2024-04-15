using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEditor.EditorGUILayout;

public abstract class ActionEditorBase : ListEditorBase
{
    protected void ActionField(List<TriggerBase.TriggerAction> _actions, TriggerBase _trigger, float space)
    {
        LabelField("Actions");
        for (byte i = 0; i < _actions.Count; ++i)
        {
            TriggerBase.TriggerAction act = _actions[i];

            BeginHorizontal();
            LabelField($"Index {i}: Action Type", GUILayout.MaxWidth(125.0f));
            EditorGUI.BeginChangeCheck();
            act.ActionType = (ActionTypes)EnumPopup(act.ActionType, GUILayout.MaxWidth(130.0f));
            switch (act.ActionType)
            {
                case ActionTypes.Enable:
                case ActionTypes.Disable:
                case ActionTypes.Delete:
                case ActionTypes.PlayerTeleport:
                case ActionTypes.StartEventScene:
                    act.TargetObject = (GameObject)ObjectField(act.TargetObject, typeof(GameObject), true);
                    break;

                case ActionTypes.StageClear:
                    break;

                case ActionTypes.CustomAction:
                    act.Text = TextField(act.Text);
                    break;
            }
            if (EditorGUI.EndChangeCheck())
            {
                _actions[i] = act;
                _trigger.SetActions(_actions.ToArray());
                EditorUtility.SetDirty(_trigger);
            }
            EndHorizontal();
        }

        Space(space);

        ListEditor(_trigger, _actions, () => _trigger.SetActions(_actions.ToArray()), "MapTrigger action");
    }
}
