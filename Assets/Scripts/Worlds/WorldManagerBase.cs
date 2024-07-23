using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public abstract class WorldManagerBase : MonoBehaviour
{
    /* ==================== Fields ==================== */

    [SerializeField] private string _worldName = null;
    [Header("Reference")]
    [SerializeField] private Image _blackScreen = null;
    [SerializeField] private TextMeshProUGUI _mapNameText = null;
    protected GameDelegate Delegate = null;
    protected string SceneToLoad = null;
    protected float Timer = 0.0f;



    /* ==================== Public Methods ==================== */


    /// <summary>
    /// Game pause. Hides play controller UI.
    /// </summary>
    public virtual void PauseGame(bool pause)
    {
        // Play controller UI
        CanvasPlayController.Instance.SetControllerActive(!pause);
    }


    /// <summary>
    /// End this scene and load next scene.
    /// </summary>
    public void LoadScene(string sceneName)
    {
        PauseGame(true);
        SceneToLoad = sceneName;
        StartBlackScreen();
    }



    /* ==================== Protected Methods ==================== */

    protected void LoatStageClearScene()
    {
        LoadScene("StageClearScene");
    }


    protected virtual void DeleteInstance()
    {
        CanvasPlayController.Instance.DeleteInstance();
        DialogueScreen.Instance.DeleteInstance();
        StageMessage.Instance?.DeleteInstance();
    }


    protected virtual void Awake()
    {
        // Black screen
        _blackScreen.color = new Color(0.0f, 0.0f, 0.0f, 1.0f);
        _mapNameText.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        _mapNameText.text = _worldName;

        // Delegate
        Delegate += BlackScreenFadeOut;
    }


    protected virtual void Update()
    {
        Delegate?.Invoke();
    }



    /* ==================== Abstract Methods ==================== */

    public abstract void CustomAction(string action);


    protected abstract void OnStageStart();



    /* ==================== Private Methods ==================== */

    private void BlackScreenFadeOut()
    {
        if (Timer < 1.0f)
        {
            Timer += Time.deltaTime;
            if (Timer >= 1.0f)
            {
                _blackScreen.color = new Color(
                    0.0f,
                    0.0f,
                    0.0f,
                    0.0f
                );
                PauseGame(false);
                _blackScreen.gameObject.SetActive(false);
                OnStageStart();
            }
            else
            {
                _blackScreen.color = new Color(
                    0.0f,
                    0.0f,
                    0.0f,
                    Mathf.Cos(Timer * Mathf.PI) * 0.5f + 0.5f
                );
            }
        }
        else if (Timer < 2.0f)
        {
            Timer += Time.deltaTime;
            if (Timer >= 2.0f)
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
                    Mathf.Cos(Timer * Mathf.PI) * 0.5f + 0.5f
                );
            }
        }
        else if (Timer > 3.0f)
        {
            Timer += Time.deltaTime;
            if (Timer >= 4.0f)
            {
                Destroy(_mapNameText.gameObject);
                Delegate -= BlackScreenFadeOut;
            }
            else
            {
                _mapNameText.color = new Color(
                    1.0f,
                    1.0f,
                    1.0f,
                    Mathf.Cos((Timer + 1.0f) * Mathf.PI) * 0.5f + 0.5f
                );
            }
        }
        else
        {
            Timer += Time.deltaTime;
        }
    }


    private void BlackScreenFadeIn()
    {
        Timer += Time.deltaTime;
        if (Timer >= 2.0f)
        {
            _blackScreen.color = new Color(
                0.0f,
                0.0f,
                0.0f,
                1.0f
            );
            DeleteInstance();
            SceneManager.LoadScene(SceneToLoad);
        }
        else
        {
            _blackScreen.color = new Color(
                0.0f,
                0.0f,
                0.0f,
                Mathf.Cos(Timer * Mathf.PI) * 0.5f + 0.5f
            );
        }
    }


    private void StartBlackScreen()
    {
        Timer = 1.0f;
        Delegate += BlackScreenFadeIn;
        _blackScreen.gameObject.SetActive(true);
    }
}
