using UnityEngine.Assertions;
using UnityEngine;
using UnityEngine.SceneManagement;
using AudioSystem;
using DG.Tweening;

public class MenuButtonManager : MonoBehaviour
{
    [SerializeField] private GameObject _settings;
    [SerializeField] private GameObject _credits;
    [SerializeField] private GameObject _controlsUI;
    [SerializeField] private Transform _sliderSkin;

    [SerializeField] private SoundData _soundData;

    private float _distance = 300f;

    public void Awake()
    {
        Assert.IsNotNull(_settings, "_settings is null");
        Assert.IsNotNull(_credits, "_credits is null");
        Assert.IsNotNull(_controlsUI, "_controlsUI is null");
        Assert.IsNotNull(_soundData, "_soundData is null");
    }

    public void StartGame()
    {
        SceneManager.LoadScene("DevScene");
    }

    public void OpenSettings()
    {
        SoundManager.Instance.CreateSound().WithSoundData(_soundData).Play();
        _settings.SetActive(true);
    }

    public void CloseSettings()
    {
        SoundManager.Instance.CreateSound().WithSoundData(_soundData).Play();
        _settings.SetActive(false);
    }

    public void OpenCredits()
    {
        _credits.SetActive(true);
        _controlsUI.SetActive(false);
    }

    public void CloseCredits()
    {
        _credits.SetActive(false);
        _controlsUI.SetActive(true);
    }

    public void Respawn()
    {
        SoundManager.Instance.CreateSound().WithSoundData(_soundData).Play();
        SceneManager.LoadScene("DevScene");
    }

    public void MainMenu()
    {
        SoundManager.Instance.CreateSound().WithSoundData(_soundData).Play();
        SceneManager.LoadScene("MainMenu");
    }


    public void RightPanelSkin()
    {
        _sliderSkin.DOMoveX(_sliderSkin.position.x - _distance, 0.1f);
    }

    public void LeftPanelSkin()
    {
        _sliderSkin.DOMoveX(_sliderSkin.position.x + _distance, 0.1f);
    }
}