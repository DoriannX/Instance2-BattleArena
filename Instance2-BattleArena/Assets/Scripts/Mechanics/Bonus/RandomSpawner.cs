using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RandomSpawner : MonoBehaviour
{
    public static RandomSpawner Instance { get; private set; }

    [System.Serializable]
    public class SpawnItem
    {
        public GameObject Prefab;
        public int Quantity;
    }

    public Tilemap Tilemap;
    public Tilemap BlockedTilemap;
    public List<SpawnItem> Objects;

    [SerializeField] private Transform _parentSpawn;
    [SerializeField] private Transform _parentSpawnCloneBonus;

    private List<Vector3> _validPositions = new List<Vector3>();

    private float respawnDelay = 3f;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        GetValidTilePositions();
        SpawnObjects();
    }

    void GetValidTilePositions()
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

    void SpawnObjects()
    {
        foreach (SpawnItem item in Objects)
        {
            for (int i = 0; i < item.Quantity && _validPositions.Count > 0; i++)
            {
                int randomIndex = Random.Range(0, _validPositions.Count);
                GameObject spawnedObject = Instantiate(item.Prefab, _validPositions[randomIndex], Quaternion.identity);
                spawnedObject.transform.SetParent(_parentSpawn);

                _validPositions.RemoveAt(randomIndex);
            }
        }
    }

    public void RespawnBonus(GameObject bonusInstance)
    {
        if (bonusInstance != null)
        {
            GameObject bonusPrefab = Instantiate(bonusInstance);
            bonusPrefab.transform.SetParent(_parentSpawnCloneBonus);
            bonusPrefab.SetActive(false);

            StartCoroutine(RespawnBonusWithDelay(bonusPrefab));
        }
    }

    private IEnumerator RespawnBonusWithDelay(GameObject bonusPrefab)
    {
        yield return new WaitForSeconds(2f); 

        if (bonusPrefab != null)
        {
            int randomIndex = Random.Range(0, _validPositions.Count);
            Vector3 spawnPosition = _validPositions[randomIndex];
            bonusPrefab.transform.position = spawnPosition;
            bonusPrefab.SetActive(true);
            bonusPrefab.transform.SetParent(_parentSpawn);

            _validPositions.RemoveAt(randomIndex);
        }
    }
}
