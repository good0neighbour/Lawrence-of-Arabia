using System.Collections.Generic;
using UnityEngine;

public class HorizontalTestStage : StageManager
{
    protected override void Start()
    {
        CharacterData data = Resources.Load<CharacterData>("CharacterData");
        data.CharacterDataPreparation();

        List<CharacterData.Character> characters = new List<CharacterData.Character>();
        List<CharacterData.Character> temp = data.GetActiveCharacterList();
        for (byte i = 0; i < Constants.HS_MAX_CHARACTER; ++i)
        {
            if (i == temp.Count)
            {
                break;
            }
            characters.Add(temp[i]);
        }
        HorizontalPlayerControl.Instance.SetCharacters(characters.ToArray());
    }
}
