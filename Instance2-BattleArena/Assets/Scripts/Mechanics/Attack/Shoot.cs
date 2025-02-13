using Unity.Netcode;
using UnityEngine;

namespace Mechanics.Attack
{
    public class Shoot : NetworkBehaviour
    {
        private static readonly int _isAttacking = Animator.StringToHash("IsAttacking");
        private BulletSpawner _bulletSpawner;

        [Header("Animator")] [SerializeField] private Animator _animator;
        private int _switchAttack;

        private PlayerStats _playerStats;
        private float _fireRate;
        private float _timeSinceLastShot;

        private void Awake()
        {
            _playerStats = GetComponent<PlayerStats>();
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
        }

        private void Start()
        {
            _bulletSpawner = GetComponent<BulletSpawner>();

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
            _animator.SetBool(_isAttacking, true);
        }

        private void ResetAnimFire()
        {
            _switchAttack++;
            _animator.SetBool(_isAttacking, false);
            if (_switchAttack >= 2)
            {
                _switchAttack = 0;
            }

            _animator.SetInteger(_isAttacking, _switchAttack);
        }
    }
}