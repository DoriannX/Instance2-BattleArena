using System.Collections;
using Unity.Netcode;
using UnityEngine;

namespace Managers
{
    public class HostClientManager : MonoBehaviour
    {
        [SerializeField] private bool _startServer;
        private void Start()
        {
            StartCoroutine(TryConnect());
        }
        
        private IEnumerator TryConnect()
        {
            while (true)
            {
                if (_startServer || Application.platform == RuntimePlatform.WindowsServer || Application.platform == RuntimePlatform.LinuxServer)
                {
                    if (NetworkManager.Singleton.StartServer())
                    {
                        yield break;
                    }
                }
                else
                {
                    if (NetworkManager.Singleton.StartClient())
                    {
                        yield break;
                    }
                }
                yield return new WaitForSeconds(1); // Wait for 1 second before retrying
            }
        }
    }
}
