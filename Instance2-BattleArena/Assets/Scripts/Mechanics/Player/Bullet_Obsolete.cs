using System;
using Unity.Netcode;
using UnityEngine;

namespace Mechanics.Player
{
    public class Bullet_Obsolete : NetworkBehaviour
    {
        private Transform _transform;
        [SerializeField] private float _speed;
        private PlayerStats _owner;
        private Vector3 _direction;
        private bool _move = false;

        private void Start()
        {
            _transform = transform;
        }

        public void StartMove(Vector3 direction, PlayerStats owner)
        {
            _direction = direction;
            _move = true;
            _owner = owner;
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
            Debug.Log("Bullet collided with " + collision.name);
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
                    int damage = _owner.Attack; // Utilise l'attaque du joueur qui a tir�
                    PlayerStats playerStats = collision.GetComponent<PlayerStats>();
                    if (playerStats != null)
                    {
                        playerStats.TakeDamage(damage);
                        Debug.Log($"Bullet dealt {damage} damage to {playerStats.name}. He has {playerStats.CurrentHealth} health left.");
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