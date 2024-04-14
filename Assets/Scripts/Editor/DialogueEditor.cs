using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;

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
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Current Language", GUILayout.MaxWidth(110.0f));
        EditorGUI.BeginChangeCheck();
        _script.CurrentLanguage = (LanguageTypes)EditorGUILayout.EnumPopup(_script.CurrentLanguage, GUILayout.MaxWidth(100.0f));
        if (EditorGUI.EndChangeCheck())
        {
            EditorUtility.SetDirty(_script);
            _status = null;
        }
        EditorGUILayout.EndHorizontal();

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

        EditorGUILayout.LabelField(_status);

        EditorGUILayout.Space(30.0f);

        for (byte i = 0; i < _diagolues.Count; ++i)
        {
            DialogueScript.Dialogue element = _diagolues[i];

            EditorGUILayout.LabelField($"Index {i.ToString()}");

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Type", GUILayout.MaxWidth(30.0f));
            EditorGUI.BeginChangeCheck();
            element.Type = (DialogueTypes)EditorGUILayout.EnumPopup(element.Type, GUILayout.MaxWidth(80.0f));
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
                    EditorGUILayout.Space(10.0f, false);
                    EditorGUILayout.LabelField("Name", GUILayout.MaxWidth(40.0f));

                    EditorGUI.BeginChangeCheck();
                    element.Name = EditorGUILayout.TextField(element.Name, GUILayout.MaxWidth(70.0f));
                    if (EditorGUI.EndChangeCheck())
                    {
                        element.Image = AssetDatabase.LoadAssetAtPath<Sprite>($"Assets/Textures/Characters/{element.Name}FullImage.PNG");
                        _diagolues[i] = element;
                        _script.SetDialogues(_diagolues.ToArray());
                        EditorUtility.SetDirty(_script);
                    }

                    EditorGUILayout.Space(10.0f, false);
                    EditorGUILayout.LabelField("Name Colour", GUILayout.MaxWidth(80.0f));
                    EditorGUI.BeginChangeCheck();
                    element.NameColour = (NameColours)EditorGUILayout.EnumPopup(element.NameColour, GUILayout.MaxWidth(70.0f));
                    EditorGUILayout.Space(10.0f, false);
                    if (element.Image == null)
                    {
                        if (string.IsNullOrEmpty(element.Name))
                        {
                            EditorGUILayout.LabelField("This dialogue will function like narration.");
                        }
                        else
                        {
                            EditorStyles.label.normal.textColor = new Color(1.0f, 0.2f, 0.2f);
                            EditorGUILayout.LabelField("『Character image isn't loaded. Input Name correctly.『");
                            EditorStyles.label.normal.textColor = Color.white;
                        }
                    }
                    else
                    {
                        EditorGUILayout.LabelField("Image Direction", GUILayout.MaxWidth(95.0f));
                        element.ImageDirection = (CharImageDir)EditorGUILayout.EnumPopup(element.ImageDirection, GUILayout.MaxWidth(60.0f));
                    }
                    EditorGUILayout.EndHorizontal();
                    ShowBranches(ref element);
                    #endregion
                    break;

                case DialogueTypes.Narration:
                    #region Narration
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    element.Text = EditorGUILayout.TextField(element.Text, GUILayout.MinHeight(20.0f));
                    element.Audio = (AudioClip)EditorGUILayout.ObjectField(element.Audio, typeof(AudioClip), false, GUILayout.MaxWidth(200.0f));
                    EditorGUILayout.EndHorizontal();
                    #endregion
                    break;

                case DialogueTypes.Talk:
                    #region Talk
                    EditorGUILayout.Space(10.0f, false);
                    EditorGUILayout.LabelField("Name", GUILayout.MaxWidth(40.0f));

                    EditorGUI.BeginChangeCheck();
                    element.Name = EditorGUILayout.TextField(element.Name, GUILayout.MaxWidth(70.0f));
                    if (EditorGUI.EndChangeCheck())
                    {
                        element.Image = AssetDatabase.LoadAssetAtPath<Sprite>($"Assets/Textures/Characters/{element.Name}FullImage.png");
                        _diagolues[i] = element;
                        _script.SetDialogues(_diagolues.ToArray());
                        EditorUtility.SetDirty(_script);
                    }

                    EditorGUILayout.Space(10.0f, false);
                    EditorGUILayout.LabelField("Name Colour", GUILayout.MaxWidth(80.0f));
                    EditorGUI.BeginChangeCheck();
                    element.NameColour = (NameColours)EditorGUILayout.EnumPopup(element.NameColour, GUILayout.MaxWidth(70.0f));
                    EditorGUILayout.Space(10.0f, false);
                    if (element.Image == null)
                    {
                        EditorStyles.label.normal.textColor = new Color(1.0f, 0.2f, 0.2f);
                        EditorGUILayout.LabelField("『Character image isn't loaded. Input Name correctly.『");
                        EditorStyles.label.normal.textColor = Color.white;
                    }
                    else
                    {
                        EditorGUILayout.LabelField("Image Direction", GUILayout.MaxWidth(95.0f));
                        element.ImageDirection = (CharImageDir)EditorGUILayout.EnumPopup(element.ImageDirection, GUILayout.MaxWidth(60.0f));
                    }
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    element.Text = EditorGUILayout.TextField(element.Text, GUILayout.MinHeight(20.0f));
                    element.Audio = (AudioClip)EditorGUILayout.ObjectField(element.Audio, typeof(AudioClip), false, GUILayout.MaxWidth(200.0f));
                    EditorGUILayout.EndHorizontal();
                    #endregion
                    break;

                case DialogueTypes.TalkMaunally:
                    #region TalkMaunally
                    EditorGUILayout.Space(10.0f, false);
                    EditorGUILayout.LabelField("Name", GUILayout.MaxWidth(40.0f));

                    EditorGUI.BeginChangeCheck();
                    element.Name = EditorGUILayout.TextField(element.Name, GUILayout.MaxWidth(70.0f));

                    EditorGUILayout.Space(10.0f, false);
                    EditorGUILayout.LabelField("Name Colour", GUILayout.MaxWidth(80.0f));
                    EditorGUI.BeginChangeCheck();
                    element.NameColour = (NameColours)EditorGUILayout.EnumPopup(element.NameColour, GUILayout.MaxWidth(70.0f));
                    EditorGUILayout.Space(10.0f, false);
                    if (element.Image == null)
                    {
                        EditorStyles.label.normal.textColor = new Color(1.0f, 0.2f, 0.2f);
                        EditorGUILayout.LabelField("『Character image isn't loaded. Input Name correctly.『");
                        EditorStyles.label.normal.textColor = Color.white;
                    }
                    else
                    {
                        EditorGUILayout.LabelField("Image Direction", GUILayout.MaxWidth(95.0f));
                        element.ImageDirection = (CharImageDir)EditorGUILayout.EnumPopup(element.ImageDirection, GUILayout.MaxWidth(60.0f));
                    }

                    EditorGUILayout.Space(10.0f, false);
                    element.Image = (Sprite)EditorGUILayout.ObjectField(element.Image, typeof(Sprite), false, GUILayout.MaxWidth(200.0f));
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    element.Text = EditorGUILayout.TextField(element.Text, GUILayout.MinHeight(20.0f));
                    element.Audio = (AudioClip)EditorGUILayout.ObjectField(element.Audio, typeof(AudioClip), false, GUILayout.MaxWidth(200.0f));
                    EditorGUILayout.EndHorizontal();
                    #endregion
                    break;
            }

            if (EditorGUI.EndChangeCheck())
            {
                _diagolues[i] = element;
                _script.SetDialogues(_diagolues.ToArray());
                EditorUtility.SetDirty(_script);
            }

            EditorGUILayout.Space(30.0f);
        }

        ListEditor(_script, _diagolues, () => _script.SetDialogues(_diagolues.ToArray()), "dialogue");
    }


    private void ShowBranches(ref DialogueScript.Dialogue element)
    {
        List<DialogueScript.BranchDialogue> branches = element.GetBranches();
        for (byte i = 0; i < branches.Count; ++i)
        {
            DialogueScript.BranchDialogue temp = branches[i];
            EditorGUILayout.BeginHorizontal();
            EditorGUI.BeginChangeCheck();
            temp.Text = EditorGUILayout.TextField(branches[i].Text, GUILayout.MinHeight(20.0f));
            temp.Branch = (DialogueScript)EditorGUILayout.ObjectField(branches[i].Branch, typeof(DialogueScript), false, GUILayout.MaxWidth(200.0f));
            temp.Audio = (AudioClip)EditorGUILayout.ObjectField(branches[i].Audio, typeof(AudioClip), false, GUILayout.MaxWidth(200.0f));
            if (EditorGUI.EndChangeCheck())
            {
                branches[i] = temp;
                element.SetBranches(branches.ToArray());
                EditorUtility.SetDirty(_script);
            }
            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.BeginHorizontal();
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
        EditorGUILayout.EndHorizontal();
    }
}
