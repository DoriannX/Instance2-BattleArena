using UnityEngine;
using UnityEngine.UI;

public enum AudioType
{
    music,
    walk,
    shoot,
    takeDamage,
    heal,
    kill,
    boost,
    score,
    levelUp,
    explosion,
    button,
    ultimateReady,
    ultimate1,
    ultimate2,
    ultimate3,
    seedPickUp,
    slash,
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance; 

    public Slider musicSlider;
    public Slider sfxSlider;

    [System.Serializable]
    public struct MusicAudioData
    {
        public AudioType Type;
        public AudioSource Source;
    }

    [System.Serializable]
    public struct SFXAudioData
    {
        public AudioType Type;
        public AudioSource Source;
    }

    public MusicAudioData[] MusicAudioDataList;
    public SFXAudioData[] SFXAudioDataList;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        musicSlider.onValueChanged.AddListener(OnMusicChange);
        sfxSlider.onValueChanged.AddListener(OnSFXChange);
    }

    public void PlayMusicSound(AudioType type)
    {
        MusicAudioData data = GetMusicAudioData(type);
        data.Source.Play();
    }

    public void StopMusicSound(AudioType type)
    {
        MusicAudioData data = GetMusicAudioData(type);
        data.Source.Stop();
    }

    public void PlaySFXSound(AudioType type)
    {
        SFXAudioData data = GetSFXAudioData(type);
        data.Source.Play();
    }

    public void StopSFXSound(AudioType type)
    {
        SFXAudioData data = GetSFXAudioData(type);
        data.Source.Stop();
    }

    public MusicAudioData GetMusicAudioData(AudioType type)
    {
        for (int i = 0; i < MusicAudioDataList.Length; i++)
        {
            if (MusicAudioDataList[i].Type == type)
            {
                return MusicAudioDataList[i];
            }
        }
        Debug.LogError("MusicAudioManager: No clip found for type " + type);
        return new MusicAudioData();
    }

    public SFXAudioData GetSFXAudioData(AudioType type)
    {
        for (int i = 0; i < SFXAudioDataList.Length; i++)
        {
            if (SFXAudioDataList[i].Type == type)
            {
                return SFXAudioDataList[i];
            }
        }
        Debug.LogError("SFXAudioManager: No clip found for type " + type);
        return new SFXAudioData();
    }

    public void SetMusicVolume(float volume)
    {
        foreach(MusicAudioData data in MusicAudioDataList)
        {
            if (data.Source != null)
            {
                data.Source.volume = volume;
            }
        }
    }

    public void SetSFXVolume(float volume)
    {
        foreach(SFXAudioData data in SFXAudioDataList)
        {
            if (data.Source != null)
            {
                data.Source.volume = volume;
            }
        }
    }

    private void OnMusicChange(float volume)
    {
        volume = musicSlider.value;
        SetMusicVolume(volume);
    }

    private void OnSFXChange(float volume)
    {
        volume = sfxSlider.value;
        SetSFXVolume(volume);
    }
}