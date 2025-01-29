using System;
using Managers;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    [RequireComponent(typeof(NetworkManager))]
    public class NetworkUIManager : MonoBehaviour
    {
        [SerializeField] private Button _hostBtn, _clientBtn;
        private RelayManager _relayManager;

        private void Awake()
        {
            _relayManager = GetComponent<RelayManager>();
        }

        private void ToggleBtns(bool state)
        {
            _hostBtn.gameObject.SetActive(state);
            _clientBtn.gameObject.SetActive(state);
        }

        private void Start()
        {
            _hostBtn.onClick.AddListener(OnHostBtnClicked);
            _clientBtn.onClick.AddListener(OnClientBtnClicked);
        }

        private async void OnClientBtnClicked()
        {
            await _relayManager.JoinRandomRelay();
            ToggleBtns(false);
        }

        private async void OnHostBtnClicked()
        {
            await _relayManager.CreateRelay();
            ToggleBtns(false);
        }
    }
}
