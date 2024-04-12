using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using TMPro;

[CustomEditor(typeof(CutScene))]
public class CutSceneEditor : ListEditorBase
{
    private List<CutScene.CutSceneAction> _actions = null;
    private CutScene _scene = null;


    private void OnEnable()
    {
        _scene = (CutScene)target;
        _actions = _scene.GetActions();
        Current = (byte)(_actions.Count - 1);
    }


    public override void OnInspectorGUI()
    {
        for (byte i = 0; i < _actions.Count; ++i)
        {
            CutScene.CutSceneAction element = _actions[i];

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField($"Index {i.ToString()}", GUILayout.MaxWidth(60.0f), GUILayout.MinWidth(60.0f));
            EditorGUI.BeginChangeCheck();
            element.Action = (CutSceneActions)EditorGUILayout.EnumPopup(element.Action, GUILayout.MaxWidth(90.0f), GUILayout.MinWidth(90.0f));
            EditorGUILayout.Space(10.0f, false);

            switch (element.Action)
            {
                case CutSceneActions.FadeIn:
                case CutSceneActions.FadeOut:
                    EditorGUILayout.LabelField("Target", GUILayout.MaxWidth(80.0f));
                    EditorGUILayout.LabelField("Image", GUILayout.MaxWidth(40.0f));
                    element.TargetImage = (Image)EditorGUILayout.ObjectField(element.TargetImage, typeof(Image), true);
                    EditorGUILayout.LabelField("Text", GUILayout.MaxWidth(30.0f));
                    element.TargetText = (TextMeshProUGUI)EditorGUILayout.ObjectField(element.TargetText, typeof(TextMeshProUGUI), true);
                    break;

                case CutSceneActions.Enable:
                case CutSceneActions.Disable:
                case CutSceneActions.Destroy:
                    EditorGUILayout.LabelField("Object", GUILayout.MaxWidth(80.0f));
                    element.TargetObject = (GameObject)EditorGUILayout.ObjectField(element.TargetObject, typeof(GameObject), true);
                    break;

                case CutSceneActions.LoadScene:
                    EditorGUILayout.LabelField("Scene Name", GUILayout.MaxWidth(80.0f));
                    element.SceneName = EditorGUILayout.TextField(element.SceneName, GUILayout.MinHeight(20.0f));
                    break;

                case CutSceneActions.Wait:
                    EditorGUILayout.LabelField("Duration", GUILayout.MaxWidth(80.0f));
                    element.Duration = EditorGUILayout.FloatField(element.Duration);
                    break;
            }

            if (EditorGUI.EndChangeCheck())
            {
                if (element.Action != CutSceneActions.Wait)
                {
                    element.Duration = 0.0f;
                }
                _actions[i] = element;
                _scene.SetActions(_actions.ToArray());
                EditorUtility.SetDirty(_scene);
            }

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space(5.0f);
        }

        ListEditor(_scene, _actions, () => _scene.SetActions(_actions.ToArray()), "CutScene action");
    }
}
