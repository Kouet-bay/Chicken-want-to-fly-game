using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("UI Panels")]
    [SerializeField] private GameObject creditUi;
    [SerializeField] private GameObject settingsUi;

    [Header("Start Game Delay")]
    [SerializeField] private float startDelay = 0.15f;

    public void StartGame()
    {
        StartCoroutine(StartGameRoutine());
    }

    private IEnumerator StartGameRoutine()
    {
        yield return new WaitForSeconds(startDelay);
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Debug.Log("Exiting Game...");
        Application.Quit();
    }

    public void ToggleCredit(bool isShow)
    {
        if (creditUi != null)
        {
            creditUi.SetActive(isShow);
        }
    }

    public void ToggleSettings(bool isShow)
    {
        if (settingsUi != null)
        {
            settingsUi.SetActive(isShow);
        }
    }
}