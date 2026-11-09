using UnityEngine;

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
        player = FindAnyObjectByType<Player>();
        
        // กำหนดจุดเกิดเริ่มต้นที่ตำแหน่งตัวละครตอนเริ่มเกม
        if (player != null)
        {
            checkPoint = player.transform.position;
        }

        // อัปเดต UI ชีวิตเริ่มต้น
        UpdateLifeUI();
    }

    // ==========================================
    // CHECKPOINT & RESPAWN METHODS
    // ==========================================
    public void SetCheckPoint(Vector3 pos)
    {
        checkPoint = pos;
        Debug.Log("Checkpoint Updated at: " + pos);
    }

    // ฟังก์ชันจัดการเมื่อ Player โดนโจมตี (หักหัวใจ และพากลับ Checkpoint ทันที)
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
            // เสียหัวใจ 1 ดวง: วาร์ปกลับจุด Checkpoint ทันทีแบบไม่มีดีเลย์!
            RespawnInstantly();
        }
    }

    // ฟังก์ชันวาร์ปกลับจุดเกิดทันทีในเฟรมเดียวกัน
    private void RespawnInstantly()
    {
        if (player != null)
        {
            player.RespawnAt(checkPoint);
            Debug.Log("Player respawned instantly at checkpoint!");
        }
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