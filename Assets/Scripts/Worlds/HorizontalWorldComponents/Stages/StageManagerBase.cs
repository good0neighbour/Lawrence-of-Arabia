using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using static Constants;

public abstract class StageManagerBase : WorldManagerBase
{
    /* ==================== Fields ==================== */

    [SerializeField] protected GameObject _failureScreen = null;
    [SerializeField] private TextMeshProUGUI _objectiveText = null;
    [SerializeField] private RectTransform _objectiveArrow = null;
    [Header("Objectives")]
    [SerializeField] private ObjectiveInfo[] _objectives = null;
    private List<EnemyBase> _enemies = new List<EnemyBase>();
    private byte _currentObjective = 0;
    private bool _showObjective = false;

    public static StageManagerBase Instance
    {
        get;
        private set;
    }

    public static ObjectPool ObjectPool
    {
        get;
        private set;
    }



    /* ==================== Public Methods ==================== */

    public override void PauseGame(bool pause)
    {
        base.PauseGame(pause);

        // Player
        HorizontalPlayerControl.Instance.PausePlayerControl(pause);

        // Enemy
        foreach (EnemyBase enemy in _enemies)
        {
            enemy.SetEnemyPause(pause);
        }

        // Objective
        if (_showObjective)
        {
            _objectiveText.gameObject.SetActive(!pause);
            _objectiveArrow.gameObject.SetActive(!pause);
        }

        // Camera size
        CameraHorizontalMovement.Instance.SetTargetSize(HOR_CAM_DEFAULT_SIZE);
    }


    public void UrgentAlert()
    {
        foreach (EnemyBase enemy in _enemies)
        {
            enemy.ReceiveUrgentAlert();
        }
        StageMessage.Instance.EnqueueMessage("You've been compromised.");
    }


    public void EnemyDeathReport(EnemyBase reporter)
    {
        _enemies.Remove(reporter);
    }


    /// <summary>
    /// Activates failure screen immediatley
    /// </summary>
    public void GameFailed()
    {
        Timer = 0.0f;
        Delegate += ShowFailureScreen;
    }


    public void StageRestart()
    {
        LoadScene(SceneManager.GetActiveScene().name);
    }


    public void NextObjective()
    {
        if (_currentObjective < _objectives.Length - 1)
        {
            ++_currentObjective;
            _objectiveText.text = _objectives[_currentObjective].Text;
            StageMessage.Instance.EnqueueMessage($"<size=80%>New Objective</size>\n{_objectives[_currentObjective].Text}");
            _showObjective = _objectives[0].Target != null;
            _objectiveArrow.gameObject.SetActive(_showObjective);
        }
        else
        {
            _objectiveText.text = null;
            _objectiveArrow.gameObject.SetActive(false);
            _showObjective = false;
        }
    }



    /* ==================== Protected Methods ==================== */

    protected override void DeleteInstance()
    {
        base.DeleteInstance();
        Instance = null;
        HorizontalPlayerControl.Instance.DeleteInstance();
        CameraHorizontalMovement.Instance.DeleteInstance();
        ObjectPool = null;
    }


    protected override void OnStageStart()
    {
        // Set objective
        if (_objectives != null && _objectives.Length > 0)
        {
            _objectiveText.text = _objectives[0].Text;
            _showObjective = _objectives[0].Target != null;
            _objectiveArrow.gameObject.SetActive(_showObjective);
        }
    }


    protected override void Awake()
    {
        base.Awake();

        // Singletop pattern
        Instance = this;

        // ObjectPool
        ObjectPool = new ObjectPool(transform.Find("ObjectPool"));

        // Find all enemies
        foreach (EnemyBase enemy in transform.Find("Enemies").GetComponentsInChildren<EnemyBase>())
        {
            _enemies.Add(enemy);
        }
    }


    protected virtual void Start()
    {
        HorizontalPlayerControl.Instance.SetCharacters(
            GameManager.Instance.SelectedCharacters,
            GameManager.Instance.SelectedWeapons
        );
        CanvasPlayController.Instance.SetCharacterButtons(GameManager.Instance.SelectedCharacters);
    }


    protected override void Update()
    {
        if (_showObjective)
        {
            // Objective direction
            Vector2 dir = new Vector2(
                _objectives[_currentObjective].Target.position.x - HorizontalPlayerControl.Instance.transform.position.x,
                _objectives[_currentObjective].Target.position.y - HorizontalPlayerControl.Instance.transform.position.y
            );

            // Arrow rotation
            Quaternion rot;
            if (dir.x > 0.0f)
            {
                rot = Quaternion.Euler(0.0f, 0.0f, Mathf.Atan(dir.y / dir.x) * RAD_TO_DEG);
            }
            else
            {
                rot = Quaternion.Euler(0.0f, 0.0f, 180.0f + Mathf.Atan(dir.y / dir.x) * RAD_TO_DEG);
            }
            _objectiveArrow.localRotation = rot;
            _objectiveArrow.localPosition = rot * new Vector3(PLAYER_OBJARROW_DISTANCE, 0.0f, 0.0f);
        }
        base.Update();
    }



    /* ==================== Abstract Methods ==================== */

    public abstract void StageClear();


    public abstract void StageReturn();



    /* ==================== Private Methods ==================== */

    private void ShowFailureScreen()
    {
        Timer += Time.deltaTime;
        if (Timer >= PLAYER_DEATH_STANDBY_TIME)
        {
            Delegate -= ShowFailureScreen;
            _failureScreen.SetActive(true);
        }
    }



    /* ==================== Struct ==================== */

    [Serializable]
    private struct ObjectiveInfo
    {
        public string Text;
        public Transform Target;
    }
}
