using Unity.Netcode;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

namespace Mechanics.Attack
{
    [RequireComponent(typeof(NetworkObject))]
    public class HitMelee : NetworkBehaviour
    {
        private static readonly int _attack = Animator.StringToHash("Attack");

        [Header("References")] [SerializeField]
        private InputActionReference _hitMelee;

        [SerializeField] private int _attackDamage = 20;
        [SerializeField] private float _attackSpeed = 0.2f;
        [SerializeField] private Animator _animator;

        private Collider2D _playerInRangeCollider2D;
        private bool _isInCollider;

        private float _timeSinceLastHit;

        private void Awake()
        {
            Assert.IsNotNull(_hitMelee, "_inputAction is missing");
        }

        private void OnEnable()
        {
            _hitMelee.action.started += Hit;
        }

        private void OnDisable()
        {
            _hitMelee.action.started -= Hit;
        }

        private void Update()
        {
            _timeSinceLastHit += Time.deltaTime;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                _playerInRangeCollider2D = other;
                _isInCollider = true;
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                _isInCollider = false;
                _playerInRangeCollider2D = null;
            }
        }

        private void Hit(InputAction.CallbackContext context)
        {
            Debug.Log("trying to hit");
            AskAnimateServerRpc();
            if (_isInCollider && _playerInRangeCollider2D != null && _timeSinceLastHit >= _attackSpeed)
            {
                Debug.Log("hit");
                _timeSinceLastHit = 0f;
                HitOnServerRpc(_playerInRangeCollider2D.GetComponent<NetworkObject>().OwnerClientId);                
            }
        }
        
        [ServerRpc(RequireOwnership = false)]
        private void AskAnimateServerRpc()
        {
            AnimateClientRpc();
            _animator.SetTrigger(_attack);
        }

        [ClientRpc]
        private void AnimateClientRpc()
        {
            Debug.Log("attacking animation");
            _animator.SetTrigger(_attack);
        }

        [ServerRpc(RequireOwnership = false)]
        private void HitOnServerRpc(ulong id)
        {
            Debug.Log("hit on server");
            NetworkManager.Singleton.ConnectedClients.TryGetValue(id, out NetworkClient playerClient);

            if (playerClient == null)
            {
                Debug.LogError("Player not found");
                return;
            }
            PlayerStats playerStats = playerClient.PlayerObject.GetComponent<PlayerStats>();

            if (playerStats != null)
            {
                playerStats.TakeDamage(playerStats.Attack);
                if(playerStats.CurrentHealth <= 0)
                {
                    GiveExperienceClientRpc(GetComponent<NetworkObject>().OwnerClientId);
                }
            }
        }

        [ClientRpc]
        private void GiveExperienceClientRpc(ulong id)
        {
            if (NetworkManager.Singleton.LocalClientId == id)
            {
                ExpManager.Instance.GainExperience(200);
            }
        }
    }
}