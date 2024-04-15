using System.Collections.Generic;
using UnityEditor;

[CustomEditor(typeof(FixedInteractableObject))]
public class FixedInteratableObjectEditor : ActionEditorBase
{
    private FixedInteractableObject _trigger = null;
    private List<TriggerBase.TriggerAction> _actions = null;


    private void OnEnable()
    {
        _trigger = (FixedInteractableObject)target;
        _actions = _trigger.GetActions();
        Current = (byte)(_actions.Count - 1);
    }


    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        EditorGUILayout.Space(10.0f);
        ActionField(_actions, _trigger, 20.0f);
    }
}
