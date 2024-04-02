using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using TMPro;

[CustomEditor(typeof(CutScene))]
public class CutSceneEditor : Editor
{
    private List<CutScene.CutSceneAction> _actions = null;
    private CutScene _scene = null;
    private byte _current = 0;
    private byte _switchFrom = 0;
    private byte _switchTo = 0;


    private void OnEnable()
    {
        _scene = (CutScene)target;
        _actions = _scene.GetActions();
        _current = (byte)(_actions.Count - 1);
    }


    public override void OnInspectorGUI()
    {
        for (byte i = 0; i < _actions.Count; ++i)
        {
            CutScene.CutSceneAction element = _actions[i];

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField($"Index {i.ToString()}", GUILayout.MaxWidth(45.0f), GUILayout.MinWidth(45.0f));
            EditorGUI.BeginChangeCheck();
            element.Action = (CutSceneActions)EditorGUILayout.EnumPopup(element.Action, GUILayout.MaxWidth(90.0f), GUILayout.MinWidth(90.0f));
            EditorGUILayout.Space(10.0f, false);

            switch (element.Action)
            {
                case CutSceneActions.FadeIn:
                case CutSceneActions.FadeOut:
                    EditorGUILayout.LabelField("Image", GUILayout.MaxWidth(80.0f));
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
                    element.Duration = EditorGUILayout.FloatField(element.Duration, GUILayout.MaxWidth(50.0f));
                    break;
            }

            if (EditorGUI.EndChangeCheck())
            {
                if (element.Action != CutSceneActions.Wait)
                {
                    element.Duration = 0.0f;
                }
                _actions[i] = element;
                EditorUtility.SetDirty(_scene);
            }

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space(5.0f);
        }

        if (GUILayout.Button("Add a dialogue to end", GUILayout.MinHeight(30.0f)))
        {
            _scene.AddAction((byte)_actions.Count);
            _current = (byte)(_actions.Count - 1);
        }

        EditorGUILayout.Space(20.0f);

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Index", GUILayout.MaxWidth(40.0f));
        _current = (byte)EditorGUILayout.IntField(_current, GUILayout.MaxWidth(30.0f));
        if (_current <= _actions.Count && GUILayout.Button("Add here", GUILayout.MaxWidth(100.0f)))
        {
            _scene.AddAction(_current);
            _current = (byte)(_actions.Count - 1);
            EditorUtility.SetDirty(_scene);
        }
        if (_current < _actions.Count && GUILayout.Button("Delete here", GUILayout.MaxWidth(100.0f)))
        {
            _scene.DeleteAction(_current);
            _current = (byte)(_actions.Count - 1);
            EditorUtility.SetDirty(_scene);
        }

        EditorGUILayout.Space(20.0f);

        EditorGUILayout.LabelField("Move From", GUILayout.MaxWidth(70.0f));
        _switchFrom = (byte)EditorGUILayout.IntField(_switchFrom, GUILayout.MaxWidth(30.0f));
        EditorGUILayout.LabelField("To", GUILayout.MaxWidth(20.0f));
        _switchTo = (byte)EditorGUILayout.IntField(_switchTo, GUILayout.MaxWidth(30.0f));
        if (_switchFrom < _actions.Count && _switchTo < _actions.Count && GUILayout.Button("Move", GUILayout.MaxWidth(100.0f)))
        {
            _scene.MoveAction(_switchFrom, _switchTo);
            EditorUtility.SetDirty(_scene);
        }
        EditorGUILayout.EndHorizontal();
    }
}
