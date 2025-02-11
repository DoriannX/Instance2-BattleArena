using System;
using System.Collections;
using Mechanics.Bonus;
using Unity.Netcode;
using UnityEngine;

namespace Mechanics.Trap
{
    public class BoxOfTacks : EffectApplier
    {
        public float DamagePerTick = 20f;
        public float TickInterval = 2f;
        public float TrapDuration = 10f;

        private bool _playerInside = false;
        private ulong _playerId;
        private bool _trapActive = false;

        [ClientRpc]
        public override void ApplyEffectClientRpc(ulong playerId)
        {
            Debug.Log($"player {playerId} is inside the trap");
            NetworkManager.Singleton.SpawnManager.SpawnedObjects.TryGetValue(playerId, out NetworkObject player);
            if (player == null)
            {
                Debug.LogWarning("Player not found");
                return;
            }

            _playerId = playerId;
            PlayerMovements playerMovements = player.GetComponent<PlayerMovements>();
            _playerInside = true;
            _trapActive = true;

            if (playerMovements != null)
            {
                playerMovements.ApplyMovementSlow();
            }
        }

        private void Update()
        {
            if (!_trapActive)
            {
                return;
            }

            DamagePlayerServerRpc(_playerId);
        }

        [ServerRpc(RequireOwnership = false)]
        private void DamagePlayerServerRpc(ulong id)
        {
            NetworkManager.Singleton.SpawnManager.SpawnedObjects.TryGetValue(id, out NetworkObject player);
            if (player == null)
            {
                return;
            }
            PlayerStats playerStats = player.GetComponent<PlayerStats>();
            playerStats.TakeDamage(20*Time.deltaTime);
            playerStats.AskUpdateHealthBarServerRpc();
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!IsServer || !other.CompareTag("Player")) return;
            _playerInside = false;

            PlayerMovements playerMovements = other.GetComponent<PlayerMovements>();
            if (playerMovements != null)
            {
                playerMovements.ResetMovementSpeed();
                NetworkObject.Despawn();
            }

            //Stop
        }
    }
}