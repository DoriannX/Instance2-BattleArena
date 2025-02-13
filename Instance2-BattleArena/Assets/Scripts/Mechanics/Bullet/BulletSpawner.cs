using System;
using AudioSystem;
using Mechanics.Player;
using Mechanics.PlayerStats;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Pool;

public class BulletSpawner : NetworkBehaviour
{
    [SerializeField] private SoundData _soundData;

    [Header("References")]
    [SerializeField] private Bullet_Obsolete bulletPrefab;
    [SerializeField] private Transform bulletSpawnPoint;  // Assurez-vous que cela soit bien assign� dans l'inspecteur
    private PlayerStats _playerStats;
    
    private ObjectPool<Bullet> _bulletPool;

    private void Awake()
    {
        _playerStats = GetComponent<PlayerStats>();
    }

    [ServerRpc]
    public void FireBulletServerRpc(ulong clientId)
    {
        Bullet_Obsolete bullet = Instantiate(bulletPrefab,bulletSpawnPoint.transform.position,Quaternion.identity);
        bullet.StartMove(transform.up, _playerStats);
        SoundManager.Instance.CreateSound().WithSoundData(_soundData).Play();
        NetworkObject bulletNetworkObject = bullet.GetComponent<NetworkObject>();

        bulletNetworkObject.Spawn();  // Assigne la balle au client qui a tir�.
        Debug.Log($"Bullet spawned for client {clientId}");
    }

    //TODO: faire la detection des collisions
    //TODO: mourir en cas de collision
}
