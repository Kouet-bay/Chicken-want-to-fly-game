using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject pauseMenuUI; // Panel หลักทั้งหมดของเมนู Pause
    [SerializeField] private GameObject settingsUi;  // Panel สำหรับหน้า Settings

    private bool isPaused = false;

    private void Update()
    {
        // เมื่อผู้เล่นกดปุ่ม Esc จะสลับสถานะ Pause / Unpause
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                // ถ้าเปิดหน้า Setting อยู่ ให้กด Esc เพื่อปิด Setting กลับมาหน้าเมนูหลักก่อน
                if (settingsUi != null && settingsUi.activeSelf)
                {
                    CloseSettings();
                }
                else
                {
                    // ถ้าอยู่หน้าเมนูหลัก ให้กด Esc เพื่อกลับเข้าเกม
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
        if (pauseMenuUI != null) 
        {
            pauseMenuUI.SetActive(true);
        }

        // ตอนกด Pause ให้เปิดเมนูหลัก แต่ซ่อนหน้า Setting ไว้ก่อน
        if (settingsUi != null)
        {
            settingsUi.SetActive(false);
        }

        Time.timeScale = 0f; // หยุดเวลาในเกม
        isPaused = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ResumeGame()
    {
        if (pauseMenuUI != null) 
        {
            pauseMenuUI.SetActive(false);
        }

        if (settingsUi != null)
        {
            settingsUi.SetActive(false);
        }

        Time.timeScale = 1f; // กลับมาเดินเวลาปกติ
        isPaused = false;     // รีเซ็ตสถานะเพื่อให้กด Esc เปิดใหม่ได้

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    
    // ==========================================
    // SETTINGS NAVIGATION
    // ==========================================
    public void OpenSettings()
    {
        if (settingsUi != null)
        {
            settingsUi.SetActive(true);
        }
    }

    public void CloseSettings()
    {
        if (settingsUi != null)
        {
            settingsUi.SetActive(false); // ปิดแค่หน้า Setting ให้กลับมาเห็นหน้าเมนูหลัก
        }
    }

    // ==========================================
    // UTILITY METHODS
    // ==========================================
    public void QuitToMainMenu(string menuSceneName)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(menuSceneName);
    }
}