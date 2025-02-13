using System;
using System.Collections.Generic;
using Events;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

namespace Mechanics.Bonus
{
    public class RandomSpawner : NetworkBehaviour
    {
        [Serializable]
        public class SpawnItem
        {
            public GameObject Prefab;
            public int Quantity;
        }

        public Tilemap Tilemap;
        public Tilemap BlockedTilemap;
        public List<SpawnItem> Objects;

        private List<Vector3> _validPositions = new();

        private void Start()
        {
            NetworkManager.Singleton.OnServerStarted += OnServerStarted;
            EventManager.OnObjectUsed += SpawnOnServerRpc;
        }

        [ServerRpc(RequireOwnership = false)]
        private void SpawnOnServerRpc()
        {
            SpawnSingleObject();
        }

        private void OnServerStarted()
        {
            Debug.Log("server started");
            if (IsServer)
            {
                Debug.Log("Objects spawned.");
                SpawnObjects();
            }
        }

        public void SpawnSingleObject()
        {
            GetValidTilePositions();
            Debug.Log($"Found {_validPositions.Count} valid positions for spawning.");
        
            if (_validPositions.Count > 0)
            {
                int randomIndex = Random.Range(0, _validPositions.Count);
                Vector3 spawnPosition = _validPositions[randomIndex];
        
                int randomObjectIndex = Random.Range(0, Objects.Count);
                GameObject prefab = Objects[randomObjectIndex].Prefab;
        
                GameObject spawnedObject = Instantiate(prefab, spawnPosition, Quaternion.identity);
        
                if (spawnedObject.TryGetComponent(out NetworkObject networkObject))
                {
                    networkObject.Spawn();
                    Debug.Log($"Spawned {spawnedObject.name} at {spawnPosition}");
                }
                else
                {
                    Debug.LogError($"{prefab.name} does not have a NetworkObject component!");
                }
        
                _validPositions.RemoveAt(randomIndex);
            }
            else
            {
                Debug.LogWarning("No valid positions available for spawning.");
            }
        }

        [ContextMenu("Spawn Objects")]
        private void SpawnObjects()
        {
            GetValidTilePositions();
            Debug.Log($"Found {_validPositions.Count} valid positions for spawning.");

            foreach (SpawnItem item in Objects)
            {
                for (int i = 0; i < item.Quantity && _validPositions.Count > 0; i++)
                {
                    int randomIndex = Random.Range(0, _validPositions.Count);
                    Vector3 spawnPosition = _validPositions[randomIndex];

                    GameObject spawnedObject = Instantiate(item.Prefab, spawnPosition, item.Prefab.transform.rotation);

                    if (spawnedObject.TryGetComponent(out NetworkObject networkObject))
                    {
                        networkObject.Spawn();
                        Debug.Log($"Spawned {spawnedObject.name} at {spawnPosition}");
                    }
                    else
                    {
                        Debug.LogError($"{item.Prefab.name} does not have a NetworkObject component!");
                    }

                    _validPositions.RemoveAt(randomIndex);
                }
            }
        }

        private void GetValidTilePositions()
        {
            _validPositions.Clear();
            BoundsInt bounds = Tilemap.cellBounds;
            Vector3Int min = bounds.min;
            Vector3Int max = bounds.max;

            for (int x = min.x; x < max.x; x++)
            {
                for (int y = min.y; y < max.y; y++)
                {
                    Vector3Int cellPosition = new Vector3Int(x, y, 0);
                    Vector3 worldPosition = Tilemap.CellToWorld(cellPosition) + Tilemap.tileAnchor;

                    if (Tilemap.HasTile(cellPosition) && !BlockedTilemap.HasTile(cellPosition))
                    {
                        Collider2D[] colliders = Physics2D.OverlapCircleAll(worldPosition, 0.5f);
                        if (colliders.Length == 0)
                        {
                            foreach (Collider2D item in colliders)
                            {
                                if (item.TryGetComponent(out EffectApplier effectApplier))
                                {
                                    break;
                                }
                            }

                            _validPositions.Add(worldPosition);
                        }
                    }
                }
            }
        }
    }
}