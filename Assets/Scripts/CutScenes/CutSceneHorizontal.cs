using UnityEngine;

public class CutSceneHorizontal : MonoBehaviour
{
    /* ==================== Fields ==================== */

    [SerializeField] private CutSceneAction[] _actions = null;



    /* ==================== Public Methods ==================== */

    public void StartCutScene()
    {

    }



    /* ==================== Struct ==================== */

    private struct CutSceneAction
    {
        public CutSceneActions Action;
        public GameObject TargetObject;
        public Transform TargetTransform;
        public float Duration;
        public DialogueScript DialogueScript;
    }
}
