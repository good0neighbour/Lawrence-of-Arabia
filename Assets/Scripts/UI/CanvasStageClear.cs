using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class CanvasStageClear : MonoBehaviour
{
    /* ==================== Fields ==================== */

    [SerializeField] private Image[] _charImages = null;
    [SerializeField] private TextMeshProUGUI[] _resultTexts = null;
    [SerializeField] private RectTransform _resultScreen = null;
    private float _timer = 0.0f;



    /* ==================== Private Methods ==================== */

    private void Awake()
    {
        CharacterData.Character[] characters = GameManager.CharacterData.GetCharacterList();

        for (byte i = 0; i < GameManager.Instance.SelectedCharacters.Length; ++i)
        {
            // Character images
            _charImages[i].gameObject.SetActive(true);
            _charImages[i].sprite = characters[(int)GameManager.Instance.SelectedCharacters[i]].ProfileImage;

            // Result texts
            if (characters[(int)GameManager.Instance.SelectedCharacters[i]].Trust < 100)
            {
                _resultTexts[i].text = "Trust increased";
                ++characters[(int)GameManager.Instance.SelectedCharacters[i]].Trust;
            }
            else
            {
                _resultTexts[i].text = "Max trust";
            }
        }
    }


    private void Update()
    {
        _timer += Time.deltaTime;
        if (_timer > 5.0f)
        {
            SceneManager.LoadScene(GameManager.GameData.CurrentTown);
        }
        if (_timer > 4.5f)
        {
            _resultScreen.localPosition = new Vector3(
                0.0f,
                Mathf.Cos((_timer + 1.5f) * Mathf.PI) * 1080.0f - 1620.0f
            );
        }
        else if (_timer > 2.5f)
        {
            _resultScreen.localPosition = new Vector3(
                0.0f,
                -540.0f
            );
        }
        else if (_timer > 2.0f)
        {
            _resultScreen.localPosition = new Vector3(
                0.0f,
                Mathf.Cos(_timer * Mathf.PI) * 1080.0f - 540.0f
            );
        }
        else if (_timer > 1.0f)
        {
            _resultScreen.localPosition = new Vector3(
                0.0f,
                540.0f
            );
        }
        else if (_timer > 0.5f)
        {
            _resultScreen.localPosition = new Vector3(
                0.0f,
                Mathf.Cos((_timer + 1.5f) * Mathf.PI) * 1080.0f + 540.0f
            );
        }
    }
}
