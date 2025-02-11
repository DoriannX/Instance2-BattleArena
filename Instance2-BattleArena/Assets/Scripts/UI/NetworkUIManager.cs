using Managers;
using Mechanics.Bonus;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using TMPro;

namespace UI
{
    [RequireComponent(typeof(NetworkManager))]
    public class NetworkUIManager : MonoBehaviour
    {
        [SerializeField] private Button _clientBtn;
        [SerializeField] private TMP_InputField _roomCodeInput;
        private NetworkManager _networkManager;
        private RelayManager _relayManager;
        [SerializeField] private RandomSpawner _randomSpawner;

        private void Awake()
        {
            _networkManager = GetComponent<NetworkManager>();
            Assert.IsNotNull(_clientBtn, "Client button is not assigned");
            Assert.IsNotNull(_roomCodeInput, "Room ip address input is not assigned");
            _relayManager = _networkManager.GetComponent<RelayManager>();
        }

        private void ToggleBtns(bool state)
        {
            _clientBtn.gameObject.SetActive(state);
            _roomCodeInput.gameObject.SetActive(state);
        }

        private void Start()
        {
            _clientBtn.onClick.AddListener(OnClientBtnClicked);
            //_networkManager.OnServerStarted += OnServerStarted;
            _networkManager.OnClientConnectedCallback += OnClientConnected;
        }
        private void OnClientConnected(ulong obj)
        {
            ToggleBtns(false);
            Debug.Log("connected to server");
        }

        private void OnClientBtnClicked()
        {
            if(_roomCodeInput.text == "")
            {
                Debug.Log("Please enter a room code");
                return;
            }
            _relayManager.JoinRelay(_roomCodeInput.text);
        }
    }
}