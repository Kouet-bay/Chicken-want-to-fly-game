using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    [Header("Health UI")]
    [SerializeField] private Image healthFillImage; 
    [SerializeField] private TMP_Text healthText;

    [Header("Life UI (3 Heart Icons)")]
    [SerializeField] private GameObject[] heartIcons;

    [Header("Stamina UI (Slider)")]
    [SerializeField] private Slider staminaSlider; // เปลี่ยนจาก Image มาเป็น Slider แทนครับ

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

    public void UpdateLifeUI(int life)
    {
        for (int i = 0; i < heartIcons.Length; i++)
        {
            if (heartIcons[i] != null)
            {
                if (i < life)
                {
                    heartIcons[i].SetActive(true);
                }
                else
                {
                    heartIcons[i].SetActive(false);
                }
            }
        }
    }

    // ฟังก์ชันอัปเดตหลอด Stamina แบบ Slider
    public void UpdateStaminaUI(float currentStamina, float maxStamina)
    {
        if (staminaSlider != null && maxStamina > 0)
        {
            // ตั้งค่า Min/Max ของ Slider ตามจริง และส่งค่าปัจจุบันเข้าไป
            staminaSlider.maxValue = maxStamina;
            staminaSlider.value = currentStamina;
        }
    }

    public void SetVisibleWinPanel(bool visible)
    {
        if (winPanel != null)
        {
            winPanel.SetActive(visible);
        }
    }

    public void SetVisibleLosePanel(bool visible)
    {
        if (losePanel != null)
        {
            losePanel.SetActive(visible);
        }
    }
}