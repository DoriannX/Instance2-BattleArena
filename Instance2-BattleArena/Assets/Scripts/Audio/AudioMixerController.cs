using UnityEngine;
using UnityEngine.Audio  ;

public class AudioMixerController : MonoBehaviour
{
    [SerializeField] private AudioMixer _audioMixer;

    public void SetVolumeSFX(float sliderValue)
    {
        _audioMixer.SetFloat("SFXVolume", Mathf.Log10(sliderValue)* 20);
    }

    public void SetVolumeMusic(float sliderValue)
    {
        _audioMixer.SetFloat("MusicVolume", Mathf.Log10(sliderValue) * 20);
    }
}