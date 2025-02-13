using AudioSystem;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExpManager : MonoBehaviour
{

    [SerializeField] private SoundData _soundData;
    public static ExpManager Instance;
    [Header("Character Settings")]
    private PlayerClassManager.CharacterClass _currentClass;
    private SpriteRenderer _playerSpriteRenderer;
    private PlayerStats _playerStats;

    [Header("ProgressBar Settings")]
    [SerializeField] private int _level = 1;
    [SerializeField] private int _currentExp = 0;
    [SerializeField] private int _expToLevel = 100;
    [SerializeField] private int _addExpToNextLevel = 50;
    [SerializeField] private int _maxLevel = 20;

    [Header("ProgressBar UI")]
    [SerializeField] private Slider _slider;
    [SerializeField] private TextMeshProUGUI _currentLevelText;

    private void Awake()
    {
        if(Instance == null) Instance = this;
    }

    public void Initialize(PlayerClassManager.CharacterClass selectedClass, GameObject playerPrefab)
    {
        _currentClass = selectedClass;
        _playerSpriteRenderer = playerPrefab.GetComponent<SpriteRenderer>();
        _playerStats = playerPrefab.GetComponent<PlayerStats>(); 

        if (_playerSpriteRenderer != null && _currentClass != null)
        {
            _playerSpriteRenderer.sprite = _currentClass.BaseSprite;
        }

        UpdateUI();
    }

    public void GainExperience(int amount)
    {
        _currentExp += amount;

        while (_currentExp >= _expToLevel && _level < _maxLevel)
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        _level++;
        SoundManager.Instance.CreateSound().WithSoundData(_soundData).Play();
        _currentExp -= _expToLevel;
        _expToLevel += _addExpToNextLevel;

        ApplyStatsBoost();
    }

    private void ApplyStatsBoost()
    {
        if (_playerStats == null) return;

        int previousMaxHealth = _playerStats.MaxHealth;
        float previousHealth = _playerStats.CurrentHealth;

        _playerStats.IncreaseStats(); 

        int newMaxHealth = _playerStats.MaxHealth;
        int healthIncrease = newMaxHealth - previousMaxHealth;
        if (previousHealth == previousMaxHealth)
        {
            _playerStats.CurrentHealth = newMaxHealth;
        }
        else
        {
            _playerStats.CurrentHealth += healthIncrease;
            _playerStats.CurrentHealth = Mathf.Min(_playerStats.CurrentHealth, newMaxHealth);
        }
        _playerStats.AskUpdateHealthBarServerRpc();
    }


    private void Update()
    {
        UpdateUI();
    }

    private void UpdateUI()
    {
        _slider.maxValue = _expToLevel;
        _slider.value = _currentExp;
        _currentLevelText.text = "Level: " + _level;
    }

    public void ResetProgress()
    {
        _level = 1;
        _currentExp = 0;
        _expToLevel = 100;

        if (_playerSpriteRenderer != null && _currentClass != null)
        {
            _playerSpriteRenderer.sprite = _currentClass.BaseSprite;
        }

        if (_playerStats != null)
        {
            _playerStats.ResetStats(_currentClass.BaseAttack, _currentClass.BaseHeal);
        }

        UpdateUI();
    }
}
