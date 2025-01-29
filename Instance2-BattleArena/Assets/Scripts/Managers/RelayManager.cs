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
    public class RelayManager: MonoBehaviour
    {
        private async void Start()
        {
            await UnityServices.InitializeAsync();
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }

        public async Task JoinRandomRelay()
        {
            await JoinRelay(RoomManager.GetRandomCode());
        }
        
        public async Task JoinRelay(string joinCode)
        {
            try
            {
                JoinAllocation allocation = await RelayService.Instance.JoinAllocationAsync(joinCode);

                UnityTransport relayTransport = NetworkManager.Singleton.GetComponent<UnityTransport>();
                relayTransport.SetRelayServerData(new RelayServerData(allocation, "dtls"));

                NetworkManager.Singleton.StartClient();
            }
            catch (RelayServiceException e)
            {
                Debug.LogError(e);
            }
        }
        
        public async Task<string> CreateRelay()
        {
            try
            {
                Allocation allocation = await RelayService.Instance.CreateAllocationAsync(4);
                string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

                UnityTransport relayTransport = NetworkManager.Singleton.GetComponent<UnityTransport>();
                relayTransport.SetRelayServerData(new RelayServerData(allocation, "dtls"));

                NetworkManager.Singleton.StartHost();
                RoomManager.AddRoomCode(joinCode);

                return joinCode;
            }
            catch (RelayServiceException e)
            {
                Debug.LogError(e);
                return null;
            }
        }
    }
}