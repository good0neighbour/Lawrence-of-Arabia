using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class DynamicEventScene : MonoBehaviour, IEventScene
{
    /* ==================== Fields ==================== */

    [SerializeField] private EventSceneBase _defaultEventScene = null;
    [SerializeField] private RandomEventScene[] _randomEventScenes = null;
    private IEventScene _current = null;
    private byte _totalWeight = 0;



    /* ==================== Public Methods ==================== */

    public void StartEventScene()
    {
        _current.StartEventScene();
        _current = RandomPick();
    }



    /* ==================== Private Methods ==================== */

    private IEventScene RandomPick()
    {
        byte ran = (byte)Random.Range(0, _totalWeight);
        byte cur = 0;
        foreach (RandomEventScene scene in _randomEventScenes)
        {
            cur += scene.Weight;
            if (ran <= cur)
            {
                return scene.EventScene;
            }
        }
        return _randomEventScenes[_randomEventScenes.Length - 1].EventScene;
    }


    private void Awake()
    {
        foreach (RandomEventScene scene in _randomEventScenes)
        {
            _totalWeight += scene.Weight;
        }
        ++_totalWeight;

        if (_defaultEventScene != null)
        {
            _current = _defaultEventScene;
        }
        else
        {
            _current = RandomPick();
        }
    }



    /* ==================== Struct ==================== */

    [Serializable]
    private struct RandomEventScene
    {
        [Tooltip("Target event scene")]
        public EventSceneBase EventScene;
        [Tooltip("Chance how much it can activate")]
        public byte Weight;
    }
}
