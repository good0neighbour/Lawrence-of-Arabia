using System.Collections.Generic;

public abstract class StageManagerBase : WorldManagerBase
{
    /* ==================== Fields ==================== */

    private List<EnemyBehaviour> _enemies = new List<EnemyBehaviour>();

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
        foreach (EnemyBehaviour enemy in _enemies)
        {
            enemy.SetEnemyPause(pause);
        }
    }


    public void UrgentAlert()
    {
        foreach (EnemyBehaviour enemy in _enemies)
        {
            enemy.ReceiveUrgentAlert();
        }
        StageMessage.Instance.EnqueueMessage("You've been compromised.");
    }


    public void EnemyDeathReport(EnemyBehaviour reporter)
    {
        _enemies.Remove(reporter);
    }



    /* ==================== Protected Methods ==================== */

    protected override void DeleteInstance()
    {
        base.DeleteInstance();
        Instance = null;
        HorizontalPlayerControl.Instance.DeleteInstance();
        CameraHorizontalMovement.Instance.DeleteInstance();
    }


    protected override void Awake()
    {
        base.Awake();

        // Singletop pattern
        Instance = this;

        // ObjectPool
        ObjectPool = new ObjectPool(transform.Find("ObjectPool"));

        // Find all enemies
        foreach (EnemyBehaviour enemy in transform.Find("Enemies").GetComponentsInChildren<EnemyBehaviour>())
        {
            _enemies.Add(enemy);
        }
    }


    protected virtual void Start()
    {
        HorizontalPlayerControl.Instance.SetCharacters(GameManager.Instance.SelectedCharacters);
        CanvasPlayController.Instance.SetCharacterButtons(GameManager.Instance.SelectedCharacters);
    }
}
