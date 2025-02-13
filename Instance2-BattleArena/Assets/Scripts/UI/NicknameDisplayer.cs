using System;
using Managers;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Assertions;

namespace UI
{
    [RequireComponent(typeof(NetworkObject))]
    public class NicknameDisplay : NetworkBehaviour
    {

        [SerializeField] private TMP_Text _nicknameText;

        private void Awake()
        {
            Assert.IsNotNull(_nicknameText,
                $"{nameof(_nicknameText)} is not assigned in {nameof(NicknameDisplay)}");
        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            if (IsOwner)
            {
                AskChangeNicknameServerRpc(NicknameManager.Instance.Nickname);
            }
        }
        
        [ServerRpc(RequireOwnership = false)]
        private void AskChangeNicknameServerRpc(string pseudo)
        {
            _nicknameText.text = pseudo;
            ChangeOnClientRpc(pseudo);
        }

        [ClientRpc]
        private void ChangeOnClientRpc(string pseudo)
        {
            _nicknameText.text = pseudo;
        }
    }
}