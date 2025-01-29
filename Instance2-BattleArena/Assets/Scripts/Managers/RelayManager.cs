using Unity.Netcode;
using UnityEngine;

namespace Managers
{
    public class RelayManager : MonoBehaviour
    {
        private void Awake()
        {
            if (Application.platform == RuntimePlatform.WindowsServer || Application.platform == RuntimePlatform.LinuxServer)
            {
                NetworkManager.Singleton.StartServer();
                Debug.Log("started server");
            }
        }
    }
}