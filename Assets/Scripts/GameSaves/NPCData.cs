using System;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "NPCData", menuName = "Lawrence of Arabia/NPCData")]
public class NPCData : ScriptableObject
{
    /* ==================== Fields ==================== */

    [SerializeField] private NPC[] _NPCdata = null;



    /* ==================== Public Methods ==================== */

    public void NPCDataPreparation()
    {
        _NPCdata = _NPCdata.OrderBy(c => c.Name).ToArray();
    }


    public NPC[] GetNPCList()
    {
        return _NPCdata;
    }



    /* ==================== Struct ==================== */

    [Serializable]
    public struct NPC
    {
        public NPCs Name;
        public DialogueScript RecentDialogue;
    }
}
