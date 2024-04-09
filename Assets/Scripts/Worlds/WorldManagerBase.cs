using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public abstract class WorldManagerBase : MonoBehaviour
{
    /* ==================== Fields ==================== */

    [SerializeField] private string _worldName = null;
    [Header("Scene Info")]
    [Tooltip("Next scene comes after completing this stage.")]
    [SerializeField] private string _nextSceneName = null;
    [Tooltip("Previous scene comes after giving this stage up.")]
    [SerializeField] private string _previousSceneName = null;
    [Header("Reference")]
    [SerializeField] private Image _blackScreen = null;
    [SerializeField] private TextMeshProUGUI _mapNameText = null;
    [SerializeField] private Joystick _joystick = null;
    [SerializeField] protected GameObject _failureScreen = null;
    private GameDelegate _delegate = null;
    private string _sceneToLoad = null;
    private float _timer = 0.0f;



    /* ==================== Public Methods ==================== */

    public void LoadNextScene()
    {
        _sceneToLoad = _nextSceneName;
        PauseGame(true);
        StartBlackScreen();
    }


    public void ReloadScene()
    {
        _sceneToLoad = SceneManager.GetActiveScene().name;
        StartBlackScreen();
    }


    public void LoadPreviousScene()
    {
        _sceneToLoad = _previousSceneName;
        StartBlackScreen();
    }


    /// <summary>
    /// Game pause. Hides play controller UI.
    /// </summary>
    public virtual void PauseGame(bool pause)
    {
        // Play controller UI
        CanvasPlayController.Instance.SetControllerActive(!pause);
    }


    /// <summary>
    /// Activates failure screen immediatley
    /// </summary>
    public void GameFailed()
    {
        _timer = 0.0f;
        _delegate = null;
        _delegate += ShowFailureScreen;
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
                SceneManager.LoadScene(_sceneToLoad);
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


    private void StartBlackScreen()
    {
        _timer = 1.0f;
        _delegate += BlackScreenFadeIn;
        _blackScreen.gameObject.SetActive(true);
    }


    private void ShowFailureScreen()
    {
        _timer += Time.deltaTime;
        if (_timer >= Constants.CHAR_DEATH_STANDBY_TIME)
        {
            _failureScreen.SetActive(true);
        }
    }


    private void Update()
    {
        _delegate.Invoke();
    }
}
