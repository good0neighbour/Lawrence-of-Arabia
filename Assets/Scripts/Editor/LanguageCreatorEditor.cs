using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using static UnityEditor.EditorGUILayout;

public class LanguageCreatorEditor : EditorWindow
{
    private static LanguageCreatorEditor _language = null;
    private DialogueScript _dialogue = null;
    private CutScene _cutscene = null;
    private Language.LanguageJson _lanJson;
    private TargetTypes _type = TargetTypes.Dialogue;
    private string fileName = null;
    private string _status = null;


    [MenuItem("Window/Lawrence of Arabia/Language Json Creator")]
    private static void Open()
    {
        if (null == _language)
        {
            _language = CreateInstance<LanguageCreatorEditor>();

            _language.position = new Rect(50.0f, 50.0f, 600.0f, 800.0f);
        }

        _language.Show();
    }


    private void OnGUI()
    {
        List<string> texts = new List<string>();

        LabelField("Target Type");
        _type = (TargetTypes)EnumPopup(_type, GUILayout.MaxWidth(200.0f));

        switch (_type)
        {
            case TargetTypes.Dialogue:
                EditorGUI.BeginChangeCheck();
                _dialogue = (DialogueScript)ObjectField(_dialogue, typeof(DialogueScript), false);
                if (EditorGUI.EndChangeCheck() && _dialogue != null)
                {
                    List<DialogueScript.Dialogue> dia = _dialogue.GetDialogueScript();
                    texts.Clear();
                    foreach (DialogueScript.Dialogue item in dia)
                    {
                        switch (item.Type)
                        {
                            case DialogueTypes.Selection:
                                foreach (DialogueScript.BranchDialogue bran in item.Branches)
                                {
                                    texts.Add(bran.Text);
                                }
                                break;

                            default:
                                texts.Add(item.Text);
                                break;
                        }
                    }
                    _lanJson.Text = texts.ToArray();
                    fileName = _dialogue.name;
                    _status = null;
                }
                break;

            case TargetTypes.CutScene:
                EditorGUI.BeginChangeCheck();
                _cutscene = (CutScene)ObjectField(_cutscene, typeof(CutScene), false);
                if (EditorGUI.EndChangeCheck() && _cutscene != null)
                {
                    CutScene.CutSceneInfo[] actions = _cutscene.GetCutSceneActions();
                    texts.Clear();
                    foreach (CutScene.CutSceneInfo item in actions)
                    {
                        texts.Add(item.Text);
                    }
                    _lanJson.Text = texts.ToArray();
                    fileName = _cutscene.name;
                    _status = null;
                }
                break;
        }

        if (_lanJson.Text == null || _lanJson.Text.Length == 0)
        {
            return;
        }

        if (GUILayout.Button("Create English Json"))
        {
            File.WriteAllText($"{Application.dataPath}/Resources/Languages/{fileName}_English.Json", JsonUtility.ToJson(_lanJson, true));
            AssetDatabase.Refresh();
            _status = "English Json file has been saved.";
        }

        LabelField(_status);
    }


    private enum TargetTypes
    {
        Dialogue,
        CutScene
    }
}
