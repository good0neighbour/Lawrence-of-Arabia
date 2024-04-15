using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueScript", menuName = "Lawrence of Arabia/DialogueScript")]
public class DialogueScript : DialogueBase
{
    /* ==================== Fields ==================== */

    [SerializeField] private Dialogue[] _dialogues = new Dialogue[0];
    [SerializeField] private LanguageTypes _currentLanguage = LanguageTypes.English;

#if UNITY_EDITOR
    public LanguageTypes CurrentLanguage
    {
        get
        {
            return _currentLanguage;
        }
        set
        {
            _currentLanguage = value;
        }
    }
#endif



    /* ==================== Public Methods ==================== */

    public override List<Dialogue> GetDialogueScript()
    {
        List<Dialogue> result = new List<Dialogue>();
        if (_currentLanguage == GameManager.GameData.CurrentLanguage)
        {
            for (byte i = 0; i < _dialogues.Length; ++i)
            {
                result.Add(_dialogues[i]);
            }
        }
        else
        {
            for (byte i = 0; i < _dialogues.Length; ++i)
            {
                // New language
                _currentLanguage = GameManager.GameData.CurrentLanguage;

                // Load Json file
                Language.LanguageJson json = JsonUtility.FromJson<Language.LanguageJson>(Resources.Load($"Languages/{name}_{_currentLanguage.ToString()}").ToString());

                // Adopt language
                byte offset = 0;
                switch (_dialogues[i].Type)
                {
                    case DialogueTypes.Selection:
                        for (int j = 0; j < _dialogues[i].Branches.Length; j++)
                        {
                            _dialogues[i].Branches[j].Text = json.Text[i + offset];
                            ++offset;
                        }
                        break;

                    default:
                        _dialogues[i].Text = json.Text[i + offset];
                        break;
                }

                // Add to list
                result.Add(_dialogues[i]);
            }
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
        public BranchDialogue[] Branches;


#if UNITY_EDITOR
        public List<BranchDialogue> GetBranches()
        {
            List<BranchDialogue> result = new List<BranchDialogue>();
            if (Branches == null)
            {
                Branches = new BranchDialogue[0];
            }
            else
            {
                foreach (BranchDialogue item in Branches)
                {
                    result.Add(item);
                }
            }
            return result;
        }


        public void SetBranches(BranchDialogue[] branches)
        {
            Branches = branches;
        }
#endif
    }



    [Serializable]
    public struct BranchDialogue
    {
        public string Text;
        public AudioClip Audio;
        public DialogueScript Branch;
    }
}
