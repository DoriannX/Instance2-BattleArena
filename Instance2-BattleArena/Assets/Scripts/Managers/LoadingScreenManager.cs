using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace Managers
{
    [RequireComponent(typeof(NetworkObject))]
    public class LoadingScreenManager : NetworkBehaviour
    {
        [SerializeField] private Image _loadingScreen;

        private void Awake()
        {
            _loadingScreen.gameObject.SetActive(true);
        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            
            Debug.Log("Loading screen is active");
            _loadingScreen.gameObject.SetActive(false);
            if (!IsServer)
            {
                NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnect;
            }
        }

        private void OnClientDisconnect(ulong obj)
        {
            _loadingScreen.gameObject.SetActive(true);
        }
    }
}
