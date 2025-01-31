using Unity.Netcode;
using UnityEngine;

namespace Mechanics.Player
{
    public class PlayerNetwork : NetworkBehaviour
    {
        private readonly NetworkVariable<Vector3> _position = new(writePerm: NetworkVariableWritePermission.Owner);
        private Vector3 _vel;

        private void Update()
        {
            if (!IsOwner)
            {
                transform.position = Vector3.SmoothDamp(transform.position, _position.Value, ref _vel, 0.05f);
            }
            else
            {
                _position.Value = transform.position;
            }
        }
    }
}