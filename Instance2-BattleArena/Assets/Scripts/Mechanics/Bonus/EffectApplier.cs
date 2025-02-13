using Unity.Netcode;
using UnityEngine;

namespace Mechanics.Bonus
{
    public abstract class EffectApplier : NetworkBehaviour, IEffectApplier
    {
        private bool _isCollected;

        [ClientRpc]
        public virtual void ApplyEffectClientRpc(ulong playerId)
        {
            //Has a body because the Rpcs functions cannot be abstract
        }

        [ServerRpc(RequireOwnership = false)]
        public virtual void DispawnServerRpc(ulong id)
        {
            NetworkManager.Singleton.SpawnManager.SpawnedObjects.TryGetValue(id, out NetworkObject obj);
            
            if (obj != null)
            {
                obj.Despawn();
            }
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            Debug.Log("Trigger enter");
            if (!_isCollected && other.CompareTag("Player") && IsServer)  // V�rifie si le serveur g�re l'�v�nement
            {
                Debug.Log("Player enter");
                _isCollected = true;

                ApplyEffectClientRpc(other.gameObject.GetComponent<NetworkObject>().NetworkObjectId);  // Applique l'effet du bonus

                //NetworkObject.Despawn();  // Supprime le bonus pour tous les clients
            }
        }
    }
    
    
}