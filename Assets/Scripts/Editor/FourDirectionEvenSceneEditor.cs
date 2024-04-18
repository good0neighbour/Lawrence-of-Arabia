using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using static UnityEditor.EditorGUILayout;

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

            LabelField($"Index {i.ToString()}");

            BeginHorizontal();
            LabelField("Action Type", GUILayout.MaxWidth(120.0f));
            EditorGUI.BeginChangeCheck();
            element.Action = (EventSceneActions)EnumPopup(element.Action, GUILayout.MaxWidth(150.0f));
            EndHorizontal();

            switch (element.Action)
            {
                case EventSceneActions.CameraMove:
                    BeginHorizontal();
                    LabelField("Target Position", GUILayout.MaxWidth(120.0f));
                    element.TargetTransform = (Transform)ObjectField(element.TargetTransform, typeof(Transform), true);
                    EndHorizontal();
                    break;

                case EventSceneActions.NPCMove:
                    BeginHorizontal();
                    LabelField("Target NPC", GUILayout.MaxWidth(120.0f));
                    element.TargetObject = (GameObject)ObjectField(element.TargetObject, typeof(GameObject), true);
                    EndHorizontal();
                    BeginHorizontal();
                    LabelField("Target Position", GUILayout.MaxWidth(120.0f));
                    element.TargetTransform = (Transform)ObjectField(element.TargetTransform, typeof(Transform), true);
                    EndHorizontal();
                    break;

                case EventSceneActions.NPCLookAtTarget:
                    BeginHorizontal();
                    LabelField("Target NPC", GUILayout.MaxWidth(120.0f));
                    element.TargetObject = (GameObject)ObjectField(element.TargetObject, typeof(GameObject), true);
                    EndHorizontal();
                    BeginHorizontal();
                    LabelField("Look at", GUILayout.MaxWidth(120.0f));
                    element.TargetTransform = (Transform)ObjectField(element.TargetTransform, typeof(Transform), true);
                    EndHorizontal();
                    break;

                case EventSceneActions.NPCLookAtPlayer:
                    BeginHorizontal();
                    LabelField("Target NPC", GUILayout.MaxWidth(120.0f));
                    element.TargetObject = (GameObject)ObjectField(element.TargetObject, typeof(GameObject), true);
                    EndHorizontal();
                    break;

                case EventSceneActions.PlayerLookAt:
                    BeginHorizontal();
                    LabelField("Look at", GUILayout.MaxWidth(120.0f));
                    element.TargetTransform = (Transform)ObjectField(element.TargetTransform, typeof(Transform), true);
                    EndHorizontal();
                    break;

                case EventSceneActions.Enable:
                    BeginHorizontal();
                    LabelField("Target Object", GUILayout.MaxWidth(120.0f));
                    element.TargetObject = (GameObject)ObjectField(element.TargetObject, typeof(GameObject), true);
                    EndHorizontal();
                    break;

                case EventSceneActions.Disable:
                    BeginHorizontal();
                    LabelField("Target Object", GUILayout.MaxWidth(120.0f));
                    element.TargetObject = (GameObject)ObjectField(element.TargetObject, typeof(GameObject), true);
                    EndHorizontal();
                    break;

                case EventSceneActions.Destroy:
                    BeginHorizontal();
                    LabelField("Target Object", GUILayout.MaxWidth(120.0f));
                    element.TargetObject = (GameObject)ObjectField(element.TargetObject, typeof(GameObject), true);
                    EndHorizontal();
                    break;

                case EventSceneActions.StartDialogue:
                    BeginHorizontal();
                    LabelField("Dialogue Script", GUILayout.MaxWidth(120.0f));
                    element.DialogueScript = (DialogueBase)ObjectField(element.DialogueScript, typeof(DialogueBase), true);
                    EndHorizontal();
                    break;

                case EventSceneActions.CloseDialogue:
                    break;

                case EventSceneActions.CustomAction:
                    BeginHorizontal();
                    LabelField("Custom Action", GUILayout.MaxWidth(120.0f));
                    element.Text = TextField(element.Text);
                    EndHorizontal();
                    break;

                case EventSceneActions.NPCJump:
                case EventSceneActions.StageClear:
                    LabelField("Cannot be used in FourDeirection stage.");
                    break;
            }

            BeginHorizontal();
            LabelField("Duration", GUILayout.MaxWidth(120.0f));
            element.Duration = FloatField(element.Duration);
            EndHorizontal();

            if (EditorGUI.EndChangeCheck())
            {
                _actions[i] = element;
                _scene.SetActions(_actions.ToArray());
                EditorUtility.SetDirty(_scene);
            }

            Space(20.0f);
        }

        BeginHorizontal();
        EditorGUI.BeginChangeCheck();
        _scene.ResumeAtTheEnd = Toggle(_scene.ResumeAtTheEnd, GUILayout.MaxWidth(20.0f));
        if (EditorGUI.EndChangeCheck())
        {
            EditorUtility.SetDirty(_scene);
        }
        LabelField("Resume game at the end of this event scene.");
        EndHorizontal();

        Space(20.0f);

        ListEditor(_scene, _actions, () => _scene.SetActions(_actions.ToArray()), "EventScene action");
    }
}
