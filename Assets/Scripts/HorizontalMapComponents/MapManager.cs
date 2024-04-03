using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapManager : MonoBehaviour
{
    /* ==================== Fields ==================== */

    [SerializeField] private string _mapName = null;
    [Header("References")]
    [SerializeField] private CanvasPlayController _controller = null;
    [SerializeField] private Joystick _joystick = null;
    [Header("Next Scene")]
    [Tooltip("Next scene comes after completing this map.")]
    [SerializeField] private string _nextSceneName = null;
    private List<EnemyBehaviour> _enemies = new List<EnemyBehaviour>();

    public static MapManager Instance
    {
        get;
        private set;
    }

    public static ObjectPool ObjectPool
    {
        get;
        private set;
    }

    public string MapName
    {
        get
        {
            return _mapName;
        }
    }



    /* ==================== Public Methods ==================== */

    /// <summary>
    /// Game pause. Hides play controller UI.
    /// </summary>
    public void PauseGame(bool pause)
    {
        // Play controller UI
        _controller.SetControllerActive(!pause);

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
    }


    public void EnemyDeathReport(EnemyBehaviour reporter)
    {
        _enemies.Remove(reporter);
    }


    public void LoadNextScene()
    {
        SceneManager.LoadScene(_nextSceneName);
    }



    /* ==================== Private Methods ==================== */

    private void Awake()
    {
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


    private void Update()
    {
        // Always functioning
        _joystick.JoystickUpdate();
    }
}
