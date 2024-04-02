using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Reflection;

public class CutScene : MonoBehaviour
{
    /* ==================== Fields ==================== */

    [SerializeField] private List<CutSceneAction> _actions = null;
    [SerializeField] private Image _skipImage = null;
    private byte _current = 0;
    private float _timer = 0.0f;
    private float _skip = 0.0f;
    private bool _skipPressed = false;



    /* ==================== Public Methods ==================== */

    public void SkipPress(bool pressed)
    {
        if (pressed)
        {
            _skipPressed = true;
        }
        else
        {
            _skipPressed = false;
            _skip = 0.0f;
            _skipImage.fillAmount = 0.0f;
        }
    }


#if UNITY_EDITOR
    public List<CutSceneAction> GetActions()
    {
        return _actions;
    }


    public void AddAction(byte index)
    {
        _actions.Insert(index, new CutSceneAction());
    }


    public void DeleteAction(byte index)
    {
        _actions.RemoveAt(index);
    }


    public void MoveAction(byte from, byte to)
    {
        CutSceneAction temp = _actions[from];
        _actions.RemoveAt(from);
        _actions.Insert(to, temp);
    }
#endif



    /* ==================== Private Methods ==================== */

    private void ColourChange(Image image, TextMeshProUGUI text, bool isFadeIn)
    {
        // Alpha value
        float alpha = 0.0f;

        // Image colour
        switch (image)
        {
            case null:
                break;

            default:
                if (isFadeIn)
                {
                    alpha = image.color.a + Time.deltaTime * Constants.CUTSCN_FADEIN_SPEED;
                    if (alpha >= 1.0f)
                    {
                        alpha = 1.0f;
                    }
                }
                else
                {
                    alpha = image.color.a - Time.deltaTime * Constants.CUTSCN_FADEIN_SPEED;
                    if (alpha <= 0.0f)
                    {
                        alpha = 0.0f;
                    }
                }
                image.color = new Color(
                    image.color.r,
                    image.color.g,
                    image.color.b,
                    alpha
                );
                break;
        }

        // Text colour
        switch (text)
        {
            case null:
                break;

            default:
                if (isFadeIn)
                {
                    alpha = text.color.a + Time.deltaTime * Constants.CUTSCN_FADEIN_SPEED;
                    if (alpha >= 1.0f)
                    {
                        alpha = 1.0f;
                    }
                }
                else
                {
                    alpha = text.color.a - Time.deltaTime * Constants.CUTSCN_FADEIN_SPEED;
                    if (alpha <= 0.0f)
                    {
                        alpha = 0.0f;
                    }
                }
                text.color = new Color(
                    text.color.r,
                    text.color.g,
                    text.color.b,
                    alpha
                );
                break;
        }

        // Colour change end
        switch (alpha)
        {
            case 0.0f:
            case 1.0f:
                ++_current;
                return;
            default:
                return;
        }
    }


    private void OnDisable()
    {
        _current = 0;
        _timer = 0.0f;
        _skip = 0.0f;
        _skipPressed = false;
        _skipImage.fillAmount = 0.0f;
    }


    private void Update()
    {
#if UNITY_EDITOR
        if (_current >= _actions.Count)
        {
            Debug.LogError($"{name}: It has to load a map or disable/distroy this cut scene at the end.");
            enabled = false;
            return;
        }
#endif
        // Skip
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SkipPress(true);
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            SkipPress(false);
        }

        if (_skipPressed)
        {
            _skip += Time.deltaTime * Constants.CUTSCN_SKIP_SPEED;
            _skipImage.fillAmount = _skip;
            if (_skip >= 1.0f)
            {
                _current = (byte)(_actions.Count - 1);
            }
        }

        // Cut scene action
        switch (_actions[_current].Action)
        {
            case CutSceneActions.FadeIn:
                ColourChange(_actions[_current].TargetImage, _actions[_current].TargetText, true);
                return;

            case CutSceneActions.FadeOut:
                ColourChange(_actions[_current].TargetImage, _actions[_current].TargetText, false);
                return;

            case CutSceneActions.Enable:
                _actions[_current].TargetObject.SetActive(true);
                return;

            case CutSceneActions.Disable:
                _actions[_current].TargetObject.SetActive(false);
                return;

            case CutSceneActions.Destroy:
                Destroy(_actions[_current].TargetObject);
                return;

            case CutSceneActions.LoadScene:
                SceneManager.LoadScene(_actions[_current].SceneName);
                return;

            case CutSceneActions.Wait:
                _timer += Time.deltaTime;
                if (_timer >= _actions[_current].Duration)
                {
                    _timer -= _actions[_current].Duration;
                    ++_current;
                }
                return;
        }
    }



    /* ==================== Struct ==================== */

    [Serializable]
    public struct CutSceneAction
    {
        public CutSceneActions Action;
        public Image TargetImage;
        public TextMeshProUGUI TargetText;
        public GameObject TargetObject;
        public float Duration;
        public string SceneName;
    }
}
