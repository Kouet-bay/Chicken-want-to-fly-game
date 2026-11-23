using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [Header("Audio Sliders")]
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider sfxSlider;

    [Header("Mouse Sensitivity Slider")]
    [SerializeField] private Slider mouseSensitivitySlider;

    void Start()
    {
        // 1. จัดการเรื่องเสียง (ดึงค่าปัจจุบันจาก AudioManager ที่จำค่าไว้แล้ว)
        if (AudioManager.Instance != null)
        {
            if (bgmSlider != null)
            {
                float currentBgm = AudioManager.Instance.GetBGMVolume();
                bgmSlider.SetValueWithoutNotify(currentBgm);
                bgmSlider.onValueChanged.RemoveAllListeners();
                bgmSlider.onValueChanged.AddListener(OnBGMVolumeChanged);
            }

            if (sfxSlider != null)
            {
                float currentSfx = AudioManager.Instance.GetSFXVolume();
                sfxSlider.SetValueWithoutNotify(currentSfx);
                sfxSlider.onValueChanged.RemoveAllListeners();
                sfxSlider.onValueChanged.AddListener(OnSFXVolumeChanged);
            }
        }
        else
        {
            Debug.LogWarning("SettingsMenu: AudioManager.Instance is null!");
        }

        // 2. จัดการเรื่องความไวเมาส์ (ดึงค่าที่เคยเซฟไว้มาแสดง)
        if (mouseSensitivitySlider != null)
        {
            float savedSensitivity = PlayerPrefs.GetFloat("MouseSensitivity", 2.0f);
            mouseSensitivitySlider.SetValueWithoutNotify(savedSensitivity);
            mouseSensitivitySlider.onValueChanged.RemoveAllListeners();
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

    private void OnMouseSensitivityChanged(float value)
    {
        PlayerPrefs.SetFloat("MouseSensitivity", value);
        PlayerPrefs.Save();

        Debug.Log("Mouse Sensitivity Updated: " + value);
    }
}