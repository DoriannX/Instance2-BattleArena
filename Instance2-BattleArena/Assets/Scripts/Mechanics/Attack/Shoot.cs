using Unity.Netcode;
using UnityEngine;

public class Shoot : NetworkBehaviour
{
    [Header("References")]
    public GameObject BulletPrefab;
    public Transform PlayerTransform;

    private BulletSpawner _bulletSpawner;

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
        if (IsOwner && Input.GetButtonDown("Fire1"))
        {
            FireBullet();
        }
    }

    private void FireBullet()
    {
        if (IsServer)
        {
            _bulletSpawner.FireBullet();  // Call the FireBullet method directly on the server
        }
        else
        {
            _bulletSpawner.FireBulletServerRpc();  // Request the server to fire the bullet
        }
    }
}
