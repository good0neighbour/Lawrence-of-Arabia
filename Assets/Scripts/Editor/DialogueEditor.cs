using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DialogueScript))]
public class DialogueEditor : Editor
{
    private List<DialogueScript.Dialogue> _diagolues = null;
    private DialogueScript _script = null;
    private byte _current = 0;
    private byte _switchFrom = 0;
    private byte _switchTo = 0;


    private void OnEnable()
    {
        _script = (DialogueScript)target;
        _diagolues = _script.GetDialogueScriptForEditor();
        _current = (byte)(_diagolues.Count - 1);
    }


    public override void OnInspectorGUI()
    {
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
                EditorUtility.SetDirty(_script);
            }

            EditorGUILayout.Space(30.0f);
        }

        if (GUILayout.Button("Add a dialogue to end", GUILayout.MinHeight(30.0f)))
        {
            Undo.RecordObject(_script, $"{_script.name}: DialogueScript dialogue added");
            _script.AddDialogue((byte)_diagolues.Count);
            _current = (byte)(_diagolues.Count - 1);
            _script.Dialogues = _diagolues;
        }

        EditorGUILayout.Space(20.0f);

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Index", GUILayout.MaxWidth(40.0f));
        _current = (byte)EditorGUILayout.IntField(_current, GUILayout.MaxWidth(30.0f));
        if (_current <= _diagolues.Count && GUILayout.Button("Add here", GUILayout.MaxWidth(100.0f)))
        {
            Undo.RecordObject(_script, $"{_script.name}: DialogueScript dialogue added");
            _script.AddDialogue(_current);
            _current = (byte)(_diagolues.Count - 1);
            _script.Dialogues = _diagolues;
        }
        if (_current < _diagolues.Count && GUILayout.Button("Delete here", GUILayout.MaxWidth(100.0f)))
        {
            Undo.RecordObject(_script, $"{_script.name}: DialogueScript dialogue deleted");
            _script.DeleteDialogue(_current);
            _current = (byte)(_diagolues.Count - 1);
            _script.Dialogues = _diagolues;
        }

        EditorGUILayout.Space(20.0f);

        EditorGUILayout.LabelField("Move From", GUILayout.MaxWidth(70.0f));
        _switchFrom = (byte)EditorGUILayout.IntField(_switchFrom, GUILayout.MaxWidth(30.0f));
        EditorGUILayout.LabelField("To", GUILayout.MaxWidth(20.0f));
        _switchTo = (byte)EditorGUILayout.IntField(_switchTo, GUILayout.MaxWidth(30.0f));
        if (_switchFrom < _diagolues.Count && _switchTo < _diagolues.Count && GUILayout.Button("Move", GUILayout.MaxWidth(100.0f)))
        {
            Undo.RecordObject(_script, $"{_script.name}: DialogueScript dialogue moved");
            _script.MoveDialogue(_switchFrom, _switchTo);
            _script.Dialogues = _diagolues;
        }
        EditorGUILayout.EndHorizontal();
    }


    private void ShowBranches(ref DialogueScript.Dialogue element)
    {
        for (byte i = 0; i < element.Branches.Count; ++i)
        {
            DialogueScript.BranchDialogue temp = element.Branches[i];
            EditorGUILayout.BeginHorizontal();
            EditorGUI.BeginChangeCheck();
            temp.Text = EditorGUILayout.TextField(element.Branches[i].Text, GUILayout.MinHeight(20.0f));
            temp.Branch = (DialogueScript)EditorGUILayout.ObjectField(element.Branches[i].Branch, typeof(DialogueScript), false, GUILayout.MaxWidth(200.0f));
            temp.Audio = (AudioClip)EditorGUILayout.ObjectField(element.Audio, typeof(AudioClip), false, GUILayout.MaxWidth(200.0f));
            if (EditorGUI.EndChangeCheck())
            {
                element.Branches[i] = temp;
                EditorUtility.SetDirty(_script);
            }
            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Add a branch to end"))
        {
            element.AddBranch();
        }
        if (GUILayout.Button("Delete branch at end"))
        {
            element.DeleteBranch();
        }
        EditorGUILayout.EndHorizontal();
    }
}
