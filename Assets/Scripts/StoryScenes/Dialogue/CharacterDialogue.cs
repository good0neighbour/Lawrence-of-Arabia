using System.Collections.Generic;
using UnityEngine;
using static Constants;

[CreateAssetMenu(fileName = "CharacterDialogue", menuName = "Lawrence of Arabia/CharacterDialogue")]
public class CharacterDialogue : DialogueBase
{
    /* ==================== Fields ==================== */

    [SerializeField] private Characters _name = Characters.Lawrence;
    [SerializeField] private DialogueScript[] _highTrust = null;
    [SerializeField] private DialogueScript[] _middleTrust = null;
    [SerializeField] private DialogueScript[] _lowTrust = null;



    /* ==================== Public Methods ==================== */

    public override List<DialogueScript.Dialogue> GetDialogueScript()
    {
        CharacterData.Character[] characters = GameManager.CharacterData.GetCharacterList();
        DialogueScript script = characters[(int)_name].RecentDialogue;
        if (script != null)
        {
            characters[(int)_name].RecentDialogue = null;
            return script.GetDialogueScript();
        }
        else
        {
            if (characters[(int)_name].Trust >= CHAR_HIGH_TRUST)
            {
                return _highTrust[Random.Range(0, _highTrust.Length)].GetDialogueScript();
            }
            else if (characters[(int)_name].Trust >= CHAR_MID_TRUST)
            {
                return _middleTrust[Random.Range(0, _middleTrust.Length)].GetDialogueScript();
            }
            else
            {
                return _lowTrust[Random.Range(0, _lowTrust.Length)].GetDialogueScript();
            }
        }
    }
}
