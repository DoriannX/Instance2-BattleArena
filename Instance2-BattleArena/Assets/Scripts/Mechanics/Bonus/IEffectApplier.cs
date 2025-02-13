using Unity.Netcode;
using UnityEngine;

namespace Mechanics.Bonus
{
    public interface IEffectApplier
    {
        public void ApplyEffectClientRpc(ulong playerId);
    }
}