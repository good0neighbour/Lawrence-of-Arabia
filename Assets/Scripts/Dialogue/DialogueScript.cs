using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueScript", menuName = "Lawrence of Arabia/DialogueScript")]
public class DialogueScript : ScriptableObject
{
    /* ==================== Fields ==================== */

    [SerializeField] private List<Dialogue> _dialogues = new List<Dialogue>();



    /* ==================== Public Methods ==================== */

    public List<Dialogue> GetDialogueScript()
    {
        List<Dialogue> result = new List<Dialogue>();
        foreach (var dialogue in _dialogues)
        {
            result.Add(dialogue);
        }
        return result;
    }


#if UNITY_EDITOR
    public void AddDialogue(byte index)
    {
        _dialogues.Insert(index, new Dialogue());
        Dialogue temp = _dialogues[index];
        temp.Branches = new List<BranchDialogue>();
        _dialogues[index] = temp;
    }


    public void DeleteDialogue(byte index)
    {
        _dialogues.RemoveAt(index);
    }


    public List<Dialogue> GetDialogueScriptForEditor()
    {
        return _dialogues;
    }
#endif



    /* ==================== Struct ==================== */

    [Serializable]
    public struct Dialogue
    {
        public Characters Name;
        public NameColours NameColour;
        public CharImageDir ImageDirection;
        public string Text;
        public List<BranchDialogue> Branches;


        public void AddBranch(byte index)
        {
            Branches.Insert(index, new BranchDialogue());
        }


        public void DeleteBranch(byte index)
        {
            Branches.RemoveAt(index);
        }
    }



    [Serializable]
    public struct BranchDialogue
    {
        public string Text;
        public DialogueScript Branch;
    }
}
