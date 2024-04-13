using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static Constants;

public class StageMessage : MonoBehaviour
{
    /* ==================== Fields ==================== */

    [SerializeField] TextMeshProUGUI _messageText;
    private Queue<string> _queue = new Queue<string>();
    private float _timer = 0.0f;

    public static StageMessage Instance
    {
        get;
        private set;
    }



    /* ==================== Public Methods ==================== */

    public void EnqueueMessage(string message)
    {
        if (enabled)
        {
            _queue.Enqueue(message);
        }
        else
        {
            ShowMessage(message);
            _messageText.gameObject.SetActive(true);
            enabled = true;
        }
    }


    public void DeleteInstance()
    {
        Instance = null;
    }



    /* ==================== Private Methods ==================== */

    private void ShowMessage(string message)
    {
        _messageText.text = message;
        _timer = 0.0f;
    }


    private void Awake()
    {
        Instance = this;
    }


    private void Update()
    {
        if (_timer < 1.0f)
        {
            _timer += Time.deltaTime * STGMSG_SPEED;
            if (_timer >= 1.0f)
            {
                _messageText.color = Color.white;
            }
            else
            {
                _messageText.color = new Color(1.0f, 1.0f, 1.0f, _timer);
            }
        }
        else if (_timer > 1.0f + STGMSG_SHOW_TIMER)
        {
            _timer += Time.deltaTime * STGMSG_SPEED;
            if (_timer >= 2.0f + STGMSG_SHOW_TIMER)
            {
                _messageText.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
                if (_queue.Count > 0)
                {
                    ShowMessage(_queue.Dequeue());
                }
                else
                {
                    _messageText.gameObject.SetActive(false);
                    enabled = false;
                }
            }
            else
            {
                _messageText.color = new Color(1.0f, 1.0f, 1.0f, 2.0f + STGMSG_SHOW_TIMER - _timer);
            }
        }
        else
        {
            _timer += Time.deltaTime * STGMSG_SPEED;
        }
    }
}
