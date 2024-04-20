using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class CanvasCharacterInfo : MonoBehaviour
{
    /* ==================== Fields ==================== */

    [SerializeField] private GameObject[] _charImageList = null;
    [SerializeField] private GameObject[] _nextImageButtons = null;
    [SerializeField] private Image _charListImages = null;
    [SerializeField] private Image _charBackground = null;
    [SerializeField] private Image _charImage = null;
    [SerializeField] private Image _charSprite = null;
    [SerializeField] private Image _charSpriteDead = null;
    [SerializeField] private TextMeshProUGUI _textLeft = null;
    [SerializeField] private TextMeshProUGUI _textRight = null;
    CharacterData.Character[] _characters = null;
    private byte _curChar = byte.MaxValue;
    private sbyte _curImg = 0;



    /* ==================== Public Methods ==================== */

    public void BurronBack()
    {
        gameObject.SetActive(false);
        TownManagerBase.Instance.PauseGame(false);
        _charBackground.gameObject.SetActive(false);
        foreach (GameObject image in _charImageList)
        {
            image.SetActive(false);
        }
        foreach (GameObject btn in _nextImageButtons)
        {
            btn.SetActive(false);
        }
        _curChar = byte.MaxValue;
        _curImg = 0;
        _textLeft.text = null;
        _textRight.text = null;
    }


    public void ButtonCharacterList()
    {
        if (_curChar == byte.MaxValue)
        {
            _charBackground.gameObject.SetActive(true);
            _charImageList[0].SetActive(true);
            foreach (GameObject btn in _nextImageButtons)
            {
                btn.SetActive(true);
            }
        }

        // Selected character
        _curChar = (byte)EventSystem.current.currentSelectedGameObject.transform.GetSiblingIndex();

        // Character image
        _charBackground.sprite = _characters[_curChar].BackgroundImage;
        _charImage.sprite = _characters[_curChar].FullImage;
        _charSprite.sprite = _characters[_curChar].Sprite;
        _charSpriteDead.sprite = _characters[_curChar].SpriteDead;

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


    public void ButtonCharNextImage(int dir)
    {
        _charImageList[_curImg].SetActive(false);
        _curImg = (sbyte)(_curImg + dir);
        if (_curImg < 0)
        {
            _curImg = (sbyte)(_charImageList.Length - 1);
        }
        else if (_curImg >= _charImageList.Length)
        {
            _curImg = 0;
        }
        _charImageList[_curImg].SetActive(true);
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
