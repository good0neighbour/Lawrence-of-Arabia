using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using static Constants;

public class CanvasCutScene : MonoBehaviour
{
    /* ==================== Fields ==================== */

    [SerializeField] private CutScene _cutScene = null;
    [SerializeField] private Image _skipImage = null;
    [SerializeField] private Image _image = null;
    [SerializeField] private TextMeshProUGUI _text = null;
    [SerializeField] private AudioSource _audioSource = null;
    [SerializeField] private GameObject _skipButton = null;
    private CutScene.CutSceneInfo[] _actions = null;
    private byte _current = 0;
    private byte _state = CUTSCENE_FADEIN;
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



    /* ==================== Private Methods ==================== */

    private void CurrentAction()
    {
        // Text
        _text.text = _actions[_current].Text;

        // Image
        if (_actions[_current].Image != null)
        {
            _image.sprite = _actions[_current].Image;
        }

        // Audio
        if (_actions[_current].Audio != null)
        {
            _audioSource.clip = _actions[_current].Audio;
            _audioSource.Play();
        }

        // Reset timer
        _timer = 0.0f;
    }


    private void CutSceneEnd()
    {
        _text.text = null;
        _state = CUTSCENE_FADEOUT;
        _timer = 1.0f;
        _skipButton.SetActive(false);
    }


    private void Awake()
    {
        _actions = _cutScene.GetCutSceneActions();
        _image.color = Color.black;
        _image.sprite = _actions[_current].Image;
        _text.text = null;
    }


    private void Update()
    {
        switch (_state)
        {
            case CUTSCENE_FADEIN:
                #region FadeIn
                _timer += Time.deltaTime;
                if (_timer >= 1.0f)
                {
                    _image.color = Color.white;
                    CurrentAction();
                    _skipButton.SetActive(true);
                    _state = CUTSCENE_ACTION;
                }
                else
                {
                    _image.color = new Color(_timer, _timer, _timer, 1.0f);
                }
                #endregion
                return;

            case CUTSCENE_ACTION:
                #region Action
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
                    _skip += Time.deltaTime * CUTSCN_SKIP_SPEED;
                    _skipImage.fillAmount = _skip;
                    if (_skip >= 1.0f)
                    {
                        CutSceneEnd();
                        SkipPress(false);
                        return;
                    }
                }

                // CutScene actions
                _timer += Time.deltaTime;
                if (_timer >= _actions[_current].Duration)
                {
                    ++_current;
                    if (_current < _actions.Length)
                    {
                        CurrentAction();
                    }
                    else
                    {
                        CutSceneEnd();
                        return;
                    }
                }
                #endregion
                return;

            case CUTSCENE_FADEOUT:
                #region FadeOut
                _timer -= Time.deltaTime;
                if (_timer <= 0.0f)
                {
                    _audioSource.Stop();
                    SceneManager.LoadScene(_cutScene.GetNextSceneName());
                }
                else
                {
                    _image.color = new Color(_timer, _timer, _timer, 1.0f);
                    _audioSource.volume = _timer;
                }
                #endregion
                return;
        }
    }
}
