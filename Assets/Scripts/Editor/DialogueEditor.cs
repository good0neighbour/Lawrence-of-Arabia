using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DialogueScript))]
public class DialogueEditor : Editor
{
    private List<DialogueScript.Dialogue> _diagolues = null;
    private DialogueScript _script = null;
    private byte _current = 0;


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
            EditorGUILayout.LabelField("Name", GUILayout.MaxWidth(40.0f));
            EditorGUI.BeginChangeCheck();
            element.Name = (Characters)EditorGUILayout.EnumPopup(element.Name, GUILayout.MaxWidth(100.0f));
            EditorGUILayout.Space(10.0f);
            EditorGUILayout.LabelField("Name Colour", GUILayout.MaxWidth(80.0f));
            element.NameColour = (NameColours)EditorGUILayout.EnumPopup(element.NameColour, GUILayout.MaxWidth(100.0f));
            EditorGUILayout.Space(10.0f);
            EditorGUILayout.LabelField("Image Direction", GUILayout.MaxWidth(100.0f));
            element.ImageDirection = (CharImageDir)EditorGUILayout.EnumPopup(element.ImageDirection, GUILayout.MaxWidth(100.0f));
            EditorGUILayout.EndHorizontal();

            switch (_diagolues[i].Name)
            {
                case Characters.Player:
                    ShowBranches(ref element);
                    break;

                default:
                    element.Text = EditorGUILayout.TextField(element.Text, GUILayout.MinHeight(20.0f));
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
            _script.AddDialogue((byte)_diagolues.Count);
            _current = (byte)(_diagolues.Count - 1);
            EditorUtility.SetDirty(_script);
        }

        EditorGUILayout.Space(20.0f);

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Index", GUILayout.MaxWidth(40.0f));
        _current = (byte)EditorGUILayout.IntField(_current, GUILayout.MaxWidth(30.0f));
        if (_current <= _diagolues.Count && GUILayout.Button("Add here", GUILayout.MaxWidth(100.0f)))
        {
            _script.AddDialogue(_current);
            _current = (byte)(_diagolues.Count - 1);
            EditorUtility.SetDirty(_script);
        }
        if (_current < _diagolues.Count && GUILayout.Button("Delete here", GUILayout.MaxWidth(100.0f)))
        {
            _script.DeleteDialogue(_current);
            _current = (byte)(_diagolues.Count - 1);
            EditorUtility.SetDirty(_script);
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
            element.AddBranch((byte)element.Branches.Count);
        }
        if (GUILayout.Button("Delete branch at end"))
        {
            element.DeleteBranch((byte)(element.Branches.Count - 1));
        }
        EditorGUILayout.EndHorizontal();
    }
}
