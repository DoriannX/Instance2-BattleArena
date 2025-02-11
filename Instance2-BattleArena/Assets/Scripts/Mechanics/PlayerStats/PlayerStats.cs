using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections;
using Unity.Netcode;

public class PlayerStats : NetworkBehaviour
{
    [Header("ProgressBar UI")] [SerializeField]
    private Slider _healthBar;

    public float AttackSpeed;
    public int Attack;
    public int CurrentHealth;
    public int MaxHealth;
    private int _initialAttack;
    private bool _isDamageBonusActive = false;
    NetworkObject _networkObject;

    private Coroutine _regenCoroutine;
    private float _timeSinceLastHit = 0f;
    private bool _isRegenerating = false;

    private void Awake()
    {
        _networkObject = GetComponent<NetworkObject>();
    }

    private void Start()
    {
        AskUpdateHealthBarServerRpc();
    }

    private void Update()
    {
        AskUpdateHealthBarServerRpc();

        if (!_isRegenerating)
        {
            _timeSinceLastHit += Time.deltaTime;

            if (_timeSinceLastHit >= 10f && _regenCoroutine == null)
            {
                _regenCoroutine = StartCoroutine(WaitAndStartRegeneration());
            }
        }
    }

    public void SetStats(int attack, int maxHealth, float attackSpeed)
    {
        AttackSpeed = attackSpeed;
        Attack = attack;
        MaxHealth = maxHealth;
        CurrentHealth = MaxHealth;
        _initialAttack = attack;
        _initialHeal = maxHealth;
        AskUpdateHealthBarServerRpc();
    }

    public void IncreaseStats()
    {
        Attack = Mathf.RoundToInt(Attack * 1.05f);
        MaxHealth = Mathf.RoundToInt(MaxHealth * 1.05f);
        AskUpdateHealthBarServerRpc();
    }

    public void ResetStats(int baseAttack, int baseMaxHealth)
    {
        Attack = baseAttack;
        MaxHealth = baseMaxHealth;
        CurrentHealth = MaxHealth;
        AskUpdateHealthBarServerRpc();
    }

    public void ApplyHealBonus(float healAmount)
    {
        if (CurrentHealth < MaxHealth)
        {
            int healToApply = Mathf.RoundToInt(healAmount);
            CurrentHealth = Mathf.Min(CurrentHealth + healToApply, MaxHealth);
            AskUpdateHealthBarServerRpc();
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

    [ServerRpc(RequireOwnership = false)]
    private void KillServerRpc()
    {
        TakeDamage(CurrentHealth);
    }

    [ContextMenu("Kill")]
    private void Kill()
    {
        KillServerRpc();
    }

    public void TakeDamage(int amount)
    {
        CurrentHealth -= amount;
        AskUpdateHealthBarServerRpc();
        _isRegenerating = false;

        if (CurrentHealth <= 0)
        {
            Debug.Log($"Player died {_networkObject}. Despawning...");
            //TODO: comprendre pourquoi tous les joueurs / le joueur qui a tué ne peux plus bouger
            _networkObject.Despawn();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void AskUpdateHealthBarServerRpc()
    {
        UpdateHealthBarClientRpc(CurrentHealth, MaxHealth);
        _healthBar.value = (float)CurrentHealth / MaxHealth;
    }

    [ClientRpc]
    private void UpdateHealthBarClientRpc(float currentHealth, float maxHealth)
    {
        _healthBar.value = currentHealth / maxHealth;
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
            AskUpdateHealthBarServerRpc();
            yield return new WaitForSeconds(1f);
        }

        _isRegenerating = false;
        _regenCoroutine = null;
    }
}
