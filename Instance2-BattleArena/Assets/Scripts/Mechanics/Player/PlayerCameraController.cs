using Unity.Cinemachine;
using Unity.Netcode;
using UnityEngine;

namespace Mechanics.Player
{
    public class PlayerCameraController : NetworkBehaviour
    {
        private CinemachineCamera _controlledCamera;
        
        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            if (!IsOwner || IsServer)
            {
                enabled = false;
                return;
            }
            if (Camera.main == null)
            {
                return;
            }
            _controlledCamera = Camera.main.GetComponent<CinemachineCamera>();
            if (_controlledCamera == null)
            {
                return;
            }
            _controlledCamera.Follow = transform;
        }
    }
}
