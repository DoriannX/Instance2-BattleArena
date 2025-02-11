using UnityEngine;
using Unity.Netcode;

public class Bonus : NetworkBehaviour
{
    private BonusEffects _bonusEffect;
    private bool _isCollected = false;

    private void Start()
    {
        _bonusEffect = GetComponent<BonusEffects>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!_isCollected && other.CompareTag("Player") && IsServer)  // Vérifie si le serveur gère l'événement
        {
            _isCollected = true;

            _bonusEffect?.ApplyEffect(other.gameObject);  // Applique l'effet du bonus

            //if (RandomSpawner.Instance != null)
            //{
            //    RandomSpawner.Instance.RespawnBonusServerRpc(NetworkObjectId);  // Appelle le ServerRpc pour respawn
            //}

            NetworkObject.Despawn();  // Supprime le bonus pour tous les clients
        }
    }
}
