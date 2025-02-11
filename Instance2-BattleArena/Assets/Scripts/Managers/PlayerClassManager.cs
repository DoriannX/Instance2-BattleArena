using Unity.Netcode;
using UnityEngine;

namespace Managers
{
    public class PlayerClassManager : NetworkBehaviour
    {
        [System.Serializable]
        public class CharacterClass
        {
            public string ClassName;
            public Sprite BaseSprite;
            public Sprite Level10Sprite;
            public Sprite Level20Sprite;
            public int BaseAttack;
            public int BaseHeal;
            public float BaseAttackSpeed;

            public void ApplyClassStats(PlayerStats stats)
            {
                stats.SetStats(BaseAttack, BaseHeal, BaseAttackSpeed);
            }
        }

        public CharacterClass[] CharacterClasses;
        private PlayerStats _playerStats;
        public GameObject PlayerPrefab;
        public GameObject PanelSelectClass;
        public ExpManager expManager;
        private NetworkManager _networkManager;

        [SerializeField] private Transform _playerSpawn;
        [SerializeField] private GameObject PlayerPrefabAlternate;
        private GameObject _playerInstance = null;

        private int _selectedClassIndex = -1;

        void Start()
        {
            _playerStats = PlayerPrefab.GetComponent<PlayerStats>();
            _networkManager = NetworkManager.Singleton;
        }


        public void SelectClass(int classIndex)
        {
            _selectedClassIndex = classIndex;
            PanelSelectClass.SetActive(false);
            AskSpawnSelfServerRpc(
                _networkManager.LocalClientId, classIndex);
        }

        [ContextMenu("Respawn")]
        public void RespawnSelf()
        {
            RespawnPlayer(_networkManager.LocalClientId, _selectedClassIndex);
        }

        public void RespawnPlayer(ulong clientId, int selectedClassIndex)
        {
            selectedClassIndex = Mathf.Clamp(selectedClassIndex, 0, CharacterClasses.Length - 1);
            AskSpawnSelfServerRpc(clientId, selectedClassIndex);
        }

        [ServerRpc(RequireOwnership = false)]
        private void AskSpawnSelfServerRpc(ulong id, int classIndex) {

            if (classIndex >= 0 && classIndex < CharacterClasses.Length)
            {
                CharacterClass selectedClass = CharacterClasses[classIndex];
                PlayerPrefab.GetComponent<SpriteRenderer>().sprite = selectedClass.BaseSprite;
                selectedClass.ApplyClassStats(_playerStats);
                PanelSelectClass.SetActive(false);


                if (classIndex == 0)
                {
                    GameObject playerInstance = Instantiate(PlayerPrefabAlternate, _playerSpawn.position, Quaternion.identity);
                    playerInstance.GetComponent<NetworkObject>().SpawnAsPlayerObject(id);
                    _playerInstance = playerInstance;
                }
                else
                {
                    GameObject playerInstance = Instantiate(PlayerPrefab, _playerSpawn.position, Quaternion.identity);
                    playerInstance.GetComponent<NetworkObject>().SpawnAsPlayerObject(id);
                    _playerInstance = playerInstance;
                }
                if (expManager != null)
                {
                    expManager.Initialize(selectedClass, _playerInstance);
                }
            }
            Debug.Log("player is spawned" + id);
        }
    }
}
