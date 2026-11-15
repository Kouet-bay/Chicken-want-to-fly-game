using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject pauseMenuUI; // ลาก Panel Pause Menu มาใส่ที่นี่

    private bool isPaused = false;

    void Update()
    {
        // เมื่อผู้เล่นกดปุ่ม Esc จะสลับสถานะ Pause / Unpause
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false); // ซ่อน UI หน้า Pause
        Time.timeScale = 1f;          // คืนค่าเวลาเกมให้เดินปกติ (1 = ปกติ)
        isPaused = false;

        // ปลดล็อกและแสดงเมาส์กลับไปเป็นโหมดเล่นเกม (ถ้าเกมใช้เมาส์หมุนมุมกล้อง)
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void PauseGame()
    {
        pauseMenuUI.SetActive(true);  // แสดง UI หน้า Pause
        Time.timeScale = 0f;          // หยุดเวลาในเกมทั้งหมด (0 = หยุดนิ่ง)
        isPaused = true;

        // ปลดล็อกเมาส์เพื่อให้ผู้เล่นเอามาคลิกปุ่มในเมนูได้
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // ฟังก์ชันสำหรับผูกกับปุ่ม Quit หรือกลับหน้าเมนูหลัก
    public void QuitToMainMenu(string menuSceneName)
    {
        Time.timeScale = 1f; // ต้องคืนค่าเวลาก่อนเปลี่ยนฉากเสมอ ไม่งั้นฉากใหม่จะค้าง
        SceneManager.LoadScene(menuSceneName);
    }
}