using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExpManager : MonoBehaviour
{
    [Header("Character Settings")]
    private PlayerClassManager.CharacterClass _currentClass;
    private SpriteRenderer _playerSpriteRenderer;
    private PlayerStats _playerStats;

    [Header("ProgressBar Settings")]
    [SerializeField] private int _level = 1;
    [SerializeField] private int _currentExp = 0;
    [SerializeField] private int _expToLevel = 100;
    [SerializeField] private int _addExpToNextLevel = 50;
    [SerializeField] private int _maxLevel = 30;

    [Header("ProgressBar UI")]
    [SerializeField] private Slider _slider;
    [SerializeField] private TextMeshProUGUI _currentLevelText;

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
        _currentExp -= _expToLevel;
        _expToLevel += _addExpToNextLevel;

        CheckForSpriteChange();
        ApplyStatsBoost();
    }

    private void CheckForSpriteChange()
    {
        if (_playerSpriteRenderer == null || _currentClass == null) return;

        if (_level >= 10 && _currentClass.Level10Sprite != null)
        {
            _playerSpriteRenderer.sprite = _currentClass.Level10Sprite;
        }
        else if (_level >= 20 && _currentClass.Level20Sprite != null)
        {
            _playerSpriteRenderer.sprite = _currentClass.Level20Sprite;
        }
    }

    private void ApplyStatsBoost()
    {
        if (_playerStats == null) return;

        _playerStats.IncreaseStats(); 
    }

    private void Update()
    {
        UpdateUI();

        if (Input.GetKeyUp(KeyCode.Escape))
        {
            GainExperience(50);
        }

        if (Input.GetKeyUp(KeyCode.H))
        {
            ResetProgress();
        }
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
