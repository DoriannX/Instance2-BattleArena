using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;

namespace Managers
{
    public class RelayManager : MonoBehaviour
    {

        private async void Start()
        {
            await UnityServices.InitializeAsync();

            AuthenticationService.Instance.SignedIn += OnPlayerSignedIn;
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            if (Application.platform == RuntimePlatform.WindowsServer || Application.platform == RuntimePlatform.LinuxServer)
            {
                await CreateRelay();
            }
        }
        private void OnPlayerSignedIn()
        {
            Debug.Log("signed in" + AuthenticationService.Instance.PlayerId);
        }

        [ContextMenu("Create Relay")]
        private async Task<string> CreateRelay()
        {
            try
            {
                Allocation allocation = await RelayService.Instance.CreateAllocationAsync(20);
                string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
                RelayServerData relayServerData = new(allocation, "wss");
                NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
                NetworkManager.Singleton.StartServer();
                Debug.Log("server started with join code: " + joinCode);
                return joinCode;
            }
            catch (RelayServiceException e)
            {
                Debug.LogError("error in creating relay allocation: " + e.Message);
                return null;
            }
        }

        
        [ContextMenu("Auto Join Relay")]
        private async void AutoCreateAndJoin()
        {
            string joinCode = await CreateRelay();
            JoinRelay(joinCode);
        }

        public async void JoinRelay(string joinCode)
        {
            try
            {
                Debug.Log("joining relay with join code " + joinCode);
                JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);
                
                RelayServerData relayServerData = new(joinAllocation, "wss");
                
                NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
                NetworkManager.Singleton.StartClient();
                Debug.Log("client " + AuthenticationService.Instance.PlayerId + " joined relay");
            }
            catch (RelayServiceException e)
            {
                Debug.LogError("error in joining relay allocation: " + e.Message);
            }
        }
    }
}