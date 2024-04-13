using UnityEngine;
using static Constants;

public class PlayerTrigger : TriggerBase
{
    /* ==================== Fields ==================== */

    [Tooltip("Select condition of this trigger.")]
    [SerializeField] private TriggerTypes _triggerType;

#if UNITY_EDITOR
    public TriggerTypes TriggerType
    {
        get
        {
            return _triggerType;
        }
        set
        {
            _triggerType = value;
        }
    }
#endif



    /* ==================== Private Methods ==================== */

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (_triggerType)
        {
            case TriggerTypes.Enter:
                if (collision.gameObject.layer == LAYER_D_PLAYER)
                {
                    ActiveTrigger();
                }
                return;

            case TriggerTypes.Interact:
                HorizontalPlayerControl.Instance.SetInteractBtnActive(ActiveTrigger, true);
                return;

            default:
                return;
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        switch (_triggerType)
        {
            case TriggerTypes.Exit:
                if (collision.gameObject.layer == LAYER_D_PLAYER)
                {
                    ActiveTrigger();
                }
                return;

            case TriggerTypes.Interact:
                HorizontalPlayerControl.Instance.SetInteractBtnActive(ActiveTrigger, false);
                return;

            default:
                return;
        }
    }
}
