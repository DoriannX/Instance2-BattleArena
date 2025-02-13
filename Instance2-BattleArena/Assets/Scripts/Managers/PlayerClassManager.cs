using NUnit.Framework;
using System.Collections.Generic;
using Mechanics.PlayerStats;
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

        [SerializeField] private Transform _playerSpawn;
        private GameObject _playerInstance;
        public int SelectedClassIndex { get; private set; } = -1;
         public CharacterClass SelectedClass;

        void Start()
        {
            _playerStats = PlayerPrefabShield.GetComponent<PlayerStats>();
        }


        public void SelectClass(int classIndex)
        {
            SelectedClassIndex = classIndex;
            AskSpawnSelfServerRpc(
                NetworkManager.Singleton.LocalClientId, classIndex);
            
            PanelSelectSkin.SetActive(true);
        }

        [ContextMenu("Respawn")]
        public void RespawnSelf()
        {
            RespawnPlayer(NetworkManager.Singleton.LocalClientId, SelectedClassIndex);
        }

        private void RespawnPlayer(ulong clientId, int selectedClassIndex)
        {
            selectedClassIndex = Mathf.Clamp(selectedClassIndex, 0, CharacterClasses.Length - 1);
            AskSpawnSelfServerRpc(clientId, selectedClassIndex);
        }

        [ServerRpc(RequireOwnership = false)]
        public void AskSpawnSelfServerRpc(ulong id, int classIndex)
        {
            if (classIndex >= 0 && classIndex < CharacterClasses.Length)
            {
                SelectedClass = CharacterClasses[classIndex];
                PlayerPrefabShield.GetComponent<SpriteRenderer>().sprite = SelectedClass.BaseSprite;
                SelectedClass.ApplyClassStats(_playerStats);

                GameObject playerPrefab = classIndex switch
                {
                    0 => PlayerPrefabShield,
                    1 => PlayerPrefabSoldier,
                    2 => PlayerPrefabCarrier,
                    _ => PlayerPrefabShield
                };
                
                GameObject playerInstance =
                    Instantiate(playerPrefab, _playerSpawn.position, Quaternion.identity);
                playerInstance.GetComponent<NetworkObject>().SpawnAsPlayerObject(id);
                _playerInstance = playerInstance;

                if (ExpManager != null)
                {
                    ExpManager.Initialize(SelectedClass, _playerInstance);
                }
            }

            Debug.Log("player is spawned" + id);
        }
    }
}