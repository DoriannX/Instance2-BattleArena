using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExpManager : MonoBehaviour
{
    public enum CharacterClass {ShieldTurkey,GunTurkey,PortTurkey }

    [Header("Character Settings")]
    [SerializeField] private CharacterClass _characterClass; // Test
    [SerializeField] private float _life; // Test
    [SerializeField] private float _damage; // Test

    [Header("ProgressBar Settings")]
    [SerializeField] private int _level;
    [SerializeField] private int _currentExp;
    [SerializeField] private int _expToLevel;
    [SerializeField] private int _addExpToNextLevel= 50;
    [SerializeField] private int _maxLevel;

    [Header("ProgressBar UI")]
    [SerializeField] private Slider _slider;
    [SerializeField] private TextMeshProUGUI _currentLevelText;

    [Header("Character Appearance")]
    [SerializeField] private SpriteRenderer _characterSprite; 
    [SerializeField] private Sprite _spriteLevel10;
    [SerializeField] private Sprite _spriteLevel20;

    private void Start()
    {
        UpdateUI();
    }

    public void GainExperience(int amount)
    {
        _currentExp += amount;
        if(_currentExp >= _expToLevel)
        {
            if (_level <= _maxLevel) 
            { 
                LevelUp();
            }
            else
            {
                return;
            }
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
        if (_level == 10 && _spriteLevel10 != null)
        {
            if (_characterSprite != null)
                _characterSprite.sprite = _spriteLevel10;
        }
        else if (_level == 20 && _spriteLevel20 != null)
        {
            if (_characterSprite != null)
                _characterSprite.sprite = _spriteLevel20;
        }
    }

    private void ApplyStatsBoost()
    {
        switch (_characterClass)
        {
            case CharacterClass.ShieldTurkey:
                _life = Mathf.Round(_life * 1.05f); // Test
                _damage = Mathf.Round(_damage * 1.05f); // Test
                break;
            case CharacterClass.GunTurkey:
                _life = Mathf.Round(_life * 1.05f); // Test
                _damage = Mathf.Round(_damage * 1.05f); // Test
                break;
            case CharacterClass.PortTurkey:
                _life = Mathf.Round(_life * 1.05f); // Test
                _damage = Mathf.Round(_damage * 1.05f); // Test
                break;
        }
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

    public void EnemyDead(int exp)
    {
        GainExperience(exp);  
        //_currentScore =+ score !! 
    }

    public void CollectSeed()
    {
        GainExperience(5);
    }

    public void ResetProgress()
    {
        _level = 1;
        _currentExp = 0;
        _expToLevel = _addExpToNextLevel; 

        //resetStatsClass !!

        UpdateUI(); 
    }

}
