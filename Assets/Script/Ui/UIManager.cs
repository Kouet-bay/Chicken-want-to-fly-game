using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    [Header("Health UI")]
    [SerializeField] private Image healthFillImage; // ตั้ง Image Type = Filled ใน Inspector แล้วปรับ fillAmount (0-1) แทนการใช้ Slider
    [SerializeField] private TMP_Text healthText;

    [Header("Exp UI")]
    [SerializeField] private Image expFillImage;
    [SerializeField] private TMP_Text expText;

    [Header("Level UI")]
    [SerializeField] private TMP_Text levelText;

    [Header("Gold UI")]
    [SerializeField] private TMP_Text goldText;

    [Header("Life UI")]
    [SerializeField] private TMP_Text lifeText;

    [Header("End Game Panels")]
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject losePanel;

    public void UpdateHealthUI(int currentHp, int maxHp)
    {
        if (healthFillImage != null && maxHp > 0)
        {
            healthFillImage.fillAmount = (float)currentHp / (float)maxHp;
        }
        if (healthText != null)
        {
            healthText.text = $"{currentHp} / {maxHp}";
        }
    }

    public void UpdateExpUI(int currentExp, int expToNextLevel)
    {
        if (expFillImage != null && expToNextLevel > 0)
        {
            expFillImage.fillAmount = (float)currentExp / expToNextLevel;
        }
        if (expText != null)
        {
            expText.text = $"{currentExp} / {expToNextLevel}";
        }
    }

    public void UpdateLevelUI(int level)
    {
        if (levelText != null)
        {
            levelText.text = $"{level}";
        }
    }

    public void UpdateGoldUI(int gold)
    {
        if (goldText != null)
        {
            goldText.text = $"{gold}";
        }
    }

    public void UpdateLifeUI(int life)
    {
        if (lifeText != null)
        {
            lifeText.text = $"{life}";
        }
    }

    public void SetVisibleWinPanel(bool visible)
    {
        if(winPanel != null)
        {
            winPanel.SetActive(visible);
        }
    }

    public void SetVisibleLosePanel(bool visible)
    {
        if(losePanel != null)
        {
            losePanel.SetActive(visible);
        }
    }
}