using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Unity.Netcode;

public class PlayerStats : NetworkBehaviour
{
    [Header("ProgressBar UI")]
    [SerializeField] private Slider _healthBar;

    public float AttackSpeed;
    public int Attack;
    public int CurrentHealth;
    public int MaxHealth;
    private int _initialAttack;
    private bool _isDamageBonusActive = false;

    private Coroutine _regenCoroutine;
    private float _timeSinceLastHit = 0f;
    private bool _isRegenerating = false;

    private void Start()
    {
        UpdateHealthBar();
    }

    private void Update()
    {
        UpdateHealthBar();

        if (!_isRegenerating)
        {
            _timeSinceLastHit += Time.deltaTime;

            if (_timeSinceLastHit >= 10f && _regenCoroutine == null)
            {
                _regenCoroutine = StartCoroutine(WaitAndStartRegeneration());
            }
        }
    }

    public void SetStats(int attack, int maxHealth ,float attackSpeed)
    {
        AttackSpeed = attackSpeed;
        Attack = attack;
        MaxHealth = maxHealth;
        CurrentHealth = MaxHealth;
        _initialAttack = attack;
        UpdateHealthBar();
    }

    public void IncreaseStats()
    {
        Attack = Mathf.RoundToInt(Attack * 1.05f);
        MaxHealth = Mathf.RoundToInt(MaxHealth * 1.05f);
        UpdateHealthBar();
    }

    public void ResetStats(int baseAttack, int baseMaxHealth)
    {
        Attack = baseAttack;
        MaxHealth = baseMaxHealth;
        CurrentHealth = MaxHealth;
        UpdateHealthBar();
    }

    public void ApplyHealBonus(float healAmount)
    {
        if (CurrentHealth < MaxHealth)
        {
            int healToApply = Mathf.RoundToInt(healAmount);
            CurrentHealth = Mathf.Min(CurrentHealth + healToApply, MaxHealth);
            UpdateHealthBar();
        }
    }

    public void ApplyDamageBonus(float damageMultiplier, float duration)
    {
        if (_isDamageBonusActive) return;

        _isDamageBonusActive = true;
        _initialAttack = Attack;
        Attack = Mathf.RoundToInt(Attack * damageMultiplier);

        StartCoroutine(RemoveDamageBonusAfterDuration(duration));
    }

    private IEnumerator RemoveDamageBonusAfterDuration(float duration)
    {
        yield return new WaitForSeconds(duration);
        Attack = _initialAttack;
        _isDamageBonusActive = false;
    }
    public void TakeDamage(int amount)
    {
        CurrentHealth -= amount;
        UpdateHealthBar();
        _isRegenerating = false;

        if (CurrentHealth <= 0)
        {
            if (IsServer)  
            {
                Debug.Log("Player died. Despawning...");
                NetworkObject networkObject = GetComponent<NetworkObject>();
                if (networkObject != null)
                {
                    networkObject.Despawn();
                }
            }
            else
            {
                RequestDespawnServerRpc();
            }
        }
    }

    public void UpdateHealthBar()
    {
        if (_healthBar != null)
        {
            _healthBar.value = (float)CurrentHealth / MaxHealth;
        }
    }

    private IEnumerator WaitAndStartRegeneration()
    {
        yield return new WaitForSeconds(5f); 

        if (_timeSinceLastHit >= 10f) 
        {
            StartCoroutine(RegenerateHealth());
        }
        else
        {
            _regenCoroutine = null;
        }
    }

    private IEnumerator RegenerateHealth()
    {
        _isRegenerating = true;

        while (CurrentHealth < MaxHealth && _isRegenerating)
        {
            CurrentHealth = Mathf.Min(CurrentHealth + 1, MaxHealth);
            UpdateHealthBar();
            yield return new WaitForSeconds(1f);
        }

        _isRegenerating = false;
        _regenCoroutine = null;
    }

    [ServerRpc(RequireOwnership = false)]
    private void RequestDespawnServerRpc()
    {
        NetworkObject networkObject = GetComponent<NetworkObject>();
        if (networkObject != null)
        {
            networkObject.Despawn();
        }
    }
}
