using Unity.Netcode;
using UnityEngine;
using UnityEngine.Assertions;

namespace Mechanics.Player
{
    public class PlayerSpawner : NetworkBehaviour
    {
        [SerializeField] private Transform _spawnPoint;
        [SerializeField] private Transform _playerPrefab;

        private void Awake()
        {
            Assert.IsNotNull(_spawnPoint, "Spawn point is not assigned");
            Assert.IsNotNull(_playerPrefab, "Player prefab is not assigned");
        }
        
        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            if (IsServer)
            {
                NetworkManager.OnClientConnectedCallback += OnClientConnected;
            }
        }

        private void OnClientConnected(ulong obj)
        {
            if (IsServer)
            {
                Transform spawnedPlayer = Instantiate(_playerPrefab, _spawnPoint.position, Quaternion.identity);
                spawnedPlayer.GetComponent<NetworkObject>().SpawnAsPlayerObject(obj);
                Debug.Log("spawned player");
            }
        }
    }
}