using Events;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;

namespace Mechanics.Bonus
{
    public class DamageEffectApplier : EffectApplier
    {
        [SerializeField] protected float _damageMultiplier = 1.2f;  
        [SerializeField] private float _duration = 10f;  

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
                playerStats.ApplyDamageBonus(_damageMultiplier, _duration);
                DispawnServerRpc(NetworkObjectId);
                
                EventManager.OnObjectUsed?.Invoke();
            }
        }
    }
}
