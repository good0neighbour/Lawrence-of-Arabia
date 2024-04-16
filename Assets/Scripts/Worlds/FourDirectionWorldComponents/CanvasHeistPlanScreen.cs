using System;
using UnityEngine;

public class CanvasHeistPlanScreen : MonoBehaviour
{
    /* ==================== Fields ==================== */

    [SerializeField] private PlanInfo[] _planButtons = null;



    /* ==================== Public Methods ==================== */

    public void ButtonPlan(int index)
    {

    }


    public void ButtonOptional()
    {

    }



    /* ==================== Private Methods ==================== */

    private void Awake()
    {

    }



    /* ==================== Struct ==================== */

    [Serializable]
    private struct PlanInfo
    {
        [Header("Base Data")]
        [Range(0, 7)]
        public byte Index;
        public HeistPlanType Type;
        [Header("OnCleared")]
        [Tooltip("Objects to enable when this plan is cleared.")]
        public GameObject[] Enable;
        [Tooltip("Objects to destroy when this plan is cleared.")]
        public GameObject[] Destroy;
    }
}
