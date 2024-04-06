using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class TownManager : MonoBehaviour
{
    /* ==================== Fields ==================== */

    [SerializeField] private string _mapName = null;
    [Header("References")]
    [SerializeField] private Joystick _joystick = null;
    [Header("Next Scene")]
    [Tooltip("Next scene comes after completing this map.")]
    [SerializeField] private string _nextSceneName = null;
    [SerializeField] private Image _blackScreen = null;
    [SerializeField] private TextMeshProUGUI _mapNameText = null;
    private GameDelegate _delegate = null;
    private float _timer = 0.0f;

    public static TownManager Instance
    {
        get;
        private set;
    }



    /* ==================== Public Methods ==================== */

    public void PauseGame(bool pause)
    {
        // Play controller UI
        CanvasPlayController.Instance.SetControllerActive(!pause);

        // Player
        FourDirectionPlayerControl.Instance.PlayerPause(pause);
    }


    public void LoadNextScene()
    {
        PauseGame(true);
        _timer = 1.0f;
        _delegate += BlackScreenFadeIn;
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


    private void DeleteInstance()
    {
        FourDirectionPlayerControl.Instance.DeleteInstance();
        CanvasPlayController.Instance.DeleteInstance();
        CameraFourDirectionMovement.Instance.DeleteInstance();
        DialogueScreen.Instance.DeleteInstance();
    }


    private void Awake()
    {
        // Singleton pattern
        Instance = this;

        // Black screen
        _blackScreen.color = new Color(0.0f, 0.0f, 0.0f, 1.0f);
        _mapNameText.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        _mapNameText.text = _mapName;

        // Delegate
        _delegate += _joystick.JoystickUpdate;
        _delegate += BlackScreenFadeOut;
    }


    private void Update()
    {
        // Always functioning
        _delegate.Invoke();
    }
}
