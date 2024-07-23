using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static Constants;

public class CanvasPlayController : MonoBehaviour
{
    /* ==================== Fields ==================== */

    [SerializeField] private GameObject[] _buttons = null;
    [SerializeField] private CharBtnRef[] _characterBtns = null;
    private byte _curChar = 0;

    public static CanvasPlayController Instance
    {
        get;
        private set;
    }



    /* ==================== Public Methods ==================== */

    public void SetControllerActive(bool active)
    {
        gameObject.SetActive(active);
        UserControl.Instance.SetUserControlActive(active);
    }


    public void SetBtnActive(byte index, bool active)
    {
        _buttons[index].SetActive(active);
    }


    public void SetCharacterButtons(Characters[] characters)
    {
        // Get character list
        CharacterData.Character[] data = GameManager.CharacterData.GetCharacterList();

        // Set character Buttons
        for (byte i = 0; i < characters.Length; ++i)
        {
            _characterBtns[i].Button.gameObject.SetActive(true);
            _characterBtns[i].Button.Find("CharacterImage").GetComponent<Image>().sprite = data[(int)characters[i]].ProfileImage;
            _characterBtns[i].Button.Find("CharacterName").GetComponent<TextMeshProUGUI>().text = data[(int)characters[i]].Name.ToString();
            _characterBtns[i].HealthGage.fillAmount = 1.0f;
        }

        //Set default Character
        _characterBtns[0].Button.localScale = new Vector3(
            PLAYCON_SELECTED_CHAR_SCALE,
            PLAYCON_SELECTED_CHAR_SCALE,
            PLAYCON_SELECTED_CHAR_SCALE
        );
    }


    public void ButtonCharacter(int index)
    {
        HorizontalPlayerControl.Instance.CharacterChange((byte)index);
    }


    public void CharacterChange(byte index)
    {
        _characterBtns[_curChar].Button.localScale = Vector3.one;
        _curChar = index;
        _characterBtns[index].Button.localScale = new Vector3(
            PLAYCON_SELECTED_CHAR_SCALE,
            PLAYCON_SELECTED_CHAR_SCALE,
            PLAYCON_SELECTED_CHAR_SCALE
        );
    }


    public void SetCharacterHealthGage(byte index, float amount)
    {
        if (amount == 0.0f)
        {
            _characterBtns[index].HealthGage.fillAmount = 0.0f;
            _characterBtns[index].KIA.SetActive(true);
        }
        else
        {
            _characterBtns[index].HealthGage.fillAmount = amount;
        }
    }


    public void DeleteInstance()
    {
        Instance = null;
    }



    /* ==================== Private Methods ==================== */

    private void Awake()
    {
        Instance = this;
    }


    private void Start()
    {
        gameObject.SetActive(false);
    }



    /* ==================== Struct ==================== */

    [Serializable]
    private struct CharBtnRef
    {
        public Image HealthGage;
        public GameObject KIA;
        public Transform Button;
    }
}
