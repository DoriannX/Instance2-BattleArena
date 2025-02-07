using Unity.Netcode;
using UnityEngine;

namespace Mechanics.Attack
{
    public class Shoot : NetworkBehaviour
    {
        [Header("References")] [SerializeField]
        private Bullet _bulletPrefab;

        private Transform _transform;

        private BulletSpawner _bulletSpawner;

        private void Awake()
        {
            _transform = transform;
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

            //_bulletSpawner.FireBulletServerRpc();
            AskServerSpawnBulletServerRpc(OwnerClientId);
        }

        [ServerRpc]
        private void AskServerSpawnBulletServerRpc(ulong ownerId)
        {
            Debug.Log("spawning bullet");
            Bullet spawnedBullet = Instantiate(_bulletPrefab,
                _transform.position + _transform.forward * _transform.localScale.magnitude, _transform.rotation);
            NetworkObject networkBullet = spawnedBullet.GetComponent<NetworkObject>();
            networkBullet.SpawnWithOwnership(networkBullet.OwnerClientId);
        }
    }
}