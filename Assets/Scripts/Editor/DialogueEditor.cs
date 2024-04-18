using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;
using static UnityEditor.EditorGUILayout;

[CustomEditor(typeof(DialogueScript))]
public class DialogueEditor : ListEditorBase
{
    private List<DialogueScript.Dialogue> _diagolues = null;
    private DialogueScript _script = null;
    private string _status = null;


    private void OnEnable()
    {
        _script = (DialogueScript)target;
        _diagolues = _script.GetDialoguesForEditor();
        Current = (byte)(_diagolues.Count - 1);
    }


    public override void OnInspectorGUI()
    {
        BeginHorizontal();
        LabelField("Current Language", GUILayout.MaxWidth(110.0f));
        EditorGUI.BeginChangeCheck();
        _script.CurrentLanguage = (LanguageTypes)EnumPopup(_script.CurrentLanguage, GUILayout.MaxWidth(100.0f));
        if (EditorGUI.EndChangeCheck())
        {
            EditorUtility.SetDirty(_script);
            _status = null;
        }
        EndHorizontal();

        if (GUILayout.Button($"Create {_script.CurrentLanguage.ToString()} Json", GUILayout.MaxWidth(210.0f)))
        {
            Language.LanguageJson lanJson = new Language.LanguageJson();
            List<string> text = new List<string>();
            foreach (DialogueScript.Dialogue item in _diagolues)
            {
                switch (item.Type)
                {
                    case DialogueTypes.Selection:
                        foreach (DialogueScript.BranchDialogue branch in item.Branches)
                        {
                            text.Add(branch.Text);
                        }
                        break;

                    default:
                        text.Add(item.Text);
                        break;
                }
            }
            lanJson.Text = text.ToArray();
            File.WriteAllText($"{Application.dataPath}/Resources/Languages/{_script.name}_{_script.CurrentLanguage.ToString()}.Json", JsonUtility.ToJson(lanJson, true));
            AssetDatabase.Refresh();
            _status = $"Saved \"Resources/Languages/{_script.name}_{_script.CurrentLanguage.ToString()}.Json\"";
        }

        LabelField(_status);

        Space(30.0f);

        for (byte i = 0; i < _diagolues.Count; ++i)
        {
            DialogueScript.Dialogue element = _diagolues[i];

            LabelField($"Index {i.ToString()}");

            BeginHorizontal();
            LabelField("Type", GUILayout.MaxWidth(30.0f));
            EditorGUI.BeginChangeCheck();
            element.Type = (DialogueTypes)EnumPopup(element.Type, GUILayout.MaxWidth(100.0f));
            if (EditorGUI.EndChangeCheck())
            {
                _diagolues[i] = element;
                _script.SetDialogues(_diagolues.ToArray());
                EditorUtility.SetDirty(_script);
            }

            switch (_diagolues[i].Type)
            {
                case DialogueTypes.Selection:
                    #region Selection
                    Space(10.0f, false);
                    LabelField("Name", GUILayout.MaxWidth(40.0f));

                    EditorGUI.BeginChangeCheck();
                    element.Name = TextField(element.Name, GUILayout.MaxWidth(70.0f));
                    if (EditorGUI.EndChangeCheck())
                    {
                        element.Image = AssetDatabase.LoadAssetAtPath<Sprite>($"Assets/Textures/Characters/{element.Name}FullImage.PNG");
                        _diagolues[i] = element;
                        _script.SetDialogues(_diagolues.ToArray());
                        EditorUtility.SetDirty(_script);
                    }

                    Space(10.0f, false);
                    LabelField("Name Colour", GUILayout.MaxWidth(80.0f));
                    EditorGUI.BeginChangeCheck();
                    element.NameColour = (NameColours)EnumPopup(element.NameColour, GUILayout.MaxWidth(70.0f));
                    Space(10.0f, false);
                    if (element.Image == null)
                    {
                        if (string.IsNullOrEmpty(element.Name))
                        {
                            LabelField("This dialogue will function like narration.");
                        }
                        else
                        {
                            EditorStyles.label.normal.textColor = new Color(1.0f, 0.2f, 0.2f);
                            LabelField("『Character image isn't loaded. Input Name correctly.『");
                            EditorStyles.label.normal.textColor = Color.white;
                        }
                    }
                    else
                    {
                        LabelField("Image Direction", GUILayout.MaxWidth(95.0f));
                        element.ImageDirection = (CharImageDir)EnumPopup(element.ImageDirection, GUILayout.MaxWidth(60.0f));
                    }
                    EndHorizontal();
                    ShowBranches(ref element);
                    #endregion
                    break;

                case DialogueTypes.Narration:
                    #region Narration
                    EndHorizontal();

                    BeginHorizontal();
                    element.Text = TextArea(element.Text);
                    element.Audio = (AudioClip)ObjectField(element.Audio, typeof(AudioClip), false, GUILayout.MaxWidth(200.0f));
                    EndHorizontal();
                    #endregion
                    break;

                case DialogueTypes.Talk:
                    #region Talk
                    Space(10.0f, false);
                    LabelField("Name", GUILayout.MaxWidth(40.0f));

                    EditorGUI.BeginChangeCheck();
                    element.Name = TextField(element.Name, GUILayout.MaxWidth(70.0f));
                    if (EditorGUI.EndChangeCheck())
                    {
                        element.Image = AssetDatabase.LoadAssetAtPath<Sprite>($"Assets/Textures/Characters/{element.Name}FullImage.png");
                        _diagolues[i] = element;
                        _script.SetDialogues(_diagolues.ToArray());
                        EditorUtility.SetDirty(_script);
                    }

                    Space(10.0f, false);
                    LabelField("Name Colour", GUILayout.MaxWidth(80.0f));
                    EditorGUI.BeginChangeCheck();
                    element.NameColour = (NameColours)EnumPopup(element.NameColour, GUILayout.MaxWidth(70.0f));
                    Space(10.0f, false);
                    if (element.Image == null)
                    {
                        EditorStyles.label.normal.textColor = new Color(1.0f, 0.2f, 0.2f);
                        LabelField("『Character image isn't loaded. Input Name correctly.『");
                        EditorStyles.label.normal.textColor = Color.white;
                    }
                    else
                    {
                        LabelField("Image Direction", GUILayout.MaxWidth(95.0f));
                        element.ImageDirection = (CharImageDir)EnumPopup(element.ImageDirection, GUILayout.MaxWidth(60.0f));
                    }
                    EndHorizontal();

                    BeginHorizontal();
                    element.Text = TextArea(element.Text);
                    element.Audio = (AudioClip)ObjectField(element.Audio, typeof(AudioClip), false, GUILayout.MaxWidth(200.0f));
                    EndHorizontal();
                    #endregion
                    break;

                case DialogueTypes.TalkMaunally:
                    #region TalkMaunally
                    Space(10.0f, false);
                    LabelField("Name", GUILayout.MaxWidth(40.0f));

                    EditorGUI.BeginChangeCheck();
                    element.Name = TextField(element.Name, GUILayout.MaxWidth(70.0f));

                    Space(10.0f, false);
                    LabelField("Name Colour", GUILayout.MaxWidth(80.0f));
                    EditorGUI.BeginChangeCheck();
                    element.NameColour = (NameColours)EnumPopup(element.NameColour, GUILayout.MaxWidth(70.0f));

                    Space(10.0f, false);
                    element.Image = (Sprite)ObjectField(element.Image, typeof(Sprite), false, GUILayout.MaxWidth(200.0f));

                    Space(10.0f, false);
                    LabelField("Image Direction", GUILayout.MaxWidth(95.0f));
                    element.ImageDirection = (CharImageDir)EnumPopup(element.ImageDirection, GUILayout.MaxWidth(60.0f));
                    EndHorizontal();

                    BeginHorizontal();
                    element.Text = TextArea(element.Text);
                    element.Audio = (AudioClip)ObjectField(element.Audio, typeof(AudioClip), false, GUILayout.MaxWidth(200.0f));
                    EndHorizontal();
                    #endregion
                    break;
            }

            if (EditorGUI.EndChangeCheck())
            {
                _diagolues[i] = element;
                _script.SetDialogues(_diagolues.ToArray());
                EditorUtility.SetDirty(_script);
            }

            Space(30.0f);
        }

        ListEditor(_script, _diagolues, () => _script.SetDialogues(_diagolues.ToArray()), "dialogue");
    }


