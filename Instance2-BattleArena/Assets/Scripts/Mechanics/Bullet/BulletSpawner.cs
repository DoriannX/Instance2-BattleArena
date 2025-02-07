using Unity.Netcode;
using UnityEngine;
using UnityEngine.Pool;

public class BulletSpawner : NetworkBehaviour
{
    [Header("References")]
    [SerializeField] private Bullet bulletPrefab;
    [SerializeField] private Transform bulletSpawnPoint;

    private ObjectPool<Bullet> _bulletPool;

    private void Awake()
    {
        _bulletPool = new ObjectPool<Bullet>(CreateBullet, OnTakeBullet, OnReturnBullet, OnDestroyBullet, true, 10, 20);
    }

    [ServerRpc]
    public void FireBulletServerRpc()
    {
        Debug.Log("instantiating bullet");
        FireBullet();
    }

    public void FireBullet()
    {
        Bullet bullet = _bulletPool.Get();
        NetworkObject bulletNetworkObject = bullet.GetComponent<NetworkObject>();
        bulletNetworkObject.SpawnWithOwnership(NetworkManager.ServerClientId);
    }

    private Bullet CreateBullet()
    {
        Bullet bullet = Instantiate(bulletPrefab);
        //bullet.SetPool(_bulletPool);
        bullet.gameObject.SetActive(false);
        return bullet;
    }

    private void OnTakeBullet(Bullet bullet)
    {
        bullet.transform.position = bulletSpawnPoint.position;
        bullet.transform.rotation = bulletSpawnPoint.rotation;
        bullet.gameObject.SetActive(true);
    }

    private void OnReturnBullet(Bullet bullet)
    {
        bullet.gameObject.SetActive(false);
    }

    private void OnDestroyBullet(Bullet bullet)
    {
        Destroy(bullet.gameObject);
    }
}
