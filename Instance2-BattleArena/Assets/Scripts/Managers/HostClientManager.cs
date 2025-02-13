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
            NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnect;
        }

        private void OnClientDisconnect(ulong obj)
        {
            StartCoroutine(TryConnect());
        }

        private IEnumerator TryConnect()
        {
            if (NetworkManager.Singleton.IsServer)
            {
                yield break;
            }
            
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
