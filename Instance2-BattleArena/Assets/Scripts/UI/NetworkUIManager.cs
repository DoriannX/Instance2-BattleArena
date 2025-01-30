using System.Collections;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
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
        [SerializeField] private TMP_InputField _roomIpAddressInput;
        private NetworkManager _networkManager;
        private UnityTransport _unityTransport;

        private void Awake()
        {
            _networkManager = NetworkManager.Singleton;
            Assert.IsNotNull(_clientBtn, "Client button is not assigned");
            Assert.IsNotNull(_roomIpAddressInput, "Room ip address input is not assigned");
            _unityTransport = _networkManager.GetComponent<UnityTransport>();
        }

        private void ToggleBtns(bool state)
        {
            _clientBtn.gameObject.SetActive(state);
            _roomIpAddressInput.gameObject.SetActive(state);
        }

        private void Start()
        {
            _clientBtn.onClick.AddListener(OnClientBtnClicked);
            _networkManager.OnClientConnectedCallback += OnClientConnected;

        }

        private void OnClientConnected(ulong obj)
        {
            ToggleBtns(false);
            Debug.Log("connected to server");
        }

        private string OnRoomAddressEntered()
        {
            return _unityTransport.ConnectionData.Address = _roomIpAddressInput.text;
        }

        private void OnClientBtnClicked()
        {
            if (!IsValidIpAddress(OnRoomAddressEntered()))
            {
                Debug.LogError("invalid ip address");
                return;
            }

            StartCoroutine(StartClientDelay());
        }

        private IEnumerator StartClientDelay()
        {
            yield return new WaitForEndOfFrame();
            _networkManager.StartClient();
        }

        private bool IsValidIpAddress(string ipAddress)
        {
            if (string.IsNullOrWhiteSpace(ipAddress))
            {
                return false;
            }

            string[] splitValues = ipAddress.Split('.');
            if (splitValues.Length != 4)
            {
                return false;
            }

            foreach (string item in splitValues)
            {
                if (!int.TryParse(item, out int tempValue) || tempValue < 0 || tempValue > 255)
                {
                    return false;
                }
            }

            return true;
        }
    }
}