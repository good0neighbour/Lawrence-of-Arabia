#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;
using static Constants;

public class TestStage : StageManagerBase
{
    public override void StageClear()
    {
        Debug.Log("Stage Clear");
    }


    public override void StageReturn()
    {
        Debug.Log("Return");
    }


    public override void CustomAction(string action)
    {
        Debug.Log($"Custom action: {action}");
    }


    protected override void Start()
    {
        // Get active character
        CharacterData.Character[] temp = GameManager.CharacterData.GetCharacterList();

        // Set character data
        List<Characters> characters = new List<Characters>();
        List<CharacterWeapons> weapons = new List<CharacterWeapons>();
        for (byte i = 0; i < HS_MAX_CHARACTER; ++i)
        {
            if (i == temp.Length)
            {
                break;
            }
            characters.Add(temp[i].Name);
            weapons.Add(CharacterWeapons.Pistol);
        }

        // Send character data to Player controller
        HorizontalPlayerControl.Instance.SetCharacters(
            characters.ToArray(),
            weapons.ToArray()
        );

        // Send character data to controller
        CanvasPlayController.Instance.SetCharacterButtons(characters.ToArray());
    }
}
#endif