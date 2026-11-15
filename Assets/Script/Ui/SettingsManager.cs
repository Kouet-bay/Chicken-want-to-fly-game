using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [Header("UI Sliders")]
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Slider mouseSensitivitySlider;

    public static float mouseSensitivity = 2f; 

    void Start()
    {
        // โหลดค่าที่เคยเซฟไว้
        float defaultSens = PlayerPrefs.GetFloat("MouseSensitivity", 2f);
        mouseSensitivity = defaultSens;
        
        if (mouseSensitivitySlider != null)
        {
            mouseSensitivitySlider.value = defaultSens;
        }
    }

    // ฟังก์ชันนี้ให้ลากไปใส่ที่ช่อง On Value Changed ของ Slider ความไวเมาส์
    public void SetMouseSensitivity(float sensitivity)
    {
        mouseSensitivity = sensitivity;
        PlayerPrefs.SetFloat("MouseSensitivity", sensitivity);
        Debug.Log("Mouse Sensitivity Updated: " + sensitivity);
    }
}