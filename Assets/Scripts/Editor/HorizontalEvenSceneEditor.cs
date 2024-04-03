using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Codice.Client.BaseCommands;

[CustomEditor(typeof(HoriaontalEventScene))]
public class HoriaontalEventSceneEditor : Editor
{
    private List<EventSceneBase.EventSceneAction> _actions = null;
    private HoriaontalEventScene _scene = null;
    private byte _current = 0;
    private byte _switchFrom = 0;
    private byte _switchTo = 0;


    private void OnEnable()
    {
        _scene = (HoriaontalEventScene)target;
        _actions = _scene.GetActions();
        _current = (byte)(_actions.Count - 1);
    }


    public override void OnInspectorGUI()
    {
        for (byte i = 0; i < _actions.Count; ++i)
        {
            EventSceneBase.EventSceneAction element = _actions[i];

            EditorGUILayout.LabelField($"Index {i.ToString()}");

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Action Type", GUILayout.MaxWidth(120.0f));
            EditorGUI.BeginChangeCheck();
            element.Action = (EvenSceneActions)EditorGUILayout.EnumPopup(element.Action, GUILayout.MaxWidth(120.0f));
            EditorGUILayout.EndHorizontal();

            switch (element.Action)
            {
                case EvenSceneActions.CameraMove:
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Target Position", GUILayout.MaxWidth(120.0f));
                    element.TargetTransform = (Transform)EditorGUILayout.ObjectField(element.TargetTransform, typeof(Transform), true);
                    EditorGUILayout.EndHorizontal();
                    break;

                case EvenSceneActions.NPCMove:
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Target NPC", GUILayout.MaxWidth(120.0f));
                    element.TargetObject = (GameObject)EditorGUILayout.ObjectField(element.TargetObject, typeof(GameObject), true);
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Target X Position", GUILayout.MaxWidth(120.0f));
                    element.TargetTransform = (Transform)EditorGUILayout.ObjectField(element.TargetTransform, typeof(Transform), true);
                    EditorGUILayout.EndHorizontal();
                    break;

                case EvenSceneActions.NPCJump:
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Target NPC", GUILayout.MaxWidth(120.0f));
                    element.TargetObject = (GameObject)EditorGUILayout.ObjectField(element.TargetObject, typeof(GameObject), true);
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Target Y Position", GUILayout.MaxWidth(120.0f));
                    element.TargetTransform = (Transform)EditorGUILayout.ObjectField(element.TargetTransform, typeof(Transform), true);
                    EditorGUILayout.EndHorizontal();
                    break;

                case EvenSceneActions.Enable:
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Target Object", GUILayout.MaxWidth(120.0f));
                    element.TargetObject = (GameObject)EditorGUILayout.ObjectField(element.TargetObject, typeof(GameObject), true);
                    EditorGUILayout.EndHorizontal();
                    break;

                case EvenSceneActions.Disable:
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Target Object", GUILayout.MaxWidth(120.0f));
                    element.TargetObject = (GameObject)EditorGUILayout.ObjectField(element.TargetObject, typeof(GameObject), true);
                    EditorGUILayout.EndHorizontal();
                    break;

                case EvenSceneActions.Destroy:
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Target Object", GUILayout.MaxWidth(120.0f));
                    element.TargetObject = (GameObject)EditorGUILayout.ObjectField(element.TargetObject, typeof(GameObject), true);
                    EditorGUILayout.EndHorizontal();
                    break;

                case EvenSceneActions.StartDialogue:
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Dialogue Script", GUILayout.MaxWidth(120.0f));
                    element.DialogueScript = (DialogueScript)EditorGUILayout.ObjectField(element.DialogueScript, typeof(DialogueScript), true);
                    EditorGUILayout.EndHorizontal();
                    break;

                case EvenSceneActions.CloseDialogue:
                case EvenSceneActions.LoadNextScene:
                    break;
            }

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Duration", GUILayout.MaxWidth(120.0f));
            element.Duration = EditorGUILayout.FloatField(element.Duration);
            EditorGUILayout.EndHorizontal();

            if (EditorGUI.EndChangeCheck())
            {
                _actions[i] = element;
                EditorUtility.SetDirty(_scene);
            }

            EditorGUILayout.Space(20.0f);
        }

        if (GUILayout.Button("Add an Action to end", GUILayout.MinHeight(30.0f)))
        {
            Undo.RecordObject(_scene, $"{_scene.name}: EventScene action added");
            _scene.AddAction((byte)_actions.Count);
            _current = (byte)(_actions.Count - 1);
            _scene.ActionList = _actions;
        }

        EditorGUILayout.Space(20.0f);

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Index", GUILayout.MaxWidth(40.0f));
        _current = (byte)EditorGUILayout.IntField(_current, GUILayout.MaxWidth(30.0f));
        if (_current <= _actions.Count && GUILayout.Button("Add here", GUILayout.MaxWidth(100.0f)))
        {
            Undo.RecordObject(_scene, $"{_scene.name}: EventScene action added");
            _scene.AddAction(_current);
            _current = (byte)(_actions.Count - 1);
            _scene.ActionList = _actions;
        }
        if (_current < _actions.Count && GUILayout.Button("Delete here", GUILayout.MaxWidth(100.0f)))
        {
            Undo.RecordObject(_scene, $"{_scene.name}: EventScene action deleted");
            _scene.DeleteAction(_current);
            _current = (byte)(_actions.Count - 1);
            _scene.ActionList = _actions;
        }

        EditorGUILayout.Space(20.0f);

        EditorGUILayout.LabelField("Move From", GUILayout.MaxWidth(70.0f));
        _switchFrom = (byte)EditorGUILayout.IntField(_switchFrom, GUILayout.MaxWidth(30.0f));
        EditorGUILayout.LabelField("To", GUILayout.MaxWidth(20.0f));
        _switchTo = (byte)EditorGUILayout.IntField(_switchTo, GUILayout.MaxWidth(30.0f));
        if (_switchFrom < _actions.Count && _switchTo < _actions.Count && GUILayout.Button("Move", GUILayout.MaxWidth(100.0f)))
        {
            Undo.RecordObject(_scene, $"{_scene.name}: EventScene action moved");
            _scene.MoveAction(_switchFrom, _switchTo);
            _scene.ActionList = _actions;
        }
        EditorGUILayout.EndHorizontal();
    }
}
