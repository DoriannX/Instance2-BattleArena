using Unity.Netcode;
using UnityEngine;

namespace Managers
{
    public class HostClientManager : MonoBehaviour
    {
        [SerializeField] private bool _startServer;
        private void Start()
        {
            if (_startServer || Application.platform == RuntimePlatform.WindowsServer || Application.platform == RuntimePlatform.LinuxServer)
            {
                NetworkManager.Singleton.StartServer();
            }
            else
            {
                NetworkManager.Singleton.StartClient();
            }
        }
    }
}
