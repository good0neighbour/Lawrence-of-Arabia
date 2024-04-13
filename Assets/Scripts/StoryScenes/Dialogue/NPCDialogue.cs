using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NPCDialogue", menuName = "Lawrence of Arabia/NPCDialogue")]
public class NPCDialogue : DialogueBase
{
    /* ==================== Fields ==================== */

    [SerializeField] private NPCs _name = NPCs.BabyPhi;
    [SerializeField] private DialogueScript[] _randomDialogues = null;



    /* ==================== Public Methods ==================== */

    public override List<DialogueScript.Dialogue> GetDialogueScript()
    {
        NPCData.NPC[] npcs = GameManager.Instance.NPCData.GetNPCList();
        DialogueScript script = npcs[(int)_name].RecentDialogue;
        if (script != null)
        {
            npcs[(int)_name].RecentDialogue = null;
            return script.GetDialogueScript();
        }
        else
        {
            return _randomDialogues[Random.Range(0, _randomDialogues.Length)].GetDialogueScript();
        }
    }
}
