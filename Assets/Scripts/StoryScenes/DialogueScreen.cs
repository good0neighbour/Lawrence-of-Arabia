using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class DialogueScreen : MonoBehaviour
{
    /* ==================== Fields ==================== */

    [SerializeField] private TextMeshProUGUI[] _dialogueTexts
        = new TextMeshProUGUI[Constants.TEXT_DISPLAY_NUM + 2];
    [SerializeField] private Transform[] _dialogueTransforms
        = new Transform[Constants.TEXT_DISPLAY_NUM + 2];
    [SerializeField] private Image _charImage = null;
    [SerializeField] private Transform _charImageTrans = null;
    [SerializeField] private Transform _buttonParent = null;
    private List<DialogueScript.Dialogue> _script = null;
    private List<ButtonRef> _buttons = new List<ButtonRef>();
    private EventSceneBase _curEvent = null;
    private CharImageDir _imageDir = CharImageDir.Left;
    private Sprite _prevChar = null;
    private byte _dialogueIndex = 0;
    private sbyte _animationState = 0;
    private sbyte _btnFocus = -1;
    private bool _playerStandby = false;

    public static DialogueScreen Instance
    {
        get;
        private set;
    }


    /* ==================== Private Methods ==================== */

    public void StartDialogue(DialogueScript script, EventSceneBase curEvent)
    {
        _curEvent = curEvent;
        _script = script.GetDialogueScript();
        gameObject.SetActive(true);
        enabled = true;
        NextDialogue();
        _animationState |= Constants.DIALOGUE_TEXT_MOVING;
    }


    public void DialogueButtonSelect()
    {
        ButtonFunctioning((sbyte)EventSystem.current.currentSelectedGameObject.transform.GetSiblingIndex());
    }


    public void CloseDialogueScreen()
    {
        gameObject.SetActive(false);
        _dialogueIndex = 0;
        foreach (TextMeshProUGUI text in _dialogueTexts)
        {
            text.text = null;
        }
        _charImage.sprite = null;
        _charImage.color = new Color();
        _prevChar = null;
    }


    public void DeleteInstance()
    {
        Instance = null;
    }



    /* ==================== Private Methods ==================== */

    private void ButtonFunctioning(sbyte btnNum)
    {
        DialogueScript.Dialogue current = _script[_dialogueIndex];

        // Player choice
        if (string.IsNullOrEmpty(current.Name))
        {
            _dialogueTexts[0].text = current.Text;
            _charImage.sprite = null;
            _charImage.color = new Color();
        }
        else
        {
            _dialogueTexts[0].text = $"<color={current.NameColour.ToString()}><b>{current.Name}</b></color>\n{current.Branches[btnNum].Text}";
            if (_prevChar != current.Image)
            {
                _imageDir = current.ImageDirection;
                _charImage.sprite = current.Image;
                _prevChar = current.Image;
                ImagePosInit();
            }
        }

        // Insert branch dialogue
        List<DialogueScript.Dialogue> branchDialogue = current.Branches[btnNum].Branch?.GetDialogueScript();
        if (branchDialogue != null)
        {
            for (sbyte i = (sbyte)(branchDialogue.Count - 1); i >= 0; --i)
            {
                _script.Insert(_dialogueIndex + 1, branchDialogue[i]);
            }
        }

        // Disable player dialogue buttons
        _buttonParent.gameObject.SetActive(false);
        foreach (ButtonRef btn in _buttons)
        {
            btn.ButtonObject.SetActive(false);
        }

        // Resume dialogue
        _playerStandby = false;
        _btnFocus = -1;
    }


    private void TextMoving()
    {
        // Text moving
        for (byte i = 0; i <= Constants.TEXT_DISPLAY_NUM; ++i)
        {
            Vector3 pos = _dialogueTransforms[i].localPosition;
            _dialogueTransforms[i].localPosition = new Vector3(pos.x, pos.y + Time.deltaTime * Constants.TEXT_SPEED, pos.z);
        }

        // Text colour
        float yPos = _dialogueTransforms[0].localPosition.y;
        float _textAlpha = (Constants.PRECAL_TEXT_ALPHA + yPos) / Constants.TEXT_SPACING;
        _dialogueTexts[0].color = new Color(1.0f, 1.0f, 1.0f, Mathf.Pow(_textAlpha, Constants.TEXT_FADE_POW));
        _dialogueTexts[Constants.TEXT_DISPLAY_NUM].color = new Color(1.0f, 1.0f, 1.0f, Mathf.Pow(1.0f - _textAlpha, Constants.TEXT_FADE_POW));

        // Text moving end
        if (yPos >= Constants.TEXT_OFFSET)
        {
            TextMovingEnd();
        }
    }


    private void TextMovingEnd()
    {
        // Clear text position
        for (sbyte i = Constants.TEXT_DISPLAY_NUM; i >= 0; --i)
        {
            _dialogueTransforms[i].localPosition = new Vector3(
                _dialogueTransforms[i].localPosition.x,
                i * Constants.TEXT_SPACING + Constants.TEXT_OFFSET,
                _dialogueTransforms[i].localPosition.z
            );
            _dialogueTransforms[i + 1] = _dialogueTransforms[i];
            _dialogueTexts[i + 1] = _dialogueTexts[i];
        }

        // Clear text
        _dialogueTexts[0] = _dialogueTexts[Constants.TEXT_DISPLAY_NUM + 1];
        _dialogueTexts[0].color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        _dialogueTexts[1].color = new Color(1.0f, 1.0f, 1.0f, 1.0f);

        // Clear new text position
        _dialogueTransforms[0] = _dialogueTransforms[Constants.TEXT_DISPLAY_NUM + 1];
        _dialogueTransforms[0].localPosition = new Vector3(
            _dialogueTransforms[0].localPosition.x,
            -Constants.TEXT_SPACING + Constants.TEXT_OFFSET,
            _dialogueTransforms[0].localPosition.z
        );

        // Initialize Status
        _animationState &= Constants.DIALOGUE_TEXT_END;
    }


    private void ImagePosInit()
    {
        _animationState |= Constants.DIALOGUE_IMAGE_MOVING;
        _charImage.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);

        switch (_imageDir)
        {
            case CharImageDir.Left:
                _charImageTrans.localPosition = new Vector3(
                    Constants.IMAGE_LEFT_POS_START,
                    _charImageTrans.localPosition.y,
                    0.0f
                );
                return;

            case CharImageDir.Right:
                _charImageTrans.localPosition = new Vector3(
                    Constants.IMAGE_RIGHT_POS_START,
                    _charImageTrans.localPosition.y,
                    0.0f
                );
                return;

            default:
                return;
        }
    }


    private void ImageMoving()
    {
        Vector2 pos = new Vector2(_charImageTrans.localPosition.x, _charImageTrans.localPosition.y);

        switch (_imageDir)
        {
            case CharImageDir.Left:
                pos.x += Constants.IMAGE_SPEED * Time.deltaTime;
                if (pos.x > Constants.IMAGE_LEFT_POS_END)
                {
                    ImageMovingEnd();
                }
                else
                {
                    _charImageTrans.localPosition = (Vector3)pos;
                    _charImage.color = new Color(
                        1.0f,
                        1.0f,
                        1.0f,
                        (pos.x - Constants.IMAGE_LEFT_POS_START) / Constants.IMAGE_MOVE_DIS
                    );
                }
                return;

            case CharImageDir.Right:
                pos.x -= Constants.IMAGE_SPEED * Time.deltaTime;
                if (pos.x < Constants.IMAGE_RIGHT_POS_END)
                {
                    ImageMovingEnd();
                }
                else
                {
                    _charImageTrans.localPosition = (Vector3)pos;
                    _charImage.color = new Color(
                        1.0f,
                        1.0f,
                        1.0f,
                        (Constants.IMAGE_RIGHT_POS_START - pos.x) / Constants.IMAGE_MOVE_DIS
                    );
                }
                return;

            default:
                return;
        }
    }


    private void ImageMovingEnd()
    {
        // Moving End
        switch (_imageDir)
        {
            case CharImageDir.Left:
                _charImageTrans.localPosition = new Vector3(
                    Constants.IMAGE_LEFT_POS_END,
                    _charImageTrans.localPosition.y,
                    0.0f
                );
                _charImage.color = Color.white;
                break;

            case CharImageDir.Right:
                _charImageTrans.localPosition = new Vector3(
                    Constants.IMAGE_RIGHT_POS_END,
                    _charImageTrans.localPosition.y,
                    0.0f
                );
                _charImage.color = Color.white;
                break;

            default:
                break;
        }

        // Initialize Status
        _animationState &= Constants.DIALOGUE_IMAGE_END;
    }


    private void NextDialogue()
    {
        DialogueScript.Dialogue current = _script[_dialogueIndex];

        switch (current.Type)
        {
            case DialogueTypes.Narration:
                _dialogueTexts[0].text = current.Text;
                _charImage.sprite = null;
                _charImage.color = new Color();
                break;

            case DialogueTypes.Selection:
                PlayerDialogueButtonShow(current);
                break;

            case DialogueTypes.Talk:
                _dialogueTexts[0].text = $"<color={current.NameColour.ToString()}><b>{current.Name}</b></color>\n{current.Text}";
                if (_prevChar != current.Image)
                {
                    _imageDir = current.ImageDirection;
                    _charImage.sprite = current.Image;
                    _prevChar = current.Image;
                    ImagePosInit();
                }
                break;
        }
    }


    private void PlayerDialogueButtonShow(DialogueScript.Dialogue current)
    {
        _playerStandby = true;
        _buttonParent.gameObject.SetActive(true);

        for (byte i = 0; i < current.Branches.Count; ++i)
        {
            if (i > _buttons.Count - 1)
            {
                GameObject newBtn = Instantiate(_buttons[0].ButtonObject, _buttonParent);
                _buttons.Add(new ButtonRef(
                    newBtn,
                    newBtn.GetComponent<Image>(),
                    newBtn.GetComponentInChildren<TextMeshProUGUI>()
                ));
            }
            _buttons[i].ButtonText.text = current.Branches[i].Text;
            _buttons[i].ButtonImage.color = Color.white;
            _buttons[i].ButtonObject.SetActive(true);
        }
    }


    private void SetButtonFocus(sbyte index)
    {
        for (byte i = 0; i < _buttons.Count; ++i)
        {
            if (i == index)
            {
                _buttons[i].ButtonImage.color = new Color(0.8f, 0.8f, 0.8f, 1.0f);
            }
            else
            {
                _buttons[i].ButtonImage.color = Color.white;
            }
        }
    }


    private void Awake()
    {
        Instance = this;
        GameObject btn = _buttonParent.GetChild(0).gameObject;
        _buttons.Add(new ButtonRef(
            btn,
            btn.GetComponent<Image>(),
            btn.GetComponentInChildren<TextMeshProUGUI>()
        ));
        gameObject.SetActive(false);
    }


    private void Update()
    {
        #region Player Selection Standby
        if (_playerStandby)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                --_btnFocus;
                if (_btnFocus < 0)
                {
                    _btnFocus = (sbyte)(_script[_dialogueIndex].Branches.Count - 1);
                }
                SetButtonFocus(_btnFocus);
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                ++_btnFocus;
                if (_btnFocus >= _script[_dialogueIndex].Branches.Count)
                {
                    _btnFocus = 0;
                }
                SetButtonFocus(_btnFocus);
            }
            else if (_btnFocus >= 0
                && (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.F)))
            {
                ButtonFunctioning(_btnFocus);
            }

            return;
        }
        #endregion

        if (Input.anyKeyDown)
        {
            if ((_animationState & Constants.DIALOGUE_TEXT_MOVING) > 0)
            {
                TextMovingEnd();
                if ((_animationState & Constants.DIALOGUE_IMAGE_MOVING) > 0)
                {
                    ImageMovingEnd();
                }
                return;
            }
            else
            {
                ++_dialogueIndex;
                if (_dialogueIndex < _script.Count)
                {
                    NextDialogue();
                    _animationState |= Constants.DIALOGUE_TEXT_MOVING;
                }
                else
                {
                    _curEvent.ResumeEventScene(true);
                    enabled = false;
                }
                return;
            }
        }

        if ((_animationState & Constants.DIALOGUE_TEXT_MOVING) > 0)
        {
            TextMoving();
        }
        if ((_animationState & Constants.DIALOGUE_IMAGE_MOVING) > 0)
        {
            ImageMoving();
        }
    }



    /* ==================== Struct ==================== */

    private struct ButtonRef
    {
        public GameObject ButtonObject;
        public Image ButtonImage;
        public TextMeshProUGUI ButtonText;


        public ButtonRef(GameObject btnObject, Image btnImage, TextMeshProUGUI btnText)
        {
            ButtonObject = btnObject;
            ButtonImage = btnImage;
            ButtonText = btnText;
        }
    }
}
