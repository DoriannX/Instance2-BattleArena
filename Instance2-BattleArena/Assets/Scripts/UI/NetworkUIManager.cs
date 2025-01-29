using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    [RequireComponent(typeof(NetworkManager))]
    public class NetworkUIManager : MonoBehaviour
    {
        [SerializeField] private Button _hostBtn, _clientBtn;
        private NetworkManager _networkManager;

        private void Awake()
        {
            _networkManager = GetComponent<NetworkManager>();
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

        private void OnClientBtnClicked()
        {
            _networkManager.StartClient();
            ToggleBtns(false);
        }

        private void OnHostBtnClicked()
        {
            _networkManager.StartHost();
            ToggleBtns(false);
        }
    }
}
