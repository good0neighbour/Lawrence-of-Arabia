using System.Collections.Generic;
using UnityEngine;

public abstract class DialogueBase : ScriptableObject
{
    public abstract List<DialogueScript.Dialogue> GetDialogueScript();
}
