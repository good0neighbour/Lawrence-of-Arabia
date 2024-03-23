using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CanvasIntro : MonoBehaviour
{
    /* ==================== Fields ==================== */

    [SerializeField] private Image _blackScreen = null;
    [SerializeField] private TextMeshProUGUI _mapName = null;
    private float _timer = 0.0f;



    /* ==================== Private Methods ==================== */

    private void Awake()
    {
        _blackScreen.color = new Color(0.0f, 0.0f, 0.0f, 1.0f);
        _mapName.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
    }


    private void Start()
    {
        _mapName.text = MapManager.Instance.MapName;
    }


    private void Update()
    {
        if (_timer < 1.0f)
        {
            _timer += Time.deltaTime;
            if (_timer >= 1.0f)
            {
                _blackScreen.color = new Color(
                    0.0f,
                    0.0f,
                    0.0f,
                    0.0f
                );
                MapManager.Instance.PauseGame(false);
            }
            else
            {
                _blackScreen.color = new Color(
                    0.0f,
                    0.0f,
                    0.0f,
                    Mathf.Cos(_timer * Mathf.PI) * 0.5f + 0.5f
                );
            }
        }
        else if (_timer < 2.0f)
        {
            _timer += Time.deltaTime;
            if (_timer >= 2.0f)
            {
                _mapName.color = new Color(
                    1.0f,
                    1.0f,
                    1.0f,
                    1.0f
                );
            }
            else
            {
                _mapName.color = new Color(
                    1.0f,
                    1.0f,
                    1.0f,
                    Mathf.Cos(_timer * Mathf.PI) * 0.5f + 0.5f
                );
            }
        }
        else if (_timer > 3.0f)
        {
            _timer += Time.deltaTime;
            if (_timer >= 4.0f)
            {
                Destroy(gameObject);
            }
            else
            {
                _mapName.color = new Color(
                    1.0f,
                    1.0f,
                    1.0f,
                    Mathf.Cos((_timer + 1.0f) * Mathf.PI) * 0.5f + 0.5f
                );
            }
        }
        else
        {
            _timer += Time.deltaTime;
        }
    }
}
