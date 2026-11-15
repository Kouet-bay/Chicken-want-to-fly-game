using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [Header("Audio Sliders")]
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider sfxSlider;

    [Header("Gameplay Sliders")]
    [SerializeField] private Slider mouseSensitivitySlider;

    void Start()
    {
        // 1. โหลดค่าความไวเมาส์ที่เคยบันทึกไว้มาแสดงที่ Slider (ถ้าไม่มี ใช้ 1.0)
        if (mouseSensitivitySlider != null)
        {
            float savedSens = PlayerPrefs.GetFloat("MouseSensitivity", 1.0f);
            mouseSensitivitySlider.value = savedSens;
            
            // ผูกฟังก์ชันเมื่อมีการเลื่อน Slider
            mouseSensitivitySlider.onValueChanged.AddListener(OnMouseSensitivityChanged);
        }
    }

    // ฟังก์ชันนี้จะทำงานทันทีที่มีการเลื่อน Slider
    private void OnMouseSensitivityChanged(float value)
    {
        // บันทึกค่าลงใน PlayerPrefs ทันที
        PlayerPrefs.SetFloat("MouseSensitivity", value);
        PlayerPrefs.Save(); // สั่งบันทึกไฟล์ลงเครื่องชัวร์ๆ

        Debug.Log("🎚️ บันทึกค่าความไวเมาส์ใหม่ลงระบบ: " + value);
    }
}