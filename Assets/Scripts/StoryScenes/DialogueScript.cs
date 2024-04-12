using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueScript", menuName = "Lawrence of Arabia/DialogueScript")]
public class DialogueScript : ScriptableObject
{
    /* ==================== Fields ==================== */

    [SerializeField] private Dialogue[] _dialogues = null;



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
    public List<Dialogue> GetDialoguesForEditor()
    {
        List<Dialogue> result = new List<Dialogue>();
        foreach (Dialogue action in _dialogues)
        {
            result.Add(action);
        }
        return result;
    }


    public void SetDialogues(Dialogue[] actions)
    {
        _dialogues = actions;
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
