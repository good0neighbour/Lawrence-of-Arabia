using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class EventSceneBase : MonoBehaviour, IEventScene
{
    /* ==================== Fields ==================== */

    [SerializeField] protected EventSceneAction[] Actions = new EventSceneAction[0];
    [SerializeField] protected bool ResumeAtEnd = true;
    protected byte Current = 0;
    private float _timer = 0.0f;

#if UNITY_EDITOR
    public bool ResumeAtTheEnd
    {
        get
        {
            return ResumeAtEnd;
        }
        set
        {
            ResumeAtEnd = value;
        }
    }
#endif



    /* ==================== Public Methods ==================== */

    public virtual void StartEventScene()
    {
#if UNITY_EDITOR
        if (Actions == null || Actions.Length == 0)
        {
            Debug.LogError($"{name}: No even scene.");
            return;
        }
#endif
        Current = 0;
        _timer = 0.0f;
        enabled = true;
        NextAction();
    }


    public void ResumeEventScene(bool resume)
    {
        enabled = resume;
    }


#if UNITY_EDITOR
    public List<EventSceneAction> GetActions()
    {
        List<EventSceneAction> result = new List<EventSceneAction>();
        foreach (EventSceneAction action in Actions)
        {
            result.Add(action);
        }
        return result;
    }


    public void SetActions(EventSceneAction[] actions)
    {
        Actions = actions;
    }
#endif



    /* ==================== Protected Methods ==================== */

    protected abstract void EndEventScene();


    protected abstract void NextAction();



    /* ==================== Private Methods ==================== */

    private void Awake()
    {
        enabled = false;
    }


    private void Update()
    {
        _timer += Time.deltaTime;
        if (_timer >= Actions[Current].Duration)
        {
            ++Current;
            if (Current >= Actions.Length)
            {
                enabled = false;
                DialogueScreen.Instance.CloseDialogueScreen();
                EndEventScene();
            }
            else
            {
                NextAction();
                _timer -= Actions[Current].Duration;
            }
        }
    }



    /* ==================== Struct ==================== */

    [Serializable]
    public struct EventSceneAction
    {
        public EventSceneActions Action;
        public GameObject TargetObject;
        public Transform TargetTransform;
        public float Duration;
        public DialogueBase DialogueScript;
        public string Text;
    }
}
