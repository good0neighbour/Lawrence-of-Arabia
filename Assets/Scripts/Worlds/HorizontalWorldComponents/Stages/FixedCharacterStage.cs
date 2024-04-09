using UnityEngine;

public class FixedCharacterStage : StageManagerBase
{
    /* ==================== Fields ==================== */

    [Header("Fixed Character")]
    [SerializeField] private Characters[] _fixedCharacters = null;



    /* ==================== Protected Methods ==================== */

    protected override void Start()
    {
        // Number of character limit
        if (_fixedCharacters.Length > Constants.HS_MAX_CHARACTER)
        {
            Characters[] temp = new Characters[Constants.HS_MAX_CHARACTER];
            for (byte i = 0; i < Constants.HS_MAX_CHARACTER; ++i)
            {
                temp[i] = _fixedCharacters[i];
            }
            _fixedCharacters = temp;
        }

        // Send character data to Player controller
        HorizontalPlayerControl.Instance.SetCharacters(_fixedCharacters);

        // Send character data to controller
        CanvasPlayController.Instance.SetCharacterButtons(_fixedCharacters);
    }
}
