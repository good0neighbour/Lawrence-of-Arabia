using UnityEngine;

public class Intro0_Sneaking : StageManagerBase
{
    /* ==================== Fields ==================== */

    private Characters[] _fixedCharacters = { Characters.Lawrence };



    /* ==================== Public Methods ==================== */

    public override void StageClear()
    {
        // New dialogue
        GameManager.NPCData.GetNPCList()[(int)NPCs.BabyPhi].RecentDialogue
            = Resources.Load<DialogueScript>("Dialogues/BabyPhi/IntroducingPhi");

        // Game data
        GameManager.GameData.CurrentHeist = "WeaponHeist";
        GameManager.GameData.CurrentPreperation = 0;

        // Next scene
        LoadScene("CS_Intro1");
    }


    public override void StageReturn()
    {
        
    }


    public override void CustomAction(string action)
    {
        
    }



    /* ==================== Protected Methods ==================== */

    protected override void Start()
    {
        // Send character data to Player controller
        HorizontalPlayerControl.Instance.SetCharacters(_fixedCharacters);

        // Send character data to controller
        CanvasPlayController.Instance.SetCharacterButtons(_fixedCharacters);
    }
}
