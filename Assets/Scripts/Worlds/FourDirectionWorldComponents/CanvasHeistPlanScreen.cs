using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static Constants;

public class CanvasHeistPlanScreen : MonoBehaviour
{
    /* ==================== Fields ==================== */

    [SerializeField] private PlanInfo[] _planButtons = null;
    [SerializeField] private Image[] _selectedImage = null;
    [SerializeField] private GameObject _planScreen = null;
    [SerializeField] private GameObject _charScreen = null;
    [SerializeField] private Image _charImage = null;
    [SerializeField] private RectTransform _scrollView = null;
    private List<Characters> _selectedChars = new List<Characters>();
    private CharacterData.Character[] _charList = null;
    private byte _current = 0;
    private float _timer = 1.0f;



    /* ==================== Public Methods ==================== */

    public void ButtonPlan(int index)
    {
        // Already done
        if ((GameManager.GameData.CurrentPreperation & 1 << index) > 0)
        {
            return;
        }

        // Character selection screen
        _current = (byte)index;
        _planScreen.SetActive(false);
        _charScreen.SetActive(true);
        _timer = 0.0f;
    }


    public void ButtonClose()
    {
       gameObject.SetActive(false);
        TownManagerBase.Instance.PauseGame(false);
    }


    public void ButtonCharScreenClose()
    {
        _planScreen.SetActive(true);
        _charScreen.SetActive(false);
        _selectedChars.Clear();
        foreach (Image image in _selectedImage)
        {
            image.transform.parent.gameObject.SetActive(false);
        }
        _scrollView.anchorMax = new Vector2(_scrollView.anchorMax.x, CHARSEL_MIN_ANCHOR);
    }


    public void ButtonChar()
    {
        // Get target character index
        byte index = (byte)EventSystem.current.currentSelectedGameObject.transform.GetSiblingIndex();

        // Is already selected
        for (byte i = 0; i < _selectedChars.Count; ++i)
        {
            if (_selectedChars[i] == _charList[index].Name)
            {
                _selectedChars.RemoveAt(i);
                for (byte j = i; j < HS_MAX_CHARACTER; ++j)
                {
                    if (j < _selectedChars.Count)
                    {
                        _selectedImage[j].sprite = _selectedImage[j + 1].sprite;
                    }
                    else
                    {
                        _selectedImage[j].transform.parent.gameObject.SetActive(false);
                    }
                }
                return;
            }
        }

        // Select
        if (_selectedChars.Count < HS_MAX_CHARACTER)
        {
            _selectedChars.Add(_charList[index].Name);
            _selectedImage[_selectedChars.Count - 1].sprite = _charList[index].ProfileImage;
            _selectedImage[_selectedChars.Count - 1].transform.parent.gameObject.SetActive(true);
        }
    }


    public void ButtonCharCancel(int index)
    {
        if (index < _selectedChars.Count)
        {
            _selectedChars.RemoveAt(index);
            for (byte j = (byte)index; j < HS_MAX_CHARACTER; ++j)
            {
                if (j < _selectedChars.Count)
                {
                    _selectedImage[j].sprite = _selectedImage[j + 1].sprite;
                }
                else
                {
                    _selectedImage[j].transform.parent.gameObject.SetActive(false);
                }
            }
        }
    }



    /* ==================== Private Methods ==================== */

    private void Awake()
    {
        // Set plan buttons
        for (byte i = 0; i < _planButtons.Length; ++i)
        {
            if ((GameManager.GameData.CurrentPreperation & 1 << _planButtons[i].Index) > 0)
            {
                if (_planButtons[i].Enable != null)
                {
                    foreach (GameObject enable in _planButtons[i].Enable)
                    {
                        enable.SetActive(true);
                    }
                }
                if (_planButtons[i].Destroy != null)
                {
                    foreach (GameObject destroy in _planButtons[i].Destroy)
                    {
                        Destroy(destroy);
                    }
                }
            }
        }

        // Set character buttons
        GameObject charBtn = _charImage.transform.parent.gameObject;
        Transform parent = charBtn.transform.parent;
        _charList = TownManagerBase.Instance.GetActiveCharList();
        _charImage.sprite = _charList[0].ButtonImage;
        for (byte i = 1; i < _charList.Length; ++i)
        {
            Instantiate(charBtn, parent).transform.GetChild(0).GetComponent<Image>().sprite = _charList[i].ButtonImage;
        }
    }


    private void Update()
    {
        if (_timer < 1.0f)
        {
            _timer += Time.deltaTime * CHARSEL_ANIM_SPEED;
            if (_timer >= 1.0f)
            {
                _scrollView.anchorMax = new Vector2(_scrollView.anchorMax.x, CHARSEL_MAX_ANCHOR);
            }
            else
            {
                _scrollView.anchorMax = new Vector2(
                    _scrollView.anchorMax.x,
                    CHARSEL_ANCHOR_ADD + Mathf.Cos(_timer * Mathf.PI) * CHARSEL_ANCHOR_MULT
                );
            }
        }
    }



    /* ==================== Struct ==================== */

    [Serializable]
    private struct PlanInfo
    {
        [Header("Base Data")]
        [Range(0, 7)]
        public byte Index;
        public HeistPlanType Type;
        [Header("OnCleared")]
        [Tooltip("Objects to enable when this plan is cleared.")]
        public GameObject[] Enable;
        [Tooltip("Objects to destroy when this plan is cleared.")]
        public GameObject[] Destroy;
    }
}
