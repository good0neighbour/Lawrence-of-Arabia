using UnityEngine;

public class LineRendererAttackEffect : MonoBehaviour, ObjectPool.PoolObject
{
    /* ==================== Fields ==================== */

    [SerializeField] private LineRenderer _lineRenderer = null;
    [SerializeField] private float _effectFadeOutSpeed = 1.0f;
    private float _alpha = 1.0f;

    public GameObject PrefabType
    {
        get;
        set;
    }



    /* ==================== Public Methods ==================== */

    public void Begin(Vector2 from, Vector2 to)
    {
        // Set position
        _lineRenderer.SetPosition(0, new Vector3(
            from.x,
            from.y,
            0.0f
        ));
        _lineRenderer.SetPosition(1, new Vector3(
            to.x,
            to.y,
            0.0f
        ));

        _alpha = 1.0f;

        // Set color
        SetColor();
    }



    /* ==================== Private Methods ==================== */

    private void SetColor()
    {
        Color color = _lineRenderer.startColor;
        _lineRenderer.startColor = new Color(color.r, color.g, color.b, _alpha);
        color = _lineRenderer.endColor;
        _lineRenderer.endColor = new Color(color.r, color.g, color.b, _alpha);
    }


    private void Update()
    {
        if (_alpha <= 0.0f)
        {
            StageManagerBase.ObjectPool.ReturnObject(PrefabType, gameObject);
        }
        // Set color
        SetColor();

        // Time flow
        _alpha -= Time.deltaTime * _effectFadeOutSpeed;
    }
}
