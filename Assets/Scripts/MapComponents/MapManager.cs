using System;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    /* ==================== Fields ==================== */

    [SerializeField] private string _mapName = null;
    [Header("Game End Conditions")]
    [SerializeField] private List<GameCondition> _victoryConditions = new List<GameCondition>();
    [SerializeField] private List<GameCondition> _defeatedConditions = new List<GameCondition>();
    [Header("References")]
    [SerializeField] private CanvasPlayController _controller = null;
    [SerializeField] private Joystick _joystick = null;
    private List<EnemyBehaviour> _enemies = new List<EnemyBehaviour>();

    public static MapManager Instance
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

        // Enemy
        foreach (EnemyBehaviour enemy in _enemies)
        {
            enemy.SetEnemyPause(pause);
        }
    }


    public void CheckVictory()
    {
        if (CheckConditions(_victoryConditions))
        {
            Debug.Log("Victory");
        }
    }


    public void CheckDefeated()
    {
        if (CheckConditions(_defeatedConditions))
        {
            Debug.Log("Defeated");
        }
    }



    /* ==================== Private Methods ==================== */

    private bool CheckConditions(List<GameCondition> conditionList)
    {
        foreach (GameCondition condition in conditionList)
        {
            switch (condition.Condition)
            {
                case GameEndTypes.Destroyed:
                    foreach (GameObject target in condition.Targets)
                    {
                        switch (target)
                        {
                            case null:
                                break;
                            default:
                                return false;
                        }
                    }
                    break;

                case GameEndTypes.Disabled:
                    foreach (GameObject target in condition.Targets)
                    {
                        switch (target.activeSelf)
                        {
                            case true:
                                return false;
                        }
                    }
                    break;

                case GameEndTypes.Enabled:
                    foreach (GameObject target in condition.Targets)
                    {
                        switch (target.activeSelf)
                        {
                            case false:
                                return false;
                        }
                    }
                    break;
            }
        }
        return true;
    }


    private void Awake()
    {
        // Singletop pattern
        Instance = this;

        // Find all enemies
        foreach (EnemyBehaviour enemy in transform.Find("Enemies").GetComponentsInChildren<EnemyBehaviour>())
        {
            _enemies.Add(enemy);
        }

        // Victory conditions
        foreach (GameCondition condition in _victoryConditions)
        {
            foreach (GameObject target in condition.Targets)
            {
                target.AddComponent<ConditionCheck>().InitializeCondition(condition.Condition, true);
            }
        }

        // Defeated conditions
        foreach (GameCondition condition in _defeatedConditions)
        {
            foreach (GameObject target in condition.Targets)
            {
                target.AddComponent<ConditionCheck>().InitializeCondition(condition.Condition, true);
            }
        }
    }


    private void Update()
    {
        // Always functioning
        _joystick.JoystickUpdate();
    }



    /* ==================== Struct ==================== */

    [Serializable]
    private struct GameCondition
    {
        public GameEndTypes Condition;
        public GameObject[] Targets;
    }



    /* ==================== Class ==================== */

    private class ConditionCheck : MonoBehaviour
    {
        private GameEndTypes _condition;
        private bool _isVictoryCondition = false;


        public void InitializeCondition(GameEndTypes condition, bool isVictoryCondition)
        {
            _condition = condition;
            _isVictoryCondition = isVictoryCondition;
        }


        private void CheckCondition()
        {
            if (_isVictoryCondition)
            {
                Instance.CheckVictory();
            }
            else
            {
                Instance.CheckDefeated();
            }
        }


        private void OnDestroy()
        {
            switch (_condition)
            {
                case GameEndTypes.Destroyed:
                    CheckCondition();
                    return;

                default:
                    return;
            }
        }


        private void OnDisable()
        {
            switch (_condition)
            {
                case GameEndTypes.Disabled:
                    CheckCondition();
                    return;

                default:
                    return;
            }
        }


        private void OnEnable()
        {
            switch (_condition)
            {
                case GameEndTypes.Enabled:
                    CheckCondition();
                    return;

                default:
                    return;
            }
        }
    }
}
