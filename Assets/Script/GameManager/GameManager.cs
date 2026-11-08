using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// Singleton: มี instance เดียวในเกม ให้ Player/Enemy/ระบบอื่นเรียกอัปเดต UI ได้จาก GameManager.Instance
// โดยไม่ต้องลาก reference ของ UI ไปส่งต่อกันไปมาระหว่าง object
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    // ระบบ Currency (Gold) ก็เป็นสถานะรวมของเกมเหมือน Exp/Level: enemy แค่ "ดรอป" ให้ ไม่ได้ถือเงินเป็นของตัวเอง
    private int gold;
    public int Gold => gold;

    // ระบบ Exp/Level ของผู้เล่นย้ายมาอยู่ที่นี่: มันคือสถานะความก้าวหน้ารวมของเกม ไม่ใช่สถานะเฉพาะตัวของ Character แต่ละตัว
    // (enemy ไม่มีเลเวลเป็นของตัวเอง มันแค่ "ให้" exp เวลาตาย) จึงเหมาะกับ Singleton ตัวเดียวที่ทุกคนเรียกเข้ามาได้ มากกว่าผูกไว้กับ Character
    private int experiencePoints;
    private int level = 1;

    public int Level => level;
    public int ExpToNextLevel => level * 100;

    [SerializeField] private int startLive = 3;
    private int life;
    public int Life { get { return life; } set { life = value; } }

    [SerializeField] private float respawnDelay = 2f;
    private Vector3 checkPoint;

    public Player player;

    private void Awake()
    {
        // กันไม่ให้มี GameManager ซ้ำสองตัวในซีนเดียวกัน ถ้ามีอยู่แล้วให้ทำลายตัวที่เพิ่งสร้างทิ้ง
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
        checkPoint = player.transform.position;

        UIManager.Instance.UpdateExpUI(experiencePoints, ExpToNextLevel);
        UIManager.Instance.UpdateLevelUI(level);
        UIManager.Instance.UpdateGoldUI(gold);
        UIManager.Instance.UpdateLifeUI(life);
    }

    public void AddGold(int amount)
    {
        gold += amount;
        Debug.Log($"Gained {amount} gold. Total: {gold}");
        UIManager.Instance.UpdateGoldUI(gold);
    }

    public void GainExperience(int exp)
    {
        experiencePoints += exp;
        Debug.Log($"Player gained {exp} experience points.");
        CheckLevelUp();
        UIManager.Instance.UpdateExpUI(experiencePoints, ExpToNextLevel);
        UIManager.Instance.UpdateLevelUI(level);
    }

    public void SetCheckPoint(Vector3 pos)
    {
        checkPoint = pos;
    }

    public void OnPlayerDeath()
    {
        life--;
        UIManager.Instance.UpdateLifeUI(life);
        if ( life <= 0 )
        {
            // Game Over!!
            UIManager.Instance.SetVisibleLosePanel(true);
        }
        else
        {
            // Respawn Player
            StartCoroutine(Respawn());
        }
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(respawnDelay);

        if(player != null )
        {
            SetEnablePlayer(true);
            player.RespawnAt(checkPoint);
        }
    }

    public void SetEnablePlayer(bool enable)
    {
        if(player != null)
        {
            player.enabled = enable;
        }
    }

    private void CheckLevelUp()
    {
        while (experiencePoints >= ExpToNextLevel)
        {
            experiencePoints -= ExpToNextLevel;
            level++;
            Debug.Log($"Leveled up to Level {level}!");
        }
    }

    public void OnEnterWinZone()
    {
        // Win Condition
        SetEnablePlayer(false);
        UIManager.Instance.SetVisibleWinPanel(true);
    }
}