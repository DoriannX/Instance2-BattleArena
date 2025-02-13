using AudioSystem;
using Events;
using Unity.Netcode;
using UnityEngine;

namespace Mechanics.Bonus
{
    public class HealEffectApplier : EffectApplier
    {

        [SerializeField] private SoundData _soundData;

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
                SoundManager.Instance.CreateSound().WithSoundData(_soundData).Play();
                HealServerRpc(playerId);
                DispawnServerRpc(NetworkObjectId);
                
                EventManager.OnObjectUsed?.Invoke();
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
