using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [Header("UI Sliders")]
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Slider mouseSensitivitySlider;

    [Header("Audio Mixer (แนะนำให้ใช้)")]
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private string musicParameter = "MusicVolume"; // ตรงกับ Exposed Parameter ใน AudioMixer
    [SerializeField] private string sfxParameter = "SFXVolume";     // ตรงกับ Exposed Parameter ใน AudioMixer

    [Header("Optional: Direct AudioSources (ถ้าไม่ได้ใช้ AudioMixer)")]
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource sfxSource;

    public static float mouseSensitivity = 2f;

    private const string MUSIC_KEY = "MusicVolume";
    private const string SFX_KEY = "SFXVolume";
    private const string SENS_KEY = "MouseSensitivity";

    private const float DEFAULT_MUSIC = 0.75f;
    private const float DEFAULT_SFX = 0.75f;
    private const float DEFAULT_SENS = 2f;

    void Start()
    {
        // 1. โหลดค่าเดิมทั้งหมดจาก PlayerPrefs
        float savedMusic = PlayerPrefs.GetFloat(MUSIC_KEY, DEFAULT_MUSIC);
        float savedSfx = PlayerPrefs.GetFloat(SFX_KEY, DEFAULT_SFX);
        float savedSens = PlayerPrefs.GetFloat(SENS_KEY, DEFAULT_SENS);

        mouseSensitivity = savedSens;

        // 2. ตั้งค่า Slider โดยไม่ให้ยิง Event ทับค่าตอนเริ่มฉาก และผูก Listener ให้ปรับได้ทั้งใน Main Menu และในเกม
        if (musicSlider != null)
        {
            musicSlider.SetValueWithoutNotify(savedMusic);
            musicSlider.onValueChanged.RemoveAllListeners();
            musicSlider.onValueChanged.AddListener(SetMusicVolume);
        }

        if (sfxSlider != null)
        {
            sfxSlider.SetValueWithoutNotify(savedSfx);
            sfxSlider.onValueChanged.RemoveAllListeners();
            sfxSlider.onValueChanged.AddListener(SetSFXVolume);
        }

        if (mouseSensitivitySlider != null)
        {
            mouseSensitivitySlider.SetValueWithoutNotify(savedSens);
            mouseSensitivitySlider.onValueChanged.RemoveAllListeners();
            mouseSensitivitySlider.onValueChanged.AddListener(SetMouseSensitivity);
        }

        // 3. นำค่าเสียงที่โหลดได้ไปใช้ทันทีตอนเริ่มฉาก
        ApplyMusicVolume(savedMusic);
        ApplySFXVolume(savedSfx);
    }

    // เรียกได้ทั้งจาก Slider Listener หรือลากใส่ On Value Changed เอง
    public void SetMusicVolume(float value)
    {
        ApplyMusicVolume(value);
        PlayerPrefs.SetFloat(MUSIC_KEY, value);
        PlayerPrefs.Save();
    }

    public void SetSFXVolume(float value)
    {
        ApplySFXVolume(value);
        PlayerPrefs.SetFloat(SFX_KEY, value);
        PlayerPrefs.Save();
    }

    // ฟังก์ชันนี้ลากไปใส่ที่ช่อง On Value Changed ของ Slider ความไวเมาส์ได้เหมือนเดิม
    public void SetMouseSensitivity(float sensitivity)
    {
        mouseSensitivity = sensitivity;
        PlayerPrefs.SetFloat(SENS_KEY, sensitivity);
        PlayerPrefs.Save();
        Debug.Log("Mouse Sensitivity Updated: " + sensitivity);
    }

    private void ApplyMusicVolume(float sliderValue)
    {
        if (audioMixer != null)
        {
            float db = sliderValue <= 0.0001f ? -80f : Mathf.Log10(sliderValue) * 20f;
            audioMixer.SetFloat(musicParameter, db);
        }

        if (bgmSource != null)
        {
            bgmSource.volume = sliderValue;
        }
    }

    private void ApplySFXVolume(float sliderValue)
    {
        if (audioMixer != null)
        {
            float db = sliderValue <= 0.0001f ? -80f : Mathf.Log10(sliderValue) * 20f;
            audioMixer.SetFloat(sfxParameter, db);
        }

        if (sfxSource != null)
        {
            sfxSource.volume = sliderValue;
        }
    }
}