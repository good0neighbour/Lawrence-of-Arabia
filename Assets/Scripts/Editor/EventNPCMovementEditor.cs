using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EventNPCMovement))]
public class EventNPCMovementEditor : Editor
{
    private EventNPCMovement _character = null;
    private bool _flip = false;


    private void OnEnable()
    {
        _character = (EventNPCMovement)target;
        _flip = _character.Flip();
    }


    public override void OnInspectorGUI()
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Flip", GUILayout.MaxWidth(40.0f));
        EditorGUI.BeginChangeCheck();
        _flip = EditorGUILayout.Toggle(_flip);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(_character, "EventPNCMovement: Modify property");
            _character.Flip(_flip);
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space(10.0f);
        base.OnInspectorGUI();
    }
}
