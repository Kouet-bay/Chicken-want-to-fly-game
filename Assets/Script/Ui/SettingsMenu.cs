using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [Header("Audio Sliders")]
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider sfxSlider;

    void Start()
    {
        // ดึงค่าเสียงปัจจุบันมาแสดงที่ Slider เมื่อเปิดหน้า Setting
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
}