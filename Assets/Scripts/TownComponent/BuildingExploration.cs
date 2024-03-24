using UnityEngine;
using UnityEngine.SceneManagement;

public class BuildingExploration : MonoBehaviour
{
    /* ==================== Fields ==================== */

    [SerializeField] private GameObject _canvas = null;



    /* ==================== Public Methods ==================== */

    public void OpenCloseWindow(bool open)
    {
        _canvas.SetActive(open);
    }


    public void ButtonStart()
    {
        SceneManager.LoadScene("SampleMap");
    }



    /* ==================== Private Methods ==================== */

    private void Awake()
    {
        _canvas.SetActive(false);
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == Constants.LAYER_D_PLAYER)
        {
            OpenCloseWindow(true);
        }
    }
}
