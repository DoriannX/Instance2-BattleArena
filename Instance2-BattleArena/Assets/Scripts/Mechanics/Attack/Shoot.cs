using Unity.Netcode;
using UnityEngine;

namespace Mechanics.Attack
{
    public class Shoot : NetworkBehaviour
    {
        [Header("References")] private BulletSpawner _bulletSpawner;
        [HideInInspector] public Transform PlayerTransform;
        public Bullet BulletPrefab;

        [Header("Animator")] [SerializeField] private Animator _animator;
        private int _switchAttack = 0;

        private PlayerStats _playerStats;
        private float _fireRate;
        private float _timeSinceLastShot;

        private void Awake()
        {
            PlayerTransform = transform;
            _playerStats = GetComponent<PlayerStats>();
            PlayerTransform = transform;
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
            if (!IsOwner || !Input.GetButtonDown("Fire1"))
            {
                return;
            }

            // Demande au serveur de tirer une balle pour ce client
            _bulletSpawner.FireBulletServerRpc(OwnerClientId);
            _animator.SetBool("IsAttacking", true);
        }

        private void ResetAnimFire()
        {
            _switchAttack++;
            _animator.SetBool("IsAttacking", false);
            if (_switchAttack >= 2)
            {
                _switchAttack = 0;
            }

            _animator.SetInteger("SwitchAttack", _switchAttack);
        }
    }
}