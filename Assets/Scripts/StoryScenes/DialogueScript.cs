using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueScript", menuName = "Lawrence of Arabia/DialogueScript")]
public class DialogueScript : ScriptableObject
{
    /* ==================== Fields ==================== */

    [SerializeField] private List<Dialogue> _dialogues = new List<Dialogue>();

#if UNITY_EDITOR
    public List<Dialogue> Dialogues
    {
        set
        {
            _dialogues = value;
        }
    }
#endif



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
        Dialogue temp = new Dialogue();
        temp.Branches = new List<BranchDialogue>();
        _dialogues.Insert(index, temp);
    }


    public void DeleteDialogue(byte index)
    {
        _dialogues.RemoveAt(index);
    }


    public List<Dialogue> GetDialogueScriptForEditor()
    {
        return _dialogues;
    }


    public void MoveDialogue(byte from, byte to)
    {
        Dialogue temp = _dialogues[from];
        _dialogues.RemoveAt(from);
        _dialogues.Insert(to, temp);
    }
#endif



    /* ==================== Struct ==================== */

    [Serializable]
    public struct Dialogue
    {
        public DialogueTypes Type;
        public string Name;
        public NameColours NameColour;
        public Sprite Image;
        public CharImageDir ImageDirection;
        public string Text;
        public AudioClip Audio;
        public List<BranchDialogue> Branches;


        public void AddBranch()
        {
            Branches.Add(new BranchDialogue());
        }


        public void DeleteBranch()
        {
            Branches.RemoveAt(Branches.Count - 1);
        }
    }



    [Serializable]
    public struct BranchDialogue
    {
        public string Text;
        public AudioClip Audio;
        public DialogueScript Branch;
    }
}
