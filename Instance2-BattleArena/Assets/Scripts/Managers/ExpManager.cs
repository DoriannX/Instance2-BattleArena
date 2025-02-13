using AudioSystem;
using Managers;
using Unity.Netcode;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Managers;
using System.Collections;
using Mechanics.PlayerStats;

public class ExpManager : NetworkBehaviour
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

    [Header("Level-Up Effect")]
    [SerializeField] private GameObject levelUpEffectPrefab;

    private bool _isLevelUpEffectActive = false;

    private GameObject _playerInstance;


    public GameObject DamageScreenFeedBack;
    public GameObject HealScreenFeedBack;
    public GameObject MovementBoostEffect;
    public GameObject FeedBackIconAttack;
    public GameObject FeedBackIconSpeed;
    public GameObject FeedBackIconHeal;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void Initialize(PlayerClassManager.CharacterClass selectedClass, GameObject playerPrefab)
    {
        _currentClass = selectedClass;
        _playerSpriteRenderer = playerPrefab.GetComponent<SpriteRenderer>();
        _playerStats = playerPrefab.GetComponent<PlayerStats>();
        _playerInstance = playerPrefab;

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

        if (IsServer && !_isLevelUpEffectActive)
        {
            SpawnLevelUpEffect();
        }
        else
        {
            SpawnLevelUpEffectServerRpc();
        }
        ApplyStatsBoost();
    }

    private void SpawnLevelUpEffect()
    {
        if (levelUpEffectPrefab != null && !_isLevelUpEffectActive)
        {
            _isLevelUpEffectActive = true;
            GameObject effectInstance = Instantiate(levelUpEffectPrefab, _playerInstance.transform.position, levelUpEffectPrefab.transform.rotation);
            NetworkObject networkObject = effectInstance.GetComponent<NetworkObject>();

            if (networkObject != null)
            {
                networkObject.Spawn();
                effectInstance.transform.SetParent(_playerInstance.transform.GetChild(0));
                StartCoroutine(FollowPlayerForDuration(effectInstance, 1.5f));
            }
        }
    }

    private IEnumerator FollowPlayerForDuration(GameObject effectInstance, float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            if (effectInstance != null && _playerSpriteRenderer != null)
            {
                effectInstance.transform.position = _playerSpriteRenderer.transform.position;
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        if (effectInstance != null)
        {
            effectInstance.GetComponent<NetworkObject>().Despawn();
            Destroy(effectInstance);
            _isLevelUpEffectActive = false;
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void SpawnLevelUpEffectServerRpc()
    {
        SpawnLevelUpEffect();
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

    public void DisableAllFeedbacks()
    {
        if (HealScreenFeedBack != null)
            HealScreenFeedBack.SetActive(false);

        if (DamageScreenFeedBack != null)
            DamageScreenFeedBack.SetActive(false);

        if (MovementBoostEffect != null)
            MovementBoostEffect.SetActive(false);
    }
}
