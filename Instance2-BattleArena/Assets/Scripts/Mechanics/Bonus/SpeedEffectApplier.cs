using Events;
using Mechanics.Movements;
using Unity.Netcode;

namespace Mechanics.Bonus
{
    public class SpeedEffectApplier : EffectApplier
    {
        [ClientRpc]
        public override void ApplyEffectClientRpc(ulong playerId)
        {
            NetworkManager.Singleton.SpawnManager.SpawnedObjects.TryGetValue(playerId, out NetworkObject player);
            if (player == null)
            {
                return;
            }
            PlayerMovements playerMovements = player.GetComponent<PlayerMovements>();
            if (playerMovements != null)
            {
                playerMovements.ApplyMovementBoost();
                DispawnServerRpc(NetworkObjectId);
                
                EventManager.OnObjectUsed?.Invoke();
            }
        }
    }
}
