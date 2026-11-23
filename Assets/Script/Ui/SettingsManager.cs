using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [Header("UI Sliders")]
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Slider mouseSensitivitySlider;

    public static float mouseSensitivity = 2f;

    private const string SENS_KEY = "MouseSensitivity";

    private void Awake()
    {
        if (musicSlider != null)
        {
            musicSlider.onValueChanged.AddListener(OnMusicSliderChanged);
        }

        if (sfxSlider != null)
        {
            sfxSlider.onValueChanged.AddListener(OnSfxSliderChanged);
        }

        if (mouseSensitivitySlider != null)
        {
            mouseSensitivitySlider.onValueChanged.AddListener(SetMouseSensitivity);
        }
    }

    private void OnEnable()
    {
        SyncUI();
    }

    private void Start()
    {
        SyncUI();
    }

    private void SyncUI()
    {
        float savedSens = PlayerPrefs.GetFloat(SENS_KEY, 2f);
        mouseSensitivity = savedSens;

        if (mouseSensitivitySlider != null)
        {
            mouseSensitivitySlider.SetValueWithoutNotify(savedSens);
        }

        if (AudioManager.Instance != null)
        {
            if (musicSlider != null)
            {
                musicSlider.SetValueWithoutNotify(AudioManager.Instance.GetBGMVolume());
            }

            if (sfxSlider != null)
            {
                sfxSlider.SetValueWithoutNotify(AudioManager.Instance.GetSFXVolume());
            }
        }
    }

    public void OnMusicSliderChanged(float value)
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.SetBGMVolume(value);
        }
    }

    public void OnSfxSliderChanged(float value)
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.SetSFXVolume(value);
        }
    }

    // ฟังก์ชันนี้ให้ลากไปใส่ที่ช่อง On Value Changed ของ Slider ความไวเมาส์ได้ หรือให้โค้ดผูกอัตโนมัติใน Awake ก็ได้
    public void SetMouseSensitivity(float sensitivity)
    {
        mouseSensitivity = sensitivity;
        PlayerPrefs.SetFloat(SENS_KEY, sensitivity);
        PlayerPrefs.Save();
        Debug.Log("Mouse Sensitivity Updated: " + sensitivity);
    }
}