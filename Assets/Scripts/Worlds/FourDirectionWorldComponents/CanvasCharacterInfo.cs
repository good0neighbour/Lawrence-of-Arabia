using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class CanvasCharacterInfo : MonoBehaviour
{
    /* ==================== Fields ==================== */

    [SerializeField] private Image _charListImages = null;
    [SerializeField] private Image _charBackground = null;
    [SerializeField] private Image _charImage = null;
    [SerializeField] private TextMeshProUGUI _textLeft = null;
    [SerializeField] private TextMeshProUGUI _textRight = null;
    CharacterData.Character[] _characters = null;
    private byte _curChar = byte.MaxValue;



    /* ==================== Public Methods ==================== */

    public void BurronBack()
    {
        gameObject.SetActive(false);
        TownManagerBase.Instance.PauseGame(false);
        _charBackground.gameObject.SetActive(false);
        _curChar = byte.MaxValue;
        _textLeft.text = null;
        _textRight.text = null;
    }


    public void ButtonCharacterList()
    {
        if (_curChar == byte.MaxValue)
        {
            _charBackground.gameObject.SetActive(true);
        }

        // Selected character
        _curChar = (byte)EventSystem.current.currentSelectedGameObject.transform.GetSiblingIndex();

        // Character image
        _charBackground.sprite = _characters[_curChar].BackgroundImage;
        _charImage.sprite = _characters[_curChar].FullImage;

        // Character information
        _textLeft.text = $"{_characters[_curChar].Name.ToString()}\n{_characters[_curChar].Type.ToString()} Character\n{_characters[_curChar].Information}";

        // Character abilities
        _textRight.text = $"Health {_characters[_curChar].CurHealth.ToString()}\nArmor {_characters[_curChar].CurArmor.ToString()}\nAdditional Damage {_characters[_curChar].CurDamage.ToString()}\nAdditional Range {_characters[_curChar].CurRange.ToString()}";
    }


    public void ButtonLevelUp()
    {
        if (GameManager.GameData.LevelAvailable > 0)
        {
            GameManager.CharacterData.CharacterLevelUp(_characters[_curChar].Name);
            --GameManager.GameData.LevelAvailable;
        }
    }



    /* ==================== Private Methods ==================== */

    private void Awake()
    {
        gameObject.SetActive(false);

        // Get characters
        _characters = GameManager.Instance.GetCharactersGot();

        // Set character buttons
        GameObject charBtn = _charListImages.transform.parent.gameObject;
        Transform parent = charBtn.transform.parent;
        _charListImages.sprite = _characters[0].ProfileImage;
        for (byte i = 1; i < _characters.Length; ++i)
        {
            Image image = Instantiate(charBtn, parent).GetComponent<Image>();
            image.transform.GetChild(0).GetComponent<Image>().sprite = _characters[i].ProfileImage;
        }
    }


    private void OnEnable()
    {
        TownManagerBase.Instance.PauseGame(true);
    }
}
