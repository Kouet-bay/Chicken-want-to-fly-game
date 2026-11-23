using UnityEngine;

using UnityEngine.SceneManagement;



public class PauseManager : MonoBehaviour

{

[Header("UI References")]

[SerializeField] private GameObject pauseMenuUI;

[SerializeField] private GameObject settingsUi;



private bool isPaused = false;



private void Update()

{

if (Input.GetKeyDown(KeyCode.Escape))

{

if (isPaused)

{

if (settingsUi != null && settingsUi.activeSelf)

{

CloseSettings();

}

else

{

ResumeGame();

}

}

else

{

PauseGame();

}

}

}



public void PauseGame()

{

if (pauseMenuUI != null) pauseMenuUI.SetActive(true);

if (settingsUi != null) settingsUi.SetActive(false);



Time.timeScale = 0f;

isPaused = true;



// เปิดให้เมาส์แสดงผลตามปกติเมื่อ Pause

Cursor.lockState = CursorLockMode.None;

Cursor.visible = true;

}



public void ResumeGame()

{

if (pauseMenuUI != null) pauseMenuUI.SetActive(false);

if (settingsUi != null) settingsUi.SetActive(false);



Time.timeScale = 1f;

isPaused = false;



// บังคับให้เมาส์แสดงผลและขยับได้ปกติหลังกด Resume

Cursor.lockState = CursorLockMode.None;

Cursor.visible = true;

}


public void OpenSettings()

{

if (settingsUi != null) settingsUi.SetActive(true);

}



public void CloseSettings()

{

if (settingsUi != null) settingsUi.SetActive(false);



Cursor.lockState = CursorLockMode.None;

Cursor.visible = true;

}



public void QuitToMainMenu(string menuSceneName)

{

Time.timeScale = 1f;

SceneManager.LoadScene(menuSceneName);

}

}