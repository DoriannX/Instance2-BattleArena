using Unity.Netcode;
using UnityEngine;

namespace Managers
{
    public class RelayManager : MonoBehaviour
    {
        private void Start()
        {
            if (Application.platform == RuntimePlatform.WindowsServer || Application.platform == RuntimePlatform.LinuxServer)
            {
                NetworkManager.Singleton.StartServer();
                Debug.Log("started server");
            }
        }
    }
}