using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using static UnityEditor.EditorGUILayout;

[CustomEditor(typeof(CutScene)), CanEditMultipleObjects]
public class CutSceneEditor : ListEditorBase
{
    private List<CutScene.CutSceneInfo> _actions = null;
    private CutScene _cutScene = null;
    private string _status = null;


    private void OnEnable()
    {
        _cutScene = (CutScene)target;
        _actions = _cutScene.GetActionsForEditor();
        Current = (byte)(_actions.Count - 1);
    }


    public override void OnInspectorGUI()
    {
        BeginHorizontal();
        LabelField("Current Language", GUILayout.MaxWidth(110.0f));
        EditorGUI.BeginChangeCheck();
        _cutScene.CurrentLanguage = (LanguageTypes)EnumPopup(_cutScene.CurrentLanguage, GUILayout.MaxWidth(100.0f));
        if (EditorGUI.EndChangeCheck())
        {
            EditorUtility.SetDirty(_cutScene);
            _status = null;
        }
        EndHorizontal();

        if (GUILayout.Button($"Create {_cutScene.CurrentLanguage.ToString()} Json", GUILayout.MaxWidth(210.0f)))
        {
            Language.LanguageJson lanJson = new Language.LanguageJson();
            List<string> text = new List<string>();
            foreach (CutScene.CutSceneInfo item in _actions)
            {
                text.Add(item.Text);
            }
            lanJson.Text = text.ToArray();
            File.WriteAllText($"{Application.dataPath}/Resources/Languages/{_cutScene.name}_{_cutScene.CurrentLanguage.ToString()}.Json", JsonUtility.ToJson(lanJson, true));
            AssetDatabase.Refresh();
            _status = $"Saved \"Resources/Languages/{_cutScene.name}_{_cutScene.CurrentLanguage.ToString()}.Json\"";
        }

        LabelField(_status);

        Space(20.0f);

        for (byte i = 0; i < _actions.Count; ++i)
        {
            CutScene.CutSceneInfo element = _actions[i];

            BeginHorizontal();
            LabelField($"Index {i.ToString()}", GUILayout.MaxWidth(120.0f));
            LabelField("Duration", GUILayout.MaxWidth(55.0f));
            EditorGUI.BeginChangeCheck();
            element.Duration = FloatField(element.Duration, GUILayout.MaxWidth(50.0f));
            Space(10.0f, false);
            LabelField("Image", GUILayout.MaxWidth(40.0f));
            element.Image = (Sprite)ObjectField(element.Image, typeof(Sprite), false, GUILayout.MaxWidth(120.0f));
            Space(10.0f, false);
            LabelField("Audio", GUILayout.MaxWidth(35.0f));
            element.Audio = (AudioClip)ObjectField(element.Audio, typeof(AudioClip), false);
            EndHorizontal();

            BeginHorizontal();
            if (element.Image != null)
            {
                GUILayout.Box(element.Image.texture, GUILayout.MaxWidth(120.0f), GUILayout.MaxHeight(60.0f), GUILayout.MinWidth(10.0f), GUILayout.MinHeight(5.0f));
                element.Text = TextField(element.Text, GUILayout.MinHeight(20.0f), GUILayout.MaxHeight(60.0f));
            }
            else
            {
                element.Text = TextField(element.Text, GUILayout.MinHeight(20.0f));
            }
            EndHorizontal();


            if (EditorGUI.EndChangeCheck())
            {
                _actions[i] = element;
                _cutScene.SetActions(_actions.ToArray());
                EditorUtility.SetDirty(_cutScene);
                _status = null;
            }

            Space(30.0f);
        }

        LabelField("Next Scene");
        EditorGUI.BeginChangeCheck();
        _cutScene.NextScene = TextField(_cutScene.NextScene, GUILayout.MinHeight(20.0f));
        if (EditorGUI.EndChangeCheck())
        {
            EditorUtility.SetDirty(_cutScene);
            _status = null;
        }

        Space(30.0f);

        ListEditor(_cutScene, _actions, () => _cutScene.SetActions(_actions.ToArray()), "CutScene");
    }
}
