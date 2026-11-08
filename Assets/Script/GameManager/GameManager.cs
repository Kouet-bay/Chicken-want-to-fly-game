using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Player Lives & Checkpoint")]
    [SerializeField] private int startLive = 3;
    private int life;
    public int Life => life;

    [SerializeField] private float respawnDelay = 2f;
    private Vector3 checkPoint;

    public Player player;

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
        life = startLive;
        player = FindAnyObjectByType<Player>();
        
        if (player != null)
        {
            checkPoint = player.transform.position;
        }

        // อัปเดต UI ชีวิตเริ่มต้น
        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateLifeUI(life);
        }
    }

    public void SetCheckPoint(Vector3 pos)
    {
        checkPoint = pos;
        Debug.Log("Checkpoint Updated: " + pos);
    }

    // ฟังก์ชันใหม่สำหรับหักหัวใจเมื่อ Player โดนตี
    public void OnPlayerTakeDamage()
    {
        life--;
        life = Mathf.Max(life, 0);

        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateLifeUI(life);
        }

        if (life <= 0)
        {
            // Game Over!! หัวใจหมด
            if (UIManager.Instance != null)
            {
                UIManager.Instance.SetVisibleLosePanel(true);
            }
            SetEnablePlayer(false);
        }
        else
        {
            // Respawn กลับจุด Checkpoint ทันทีเมื่อเสียหัวใจไป 1 ดวง
            StartCoroutine(Respawn());
        }
    }

    public void OnPlayerDeath()
    {
        // เผื่อกรณีตายทันทีจากเคสอื่น
        OnPlayerTakeDamage();
    }

    private IEnumerator Respawn()
    {
        SetEnablePlayer(false); // ปิดการควบคุมระหว่างรอเกิด
        yield return new WaitForSeconds(respawnDelay);

        if (player != null)
        {
            player.RespawnAt(checkPoint);
            SetEnablePlayer(true);
        }
    }

    public void SetEnablePlayer(bool enable)
    {
        if (player != null)
        {
            player.enabled = enable;
        }
    }

    public void OnEnterWinZone()
    {
        // Win Condition
        SetEnablePlayer(false);
        if (UIManager.Instance != null)
        {
            UIManager.Instance.SetVisibleWinPanel(true);
        }
    }
}