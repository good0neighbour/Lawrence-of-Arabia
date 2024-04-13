using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Constants;

public abstract class StageManagerBase : WorldManagerBase
{
    /* ==================== Fields ==================== */

    [SerializeField] protected GameObject _failureScreen = null;
    private List<EnemyBase> _enemies = new List<EnemyBase>();

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
        Delegate = null;
        Delegate += ShowFailureScreen;
    }


    public void StageRestart()
    {
        LoadScene(SceneManager.GetActiveScene().name);
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
        HorizontalPlayerControl.Instance.SetCharacters(GameManager.Instance.SelectedCharacters);
        CanvasPlayController.Instance.SetCharacterButtons(GameManager.Instance.SelectedCharacters);
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
            _failureScreen.SetActive(true);
        }
    }
}
