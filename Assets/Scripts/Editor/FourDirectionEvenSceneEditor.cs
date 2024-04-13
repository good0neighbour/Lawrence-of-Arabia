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
            element.Action = (EventSceneActions)EditorGUILayout.EnumPopup(element.Action, GUILayout.MaxWidth(150.0f));
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

                case EventSceneActions.NPCLookAtTarget:
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Target NPC", GUILayout.MaxWidth(120.0f));
                    element.TargetObject = (GameObject)EditorGUILayout.ObjectField(element.TargetObject, typeof(GameObject), true);
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Look at", GUILayout.MaxWidth(120.0f));
                    element.TargetTransform = (Transform)EditorGUILayout.ObjectField(element.TargetTransform, typeof(Transform), true);
                    EditorGUILayout.EndHorizontal();
                    break;

                case EventSceneActions.NPCLookAtPlayer:
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Target NPC", GUILayout.MaxWidth(120.0f));
                    element.TargetObject = (GameObject)EditorGUILayout.ObjectField(element.TargetObject, typeof(GameObject), true);
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
                    element.DialogueScript = (DialogueBase)EditorGUILayout.ObjectField(element.DialogueScript, typeof(DialogueBase), true);
                    EditorGUILayout.EndHorizontal();
                    break;

                case EventSceneActions.CloseDialogue:
                case EventSceneActions.StageClear:
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
    }
}
