#if UNITY_EDITOR
using System.Collections.Generic;

public class TestStage : StageManagerBase
{
    protected override void Start()
    {
        // Get active character
        List<CharacterData.Character> temp = GameManager.Instance.GetActiveCharacterList();

        // Set character data
        List<Characters> characters = new List<Characters>();
        for (byte i = 0; i < Constants.HS_MAX_CHARACTER; ++i)
        {
            if (i == temp.Count)
            {
                break;
            }
            characters.Add(temp[i].Name);
        }

        // Send character data to Player controller
        HorizontalPlayerControl.Instance.SetCharacters(characters.ToArray());

        // Send character data to controller
        CanvasPlayController.Instance.SetCharacterButtons(characters.ToArray());
    }
}
#endif