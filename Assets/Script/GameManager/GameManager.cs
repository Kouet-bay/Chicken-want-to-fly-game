using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    // ==========================================
    // 1. PLAYER LIVES & CHECKPOINT SETTINGS
    // ==========================================
    [Header("Player Lives & Checkpoint")]
    [SerializeField] private int startLives = 3;
    private int life;
    public int Life => life;

    private Vector3 checkPoint;

    [Header("References")]
    public Player player;

    // ==========================================
    // 2. PAUSE & SETTINGS SYSTEM (ระบบหยุดเกม)
    // ==========================================
    [Header("Pause UI Reference")]
    [SerializeField] private GameObject pauseMenuUI; // ลาก UI Panel Pause มาใส่ที่นี่ใน Inspector
    private bool isPaused = false;

    // ==========================================
    // UNITY LIFECYCLE METHODS
    // ==========================================
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        life = startLives;
        
        // ค้นหา Player ในฉากอัตโนมัติถ้ายังไม่ได้ลากใส่
        if (player == null)
        {
            player = FindAnyObjectByType<Player>();
        }
        
        // กำหนดจุดเกิดเริ่มต้นที่ตำแหน่งตัวละครตอนเริ่มเกม
        if (player != null)
        {
            checkPoint = player.transform.position;
        }

        // อัปเดต UI ชีวิตเริ่มต้น
        UpdateLifeUI();

        // ซ่อนหน้า Pause ตอนเริ่มเกมเสมอ
        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(false);
        }
    }

    private void Update()
    {
        // เช็คการกดปุ่ม Esc เพื่อเปิด/ปิดเมนูตั้งค่าและหยุดเวลาเกม
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

    // ==========================================
    // CHECKPOINT & RESPAWN METHODS
    // ==========================================
    public void SetCheckPoint(Vector3 pos)
    {
        checkPoint = pos;
        Debug.Log("Checkpoint Updated at: " + pos);
    }

    public void OnPlayerTakeDamage()
    {
        life--;
        life = Mathf.Max(life, 0);

        UpdateLifeUI();

        if (life <= 0)
        {
            // Game Over: หัวใจหมดเกลี้ยง
            if (UIManager.Instance != null)
            {
                UIManager.Instance.SetVisibleLosePanel(true);
            }
            SetEnablePlayer(false);
            Debug.Log("Game Over! Out of lives.");
        }
        else
        {
            // เสียหัวใจ 1 ดวง: วาร์ปกลับจุด Checkpoint ทันที
            RespawnInstantly();
        }
    }

    private void RespawnInstantly()
    {
        if (player != null)
        {
            player.RespawnAt(checkPoint);
            Debug.Log("Player respawned instantly at checkpoint!");
        }
    }

    // ==========================================
    // PAUSE & MOUSE CURSOR CONTROL METHODS
    // ==========================================
    public void ResumeGame()
    {
        if (pauseMenuUI != null) 
        {
            pauseMenuUI.SetActive(false);
        }
        
        Time.timeScale = 1f; // คืนค่าเวลาให้เกมกลับมาเดินปกติ (เกมข้างหลังเล่นต่อ)
        isPaused = false;

        // ซ่อนและล็อกเมาส์กลับสำหรับควบคุมมุมกล้อง 3D
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void PauseGame()
    {
        if (pauseMenuUI != null) 
        {
            pauseMenuUI.SetActive(true);
        }
        
        Time.timeScale = 0f; // หยุดเวลาในเกมทั้งหมด (หยุดการเคลื่อนไหวและเกมข้างหลัง)
        isPaused = true;

        // ปลดล็อกและแสดงลูกศรเมาส์เพื่อให้ผู้เล่นคลิกปรับค่าใน Setting ได้
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void QuitToMainMenu(string menuSceneName)
    {
        Time.timeScale = 1f; // คืนค่าเวลาก่อนเปลี่ยนฉากเสมอ ป้องกันเวลาค้างในฉากถัดไป
        SceneManager.LoadScene(menuSceneName);
    }

    // ==========================================
    // UTILITY METHODS
    // ==========================================
    public void SetEnablePlayer(bool enable)
    {
        if (player != null)
        {
            player.enabled = enable;
        }
    }

    private void UpdateLifeUI()
    {
        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateLifeUI(life);
        }
    }

    public void OnEnterWinZone()
    {
        SetEnablePlayer(false);
        if (UIManager.Instance != null)
        {
            UIManager.Instance.SetVisibleWinPanel(true);
        }
    }
}