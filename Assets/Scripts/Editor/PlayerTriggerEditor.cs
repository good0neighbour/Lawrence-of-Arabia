using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEditor.EditorGUILayout;

[CustomEditor(typeof(PlayerTrigger))]
public class PlayerTriggerEditor : ActionEditorBase
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
        BeginHorizontal();
        LabelField("Trigger Type", GUILayout.MaxWidth(100.0f));
        Undo.RecordObject(_trigger, $"{_trigger.name}: PlayerTrigger property modified");
        _trigger.TriggerType = (TriggerTypes)EnumPopup(_trigger.TriggerType, GUILayout.MaxWidth(130.0f));
        EndHorizontal();

        Space(10.0f);

        ActionField(_actions, _trigger, 20.0f);
    }
}
