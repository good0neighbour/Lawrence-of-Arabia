using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FourDirectionEventScene))]
public class FourDirectionEventSceneEditor : ListEditorBase
{
    private List<EventSceneBase.EventSceneAction> _actions = null;
    private FourDirectionEventScene _scene = null;


    private void OnEnable()
    {
        _scene = (FourDirectionEventScene)target;
        _actions = _scene.GetActions();
        Current = (byte)(_actions.Count - 1);
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
            element.Action = (EventSceneActions)EditorGUILayout.EnumPopup(element.Action, GUILayout.MaxWidth(120.0f));
            EditorGUILayout.EndHorizontal();

            switch (element.Action)
            {
                case EventSceneActions.CameraMove:
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Target Position", GUILayout.MaxWidth(120.0f));
                    element.TargetTransform = (Transform)EditorGUILayout.ObjectField(element.TargetTransform, typeof(Transform), true);
                    EditorGUILayout.EndHorizontal();
                    break;

                case EventSceneActions.NPCMove:
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Target NPC", GUILayout.MaxWidth(120.0f));
                    element.TargetObject = (GameObject)EditorGUILayout.ObjectField(element.TargetObject, typeof(GameObject), true);
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Target Position", GUILayout.MaxWidth(120.0f));
                    element.TargetTransform = (Transform)EditorGUILayout.ObjectField(element.TargetTransform, typeof(Transform), true);
                    EditorGUILayout.EndHorizontal();
                    break;

                case EventSceneActions.NPCJump:
                    EditorGUILayout.LabelField("Cannot jump in FourDirection map.");
                    break;

                case EventSceneActions.NPCLookAt:
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Target NPC", GUILayout.MaxWidth(120.0f));
                    element.TargetObject = (GameObject)EditorGUILayout.ObjectField(element.TargetObject, typeof(GameObject), true);
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Look at", GUILayout.MaxWidth(120.0f));
                    element.TargetTransform = (Transform)EditorGUILayout.ObjectField(element.TargetTransform, typeof(Transform), true);
                    EditorGUILayout.EndHorizontal();
                    break;

                case EventSceneActions.PlayerLookAt:
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Look at", GUILayout.MaxWidth(120.0f));
                    element.TargetTransform = (Transform)EditorGUILayout.ObjectField(element.TargetTransform, typeof(Transform), true);
                    EditorGUILayout.EndHorizontal();
                    break;

                case EventSceneActions.Enable:
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Target Object", GUILayout.MaxWidth(120.0f));
                    element.TargetObject = (GameObject)EditorGUILayout.ObjectField(element.TargetObject, typeof(GameObject), true);
                    EditorGUILayout.EndHorizontal();
                    break;

                case EventSceneActions.Disable:
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Target Object", GUILayout.MaxWidth(120.0f));
                    element.TargetObject = (GameObject)EditorGUILayout.ObjectField(element.TargetObject, typeof(GameObject), true);
                    EditorGUILayout.EndHorizontal();
                    break;

                case EventSceneActions.Destroy:
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Target Object", GUILayout.MaxWidth(120.0f));
                    element.TargetObject = (GameObject)EditorGUILayout.ObjectField(element.TargetObject, typeof(GameObject), true);
                    EditorGUILayout.EndHorizontal();
                    break;

                case EventSceneActions.StartDialogue:
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Dialogue Script", GUILayout.MaxWidth(120.0f));
                    element.DialogueScript = (DialogueScript)EditorGUILayout.ObjectField(element.DialogueScript, typeof(DialogueScript), true);
                    EditorGUILayout.EndHorizontal();
                    break;

                case EventSceneActions.CloseDialogue:
                case EventSceneActions.LoadNextScene:
                    break;
            }

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Duration", GUILayout.MaxWidth(120.0f));
            element.Duration = EditorGUILayout.FloatField(element.Duration);
            EditorGUILayout.EndHorizontal();

            if (EditorGUI.EndChangeCheck())
            {
                _actions[i] = element;
                _scene.SetActions(_actions.ToArray());
                EditorUtility.SetDirty(_scene);
            }

            EditorGUILayout.Space(20.0f);
        }

        ListEditor(_scene, _actions, () => _scene.SetActions(_actions.ToArray()), "EventScene action");

        //if (GUILayout.Button("Add an Action to end", GUILayout.MinHeight(30.0f)))
        //{
        //    Undo.RecordObject(_scene, $"{_scene.name}: EventScene action added");
        //    _actions.Add(new EventSceneBase.EventSceneAction());
        //    _current = (byte)(_actions.Count - 1);
        //    _scene.SetActions(_actions.ToArray());
        //}

        //EditorGUILayout.Space(20.0f);

        //EditorGUILayout.BeginHorizontal();
        //EditorGUILayout.LabelField("Index", GUILayout.MaxWidth(40.0f));
        //_current = (byte)EditorGUILayout.IntField(_current, GUILayout.MaxWidth(30.0f));
        //if (_current <= _actions.Count && GUILayout.Button("Add here", GUILayout.MaxWidth(100.0f)))
        //{
        //    Undo.RecordObject(_scene, $"{_scene.name}: EventScene action added");
        //    _actions.Insert(_current, new EventSceneBase.EventSceneAction());
        //    _current = (byte)(_actions.Count - 1);
        //    _scene.SetActions(_actions.ToArray());
        //}
        //if (_current < _actions.Count && GUILayout.Button("Delete here", GUILayout.MaxWidth(100.0f)))
        //{
        //    Undo.RecordObject(_scene, $"{_scene.name}: EventScene action deleted");
        //    _actions.RemoveAt(_current);
        //    _current = (byte)(_actions.Count - 1);
        //    _scene.SetActions(_actions.ToArray());
        //}

        //EditorGUILayout.Space(20.0f);

        //EditorGUILayout.LabelField("Move From", GUILayout.MaxWidth(70.0f));
        //_switchFrom = (byte)EditorGUILayout.IntField(_switchFrom, GUILayout.MaxWidth(30.0f));
        //EditorGUILayout.LabelField("To", GUILayout.MaxWidth(20.0f));
        //_switchTo = (byte)EditorGUILayout.IntField(_switchTo, GUILayout.MaxWidth(30.0f));
        //if (_switchFrom < _actions.Count && _switchTo < _actions.Count && GUILayout.Button("Move", GUILayout.MaxWidth(100.0f)))
        //{
        //    Undo.RecordObject(_scene, $"{_scene.name}: EventScene action moved");
        //    EventSceneBase.EventSceneAction temp = _actions[_switchFrom];
        //    _actions.RemoveAt(_switchFrom);
        //    _actions.Insert(_switchTo, temp);
        //    _scene.SetActions(_actions.ToArray());
        //}
        //EditorGUILayout.EndHorizontal();
    }
}
