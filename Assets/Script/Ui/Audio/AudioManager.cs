using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Audio Sources")]
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource sfxSource;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void PlayBGM(AudioClip clip)
    {
        if (clip == null) return;
        if (bgmSource.clip == clip && bgmSource.isPlaying) return;

        bgmSource.clip = clip;
        bgmSource.loop = true;
        bgmSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip == null) return;
        sfxSource.PlayOneShot(clip);
    }

    // แยกปรับเสียง BGM
    public void SetBGMVolume(float volume)
    {
        if (bgmSource != null)
        {
            bgmSource.volume = volume;
        }
    }

    // แยกปรับเสียง SFX
    public void SetSFXVolume(float volume)
    {
        if (sfxSource != null)
        {
            sfxSource.volume = volume;
        }
    }

    public float GetBGMVolume()
    {
        return bgmSource != null ? bgmSource.volume : 1f;
    }

    public float GetSFXVolume()
    {
        return sfxSource != null ? sfxSource.volume : 1f;
    }
}