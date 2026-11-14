using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("UI Panels")]
    [SerializeField] private GameObject creditUi;
    [SerializeField] private GameObject settingsUi; // หน้าต่าง Setting

    public void StartGame()
    {
        SceneManager.LoadScene(1); // โหลดฉากเกม (Index 1)
    }

    public void QuitGame()
    {
        Debug.Log("Exiting Game...");
        Application.Quit();
    }

    // เปิด-ปิดหน้าต่าง Credits
    public void ToggleCredit(bool isShow)
    {
        if (creditUi != null)
        {
            creditUi.SetActive(isShow);
        }
    }

    // เปิด-ปิดหน้าต่าง Settings
    public void ToggleSettings(bool isShow)
    {
        if (settingsUi != null)
        {
            settingsUi.SetActive(isShow);
        }
    }
}