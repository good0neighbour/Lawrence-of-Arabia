using System;
using UnityEngine;

public abstract class EventSceneBase : MonoBehaviour
{
    /* ==================== Fields ==================== */

    [SerializeField] protected CutSceneAction[] Actions = null;
    protected byte Current = 0;
    private float _timer = 0.0f;



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
    protected struct CutSceneAction
    {
        public EvenSceneActions Action;
        public GameObject TargetObject;
        public Transform TargetTransform;
        public float Duration;
        public DialogueScript DialogueScript;
    }
}