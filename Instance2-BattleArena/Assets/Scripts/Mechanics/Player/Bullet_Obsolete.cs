using System;
using Unity.Netcode;
using UnityEngine;

namespace Mechanics.Player
{
    public class Bullet_Obsolete : NetworkBehaviour  
    {
        private Transform _transform;
        [SerializeField] private float _speed;
        private Vector3 _direction;
        private bool _move = false;

        private void Start()
        {
            _transform = transform;
        }

        public void StartMove(Vector3 direction)
        {
            _direction = direction;
            _move = true;
        }

        private void Move()
        {
            if (!_move)
            {
                return;
            }
            _transform.position += Time.deltaTime * _speed * _direction;
        }

        private void Update()
        {
            Move();
        }

        //To init
        public void SetDirection(Vector3 direction)
        {
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Wall"))
            {
                if (IsServer)
                {
                    Debug.Log("Bullet hit a wall and will be destroyed.");
                    GetComponent<NetworkObject>().Despawn();
                }
                else
                {
                    DestroyBulletServerRpc();
                }
            }

            if (collision.CompareTag("Player"))
            {
                if (IsServer)
                {
                    Debug.Log("Bullet hit a player.");
                    if (!NetworkManager.Singleton.ConnectedClients.ContainsKey(OwnerClientId))
                    {
                        Debug.LogError("Invalid OwnerClientId: " + OwnerClientId);
                        return;
                    }
                    PlayerStats shooterStats = NetworkManager.Singleton.ConnectedClients[OwnerClientId].PlayerObject.GetComponent<PlayerStats>();

                    if (shooterStats != null)
                    {
                        int damage = shooterStats.Attack;  // Utilise l'attaque du joueur qui a tir�
                        PlayerStats playerStats = collision.GetComponent<PlayerStats>();
                        if (playerStats != null)
                        {
                            playerStats.TakeDamage(damage);
                            Debug.Log($"Bullet dealt {damage} damage to the player.");
                        }
                    }

                    // D�truire la balle apr�s avoir inflig� des d�g�ts
                    GetComponent<NetworkObject>().Despawn();
                }
            }
        }

        [ServerRpc]
        private void DestroyBulletServerRpc()
        {
            Debug.Log("Server received request to destroy bullet.");
            GetComponent<NetworkObject>().Despawn();
        }
    }
}