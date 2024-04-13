using UnityEngine;

public class Intro0_Sneaking : StageManagerBase
{
    /* ==================== Fields ==================== */

    private Characters[] _fixedCharacters = { Characters.Lawrence };



    /* ==================== Public Methods ==================== */

    public override void StageClear()
    {
        GameManager.Instance.NPCData.GetNPCList()[(int)NPCs.BabyPhi].RecentDialogue
            = Resources.Load<DialogueScript>("Dialogues/BabyPhi/IntroducingPhi");
        LoadScene("CS_Intro1");
    }


    public override void StageReturn()
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
