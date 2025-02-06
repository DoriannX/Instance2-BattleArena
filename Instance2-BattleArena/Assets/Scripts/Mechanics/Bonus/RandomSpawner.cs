using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RandomSpawner : NetworkBehaviour
{
    [System.Serializable]
    public class SpawnItem
    {
        public GameObject Prefab;
        public int Quantity;
    }

    public Tilemap Tilemap;
    public Tilemap BlockedTilemap;
    public List<SpawnItem> Objects;

    private List<Vector3> _validPositions = new List<Vector3>();

    private void Awake()
    {
        Debug.Log("RandomSpawner initialized");
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        if (IsServer)
        {
            Debug.Log("Server started, preparing to spawn objects...");
            SpawnObjects();
        }
    }

    public void SpawnObjects()
    {
        GetValidTilePositions();
        Debug.Log($"Found {_validPositions.Count} valid positions for spawning.");

        foreach (SpawnItem item in Objects)
        {
            for (int i = 0; i < item.Quantity && _validPositions.Count > 0; i++)
            {
                int randomIndex = Random.Range(0, _validPositions.Count);
                Vector3 spawnPosition = _validPositions[randomIndex];

                GameObject spawnedObject = Instantiate(item.Prefab, spawnPosition, Quaternion.identity);

                if (spawnedObject.TryGetComponent(out NetworkObject networkObject))
                {
                    networkObject.Spawn();  
                    Debug.Log($"Spawned {item.Prefab.name} at {spawnPosition}");
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

        foreach (Vector3Int pos in bounds.allPositionsWithin)
        {
            if (Tilemap.HasTile(pos) && (BlockedTilemap == null || !BlockedTilemap.HasTile(pos)))
            {
                Vector3 worldPos = Tilemap.GetCellCenterWorld(pos);
                _validPositions.Add(worldPos);
            }
        }
    }
}
