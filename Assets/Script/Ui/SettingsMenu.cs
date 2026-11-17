using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [Header("Audio Sliders")]
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider sfxSlider;

    [Header("Mouse Sensitivity Slider")]
    [SerializeField] private Slider mouseSensitivitySlider; // เพิ่ม Slider ความไวเมาส์

    void Start()
    {
        // 1. จัดการเรื่องเสียง
        if (AudioManager.Instance != null)
        {
            if (bgmSlider != null)
            {
                bgmSlider.value = AudioManager.Instance.GetBGMVolume();
                bgmSlider.onValueChanged.AddListener(OnBGMVolumeChanged);
            }

            if (sfxSlider != null)
            {
                sfxSlider.value = AudioManager.Instance.GetSFXVolume();
                sfxSlider.onValueChanged.AddListener(OnSFXVolumeChanged);
            }
        }

        // 2. จัดการเรื่องความไวเมาส์ (ดึงค่าที่เคยเซฟไว้มาแสดง)
        if (mouseSensitivitySlider != null)
        {
            // โหลดค่าความไวเมาส์เดิม (ถ้ายังไม่เคยปรับ ให้ค่าเริ่มต้นเป็น 2.0 หรือตามที่คุณใช้)
            float savedSensitivity = PlayerPrefs.GetFloat("MouseSensitivity", 2.0f);
            mouseSensitivitySlider.value = savedSensitivity;
            
            // ผูก Event เมื่อเลื่อนสไลเดอร์
            mouseSensitivitySlider.onValueChanged.AddListener(OnMouseSensitivityChanged);
        }
    }

    private void OnBGMVolumeChanged(float value)
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.SetBGMVolume(value);
        }
    }

    private void OnSFXVolumeChanged(float value)
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.SetSFXVolume(value);
        }
    }

    // ฟังก์ชันบันทึกค่าความไวเมาส์ลง PlayerPrefs ทันทีที่เลื่อน
    private void OnMouseSensitivityChanged(float value)
    {
        PlayerPrefs.SetFloat("MouseSensitivity", value);
        PlayerPrefs.Save(); // บันทึกข้อมูลลงหน่วยความจำเครื่องทันที
        
        // (ถ้ามีสคริปต์ควบคุมมุมมองกล้องในหน้า Main Menu สามารถอัปเดตตรงนี้ได้ทันที)
        Debug.Log("Mouse Sensitivity Updated: " + value);
    }
}