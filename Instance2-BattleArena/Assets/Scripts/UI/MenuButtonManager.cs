using UnityEngine.Assertions;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtonManager : MonoBehaviour
{
    [SerializeField] private GameObject _settings;
    [SerializeField] private GameObject _credits;
    [SerializeField] private GameObject _login;
    [SerializeField] private GameObject _register;
    [SerializeField] private GameObject _controlsUI;

    public void Awake()
    {
        Assert.IsNotNull(_settings, "_settings is null");
        Assert.IsNotNull(_credits, "_credits is null");
        Assert.IsNotNull(_login, "_login is null");
        Assert.IsNotNull(_register, "_register is null");
        Assert.IsNotNull(_controlsUI, "_controlsUI is null");
    }

    public void StartGame()
    {
        SceneManager.LoadScene("DevScene");
    }

    public void OpenSettings()
    {
        AudioManager.Instance.PlaySFXSound(AudioType.heal);
        _settings.SetActive(true);
    }

    public void CloseSettings()
    {
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

    public void OpenRegisterMenu()
    {
        _register.SetActive(true);
        _login.SetActive(false);
    }

    public void OpenLoginMenu()
    {
        _login.SetActive(true);
        _register.SetActive(false);
    }

    public void Respawn()
    {
        SceneManager.LoadScene("DevScene");
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void Login()
    {

    }

    public void Register()
    {

    }
}
