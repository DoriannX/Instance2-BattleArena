using AudioSystem;
using Mechanics.Bonus;
using Unity.Netcode;
using UnityEngine;

public class Seed : EffectApplier
{
    [SerializeField] private SoundData _soundData;
    public override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
        if (IsServer)
        {
            SoundManager.Instance.CreateSound().WithSoundData(_soundData).Play();
            
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
