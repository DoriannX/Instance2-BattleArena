using Mechanics.Player;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Pool;

public class BulletSpawner : NetworkBehaviour
{
    [Header("References")]
    [SerializeField] private Bullet_Obsolete bulletPrefab;
    [SerializeField] private Transform bulletSpawnPoint;  // Assurez-vous que cela soit bien assigné dans l'inspecteur

    private ObjectPool<Bullet> _bulletPool;

    public void FireBulletServerRpc(ulong clientId)
    {
        Bullet_Obsolete bullet = Instantiate(bulletPrefab,bulletSpawnPoint.transform.position,Quaternion.identity);
        bullet.StartMove(transform.up);
        NetworkObject bulletNetworkObject = bullet.GetComponent<NetworkObject>();

        bulletNetworkObject.Spawn();  // Assigne la balle au client qui a tiré.
        Debug.Log($"Bullet spawned for client {clientId}");
    }

    //TODO: faire la detection des collisions
    //TODO: mourir en cas de collision
}