    private void ShowBranches(ref DialogueScript.Dialogue element)
    {
        List<DialogueScript.BranchDialogue> branches = element.GetBranches();

        for (byte i = 0; i < branches.Count; ++i)
        {
            DialogueScript.BranchDialogue temp = branches[i];
            BeginHorizontal();
            EditorGUI.BeginChangeCheck();
            temp.Text = TextArea(branches[i].Text);
            temp.Branch = (DialogueScript)ObjectField(branches[i].Branch, typeof(DialogueScript), false, GUILayout.MaxWidth(200.0f));
            temp.Audio = (AudioClip)ObjectField(branches[i].Audio, typeof(AudioClip), false, GUILayout.MaxWidth(200.0f));
            if (EditorGUI.EndChangeCheck())
            {
                branches[i] = temp;
                element.SetBranches(branches.ToArray());
                EditorUtility.SetDirty(_script);
            }
            EndHorizontal();
        }

        BeginHorizontal();
        if (GUILayout.Button("Add a branch to end"))
        {
            branches.Add(new DialogueScript.BranchDialogue());
            element.SetBranches(branches.ToArray());
            EditorUtility.SetDirty(_script);
        }
        if (GUILayout.Button("Delete branch at end"))
        {
            branches.RemoveAt(branches.Count - 1);
            element.SetBranches(branches.ToArray());
            EditorUtility.SetDirty(_script);
        }
        EndHorizontal();
    }
}
