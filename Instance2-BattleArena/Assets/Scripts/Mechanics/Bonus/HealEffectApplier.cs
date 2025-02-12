using Unity.Netcode;
using UnityEngine;

namespace Mechanics.Bonus
{
    public class HealEffectApplier : EffectApplier
    {
        [ClientRpc]
        public override void ApplyEffectClientRpc(ulong playerId)
        {
            NetworkManager.Singleton.SpawnManager.SpawnedObjects.TryGetValue(playerId, out NetworkObject player);
            if (player == null)
            {
                return;
            }
            PlayerStats playerStats = player.GetComponent<PlayerStats>();

            if (playerStats != null)
            {
                HealServerRpc(playerId);
                DispawnServerRpc(NetworkObjectId);
            }
        }

        [ServerRpc(RequireOwnership = false)]
        private void HealServerRpc(ulong id)
        {
            NetworkManager.Singleton.SpawnManager.SpawnedObjects.TryGetValue(id, out NetworkObject player);
            if (player == null)
            {
                return;
            }
            PlayerStats playerStats = player.GetComponent<PlayerStats>();
            playerStats.ApplyHealBonus(75);
        }
    }
}
