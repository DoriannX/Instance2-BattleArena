using Mechanics.Bonus;
using Unity.Netcode;
using UnityEngine;

public class Seed : EffectApplier
{
    public override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
        if (IsServer)
        {
            GainExperienceSeedClientRpc(other.gameObject.GetComponent<NetworkObject>().OwnerClientId);
        }
    }

    [ClientRpc]
    public void GainExperienceSeedClientRpc(ulong playerId)
    {
        if(NetworkManager.Singleton.LocalClientId == playerId)
        {
            ExpManager.Instance.GainExperience(50);
            DispawnServerRpc(NetworkObjectId);
        }
    }   
}
