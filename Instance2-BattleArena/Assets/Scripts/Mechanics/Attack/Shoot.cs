using Unity.Netcode;
using UnityEngine;

namespace Mechanics.Attack
{
    public class Shoot : NetworkBehaviour
    {
        private static readonly int _attacking = Animator.StringToHash("Attack");
        private static readonly int _attack = Animator.StringToHash("SwitchAttack");
        private BulletSpawner _bulletSpawner;

        [Header("Animator")] [SerializeField] private Animator _animator;
        private int _switchAttack;

        private PlayerStats.PlayerStats _playerStats;
        private float _fireRate;
        private float _timeSinceLastShot;

        private void Awake()
        {
            _playerStats = GetComponent<PlayerStats.PlayerStats>();
            if (_playerStats == null)
            {
                return;
            }

            if (_playerStats.AttackSpeed > 0)
            {
                _fireRate = 1f / Mathf.Max(_playerStats.AttackSpeed, 0.1f);
            }
            else
            {
                _fireRate = 0.1f;
            }
            _bulletSpawner = GetComponent<BulletSpawner>();
        }

        private void Start()
        {

            if (_bulletSpawner == null)
            {
                Debug.LogError("BulletSpawner component not found!");
            }
        }

        private void Update()
        {
            if (!IsOwner || !Input.GetButtonDown("Fire1") || _timeSinceLastShot + _fireRate > Time.time)
            {
                return;
            }

            _bulletSpawner.FireBulletServerRpc(OwnerClientId);
            _timeSinceLastShot = Time.time;
            AskAnimateServerRpc();
        }
        
        [ServerRpc(RequireOwnership = false)]
        private void AskAnimateServerRpc()
        {
            AnimateClientRpc();
            _animator.SetTrigger(_attacking);
        }

        [ClientRpc]
        private void AnimateClientRpc()
        {
            _switchAttack++;
            if (_switchAttack >= 2)
            {
                _switchAttack = 0;
            }
            _animator.SetInteger(_attack, _switchAttack);
            _animator.SetTrigger(_attacking);
        }
    }
}