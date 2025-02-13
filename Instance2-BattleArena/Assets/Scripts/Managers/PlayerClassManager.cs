using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Serialization;

namespace Managers
{
    public class PlayerClassManager : NetworkBehaviour
    {
        [System.Serializable]
        public class CharacterClass
        {
            public string ClassName;
            public Sprite BaseSprite;
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
        public GameObject PlayerPrefabShield;
        public GameObject PlayerPrefabSoldier;
        public GameObject PlayerPrefabCarrier;
        public GameObject PanelSelectSkin;
        public ExpManager ExpManager;
        private NetworkManager _networkManager;

        [SerializeField] private Transform _playerSpawn;
        private GameObject _playerInstance = null;

        private int _selectedClassIndex = -1;
        public int SelectedClassIndex = -1;
        public CharacterClass SelectedClass;

        void Start()
        {
            _networkManager = NetworkManager.Singleton;
            _playerStats = PlayerPrefabShield.GetComponent<PlayerStats>();
        }


        public void SelectClass(int classIndex)
        {
            _selectedClassIndex = classIndex;
            AskSpawnSelfServerRpc(
                _networkManager.LocalClientId, classIndex);
            
            PanelSelectSkin.SetActive(true);
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
        private void AskSpawnSelfServerRpc(ulong id, int classIndex)
        {
            if (classIndex >= 0 && classIndex < CharacterClasses.Length)
            {
                CharacterClass selectedClass = CharacterClasses[classIndex];
                PlayerPrefabShield.GetComponent<SpriteRenderer>().sprite = selectedClass.BaseSprite;
                selectedClass.ApplyClassStats(_playerStats);


                if (classIndex == 0)
                {
                    GameObject playerInstance =
                        Instantiate(PlayerPrefabAlternate, _playerSpawn.position, Quaternion.identity);
                    playerInstance.GetComponent<NetworkObject>().SpawnAsPlayerObject(id);
                    _playerInstance = playerInstance;
                }
                else
                {
                    GameObject playerInstance = Instantiate(PlayerPrefab, _playerSpawn.position, Quaternion.identity);
                    playerInstance.GetComponent<NetworkObject>().SpawnAsPlayerObject(id);
                    _playerInstance = playerInstance;
                }

                if (ExpManager != null)
                {
                    ExpManager.Initialize(selectedClass, _playerInstance);
                }
            }

            Debug.Log("player is spawned" + id);
        }
    }
}