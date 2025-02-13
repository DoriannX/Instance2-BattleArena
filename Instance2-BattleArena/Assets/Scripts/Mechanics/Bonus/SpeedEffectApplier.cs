using AudioSystem;
using Events;
using Mechanics.Movements;
using Unity.Netcode;
using UnityEngine;

namespace Mechanics.Bonus
{
    public class SpeedEffectApplier : EffectApplier
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
            PlayerMovements playerMovements = player.GetComponent<PlayerMovements>();
            if (playerMovements != null)
            {
                SoundManager.Instance.CreateSound().WithSoundData(_soundData).Play();
                playerMovements.ApplyMovementBoost();
                DispawnServerRpc(NetworkObjectId);
                
                EventManager.OnObjectUsed?.Invoke();
            }
        }
    }
}
