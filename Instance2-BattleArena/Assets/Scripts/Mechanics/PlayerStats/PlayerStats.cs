using System.Collections;
using UI;
using Unity.Netcode;
using AudioSystem;
using UnityEngine;
using UnityEngine.UI;

namespace Mechanics.PlayerStats
{
    public class PlayerStats : NetworkBehaviour
    {
        [SerializeField] private SoundData _soundData;
        [SerializeField] private SoundData _soundData2;

        [Header("ProgressBar UI")] [SerializeField]
        private Slider _healthBar;

        public float AttackSpeed;
        public int Attack;
        public float CurrentHealth;
        public int MaxHealth;
        private int _initialAttack;
        private bool _isDamageBonusActive = false;
        NetworkObject _networkObject;

        private Coroutine _regenCoroutine;
        private float _timeSinceLastHit = 0f;
        private bool _isRegenerating = false;

        [Header("Death Effect Prefab")] [SerializeField]
        private GameObject _deathEffectPrefab;

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
            if (IsServer)
            {
                CurrentHealth += healAmount;
                CurrentHealth = Mathf.Clamp(CurrentHealth, 0, MaxHealth);
                AskUpdateHealthBarServerRpc();

                ShowHealEffectClientRpc(new ClientRpcParams
                {
                    Send = new ClientRpcSendParams
                    {
                        TargetClientIds = new[] { _networkObject.OwnerClientId }
                    }
                });

                Debug.Log("Healing player for " + healAmount + " health points.");
            }
        }

        [ClientRpc]
        private void ShowHealEffectClientRpc(ClientRpcParams clientRpcParams = default)
        {
            ExpManager.Instance.DisableAllFeedbacks();
            if (ExpManager.Instance.HealScreenFeedBack != null)
            {
                ExpManager.Instance.HealScreenFeedBack.gameObject.SetActive(true);
                ExpManager.Instance.FeedBackIconHeal.gameObject.SetActive(true);
                StartCoroutine(DeactivateEffectAfterDuration(ExpManager.Instance.HealScreenFeedBack, 2f));
                StartCoroutine(DeactivateEffectAfterDuration(ExpManager.Instance.FeedBackIconHeal, 2f));
            }
        }

        public void ApplyDamageBonus(float damageMultiplier, float duration)
        {
            if (_isDamageBonusActive) return;

            _isDamageBonusActive = true;
            _initialAttack = Attack;
            Attack = Mathf.RoundToInt(Attack * damageMultiplier);

            StartCoroutine(RemoveDamageBonusAfterDuration(duration));
            ExpManager.Instance.DisableAllFeedbacks();
            if (ExpManager.Instance.DamageScreenFeedBack != null)
            {
                ExpManager.Instance.DamageScreenFeedBack.gameObject.SetActive(true);
                ExpManager.Instance.FeedBackIconAttack.gameObject.SetActive(true);
                StartCoroutine(DeactivateEffectAfterDuration(ExpManager.Instance.DamageScreenFeedBack, duration));
                StartCoroutine(DeactivateEffectAfterDuration(ExpManager.Instance.FeedBackIconAttack, duration));
            }
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
            SoundManager.Instance.CreateSound().WithSoundData(_soundData).Play();
        }

        public void TakeDamage(float amount)
        {
            if (IsServer)
            {
                CurrentHealth -= amount;
                SoundManager.Instance.CreateSound().WithSoundData(_soundData2).Play();
                AskUpdateHealthBarServerRpc();
                _isRegenerating = false;

                if (CurrentHealth <= 0)
                {
                    Debug.Log($"Player died {_networkObject}. Despawning...");
                    GameObject deadPrefab = Instantiate(_deathEffectPrefab, _networkObject.transform.position,
                        Quaternion.identity);
                    deadPrefab.GetComponent<NetworkObject>().Spawn();
                    SendPlayerDiedMessageClientRpc(_networkObject.OwnerClientId);
                    _networkObject.Despawn();
                }
            }
        }


        [ClientRpc]
        private void SendPlayerDiedMessageClientRpc(ulong clientId)
        {
            if (NetworkManager.Singleton.LocalClientId == clientId)
            {
                Debug.Log($"Player died {_networkObject}. Despawning...");
                EndMenuManager.Instance.Toggle();
            }
        }

        [ServerRpc(RequireOwnership = false)]
        public void AskUpdateHealthBarServerRpc()
        {
            UpdateHealthBarClientRpc(CurrentHealth, MaxHealth);
            _healthBar.value = CurrentHealth / MaxHealth;
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

        private IEnumerator DeactivateEffectAfterDuration(GameObject effect, float duration)
        {
            yield return new WaitForSeconds(duration);
            effect.gameObject.SetActive(false);
        }
    }
}