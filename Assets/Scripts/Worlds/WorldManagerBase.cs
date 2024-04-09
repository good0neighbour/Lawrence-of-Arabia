using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public abstract class WorldManagerBase : MonoBehaviour
{
    /* ==================== Fields ==================== */

    [SerializeField] private string _worldName = null;
    [Header("Next Scene")]
    [Tooltip("Next scene comes after completing this map.")]
    [SerializeField] private string _nextSceneName = null;
    [Header("Reference")]
    [SerializeField] private Image _blackScreen = null;
    [SerializeField] private TextMeshProUGUI _mapNameText = null;
    [SerializeField] private Joystick _joystick = null;
    private GameDelegate _delegate = null;
    private float _timer = 0.0f;



    /* ==================== Public Methods ==================== */

    public void LoadNextScene()
    {
        PauseGame(true);
        _timer = 1.0f;
        _delegate += BlackScreenFadeIn;
        _blackScreen.gameObject.SetActive(true);
    }


    /// <summary>
    /// Game pause. Hides play controller UI.
    /// </summary>
    public virtual void PauseGame(bool pause)
    {
        // Play controller UI
        CanvasPlayController.Instance.SetControllerActive(!pause);
    }



    /* ==================== Protected Methods ==================== */

    protected virtual void DeleteInstance()
    {
        CanvasPlayController.Instance.DeleteInstance();
        DialogueScreen.Instance.DeleteInstance();
        StageMessage.Instance.DeleteInstance();
    }


    protected virtual void Awake()
    {
        // Black screen
        _blackScreen.color = new Color(0.0f, 0.0f, 0.0f, 1.0f);
        _mapNameText.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        _mapNameText.text = _worldName;

        // Delegate
        _delegate += _joystick.JoystickUpdate;
        _delegate += BlackScreenFadeOut;
    }



    /* ==================== Private Methods ==================== */

    private void BlackScreenFadeOut()
    {
        if (_timer < 1.0f)
        {
            _timer += Time.deltaTime;
            if (_timer >= 1.0f)
            {
                _blackScreen.color = new Color(
                    0.0f,
                    0.0f,
                    0.0f,
                    0.0f
                );
                PauseGame(false);
                _blackScreen.gameObject.SetActive(false);
            }
            else
            {
                _blackScreen.color = new Color(
                    0.0f,
                    0.0f,
                    0.0f,
                    Mathf.Cos(_timer * Mathf.PI) * 0.5f + 0.5f
                );
            }
        }
        else if (_timer < 2.0f)
        {
            _timer += Time.deltaTime;
            if (_timer >= 2.0f)
            {
                _mapNameText.color = new Color(
                    1.0f,
                    1.0f,
                    1.0f,
                    1.0f
                );
            }
            else
            {
                _mapNameText.color = new Color(
                    1.0f,
                    1.0f,
                    1.0f,
                    Mathf.Cos(_timer * Mathf.PI) * 0.5f + 0.5f
                );
            }
        }
        else if (_timer > 3.0f)
        {
            _timer += Time.deltaTime;
            if (_timer >= 4.0f)
            {
                Destroy(_mapNameText.gameObject);
                _delegate -= BlackScreenFadeOut;
            }
            else
            {
                _mapNameText.color = new Color(
                    1.0f,
                    1.0f,
                    1.0f,
                    Mathf.Cos((_timer + 1.0f) * Mathf.PI) * 0.5f + 0.5f
                );
            }
        }
        else
        {
            _timer += Time.deltaTime;
        }
    }


    private void BlackScreenFadeIn()
    {
        if (_timer < 2.0f)
        {
            _timer += Time.deltaTime;
            if (_timer >= 2.0f)
            {
                _blackScreen.color = new Color(
                    0.0f,
                    0.0f,
                    0.0f,
                    1.0f
                );
                _delegate = null;
                DeleteInstance();
                SceneManager.LoadScene(_nextSceneName);
            }
            else
            {
                _blackScreen.color = new Color(
                    0.0f,
                    0.0f,
                    0.0f,
                    Mathf.Cos(_timer * Mathf.PI) * 0.5f + 0.5f
                );
            }
        }
        else
        {
            _timer += Time.deltaTime;
        }
    }


    private void Update()
    {
        _delegate.Invoke();
    }
}
