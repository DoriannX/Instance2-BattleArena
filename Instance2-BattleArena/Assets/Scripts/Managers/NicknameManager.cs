using System;
using TMPro;
using UI;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace Managers
{
    [RequireComponent(typeof(NetworkObject))]
    public class NicknameManager : NetworkBehaviour
    {
        public static NicknameManager Instance;
        [SerializeField] private TMP_InputField _nicknameInputField;
        [SerializeField] private Button _validateButton;
        [SerializeField] private UiClassSelectorManager _uiClassSelectorManager;
        [SerializeField] private CanvasGroup _nicknameCanvasGroup;
        private string _nickname;

        public string Nickname => _nickname;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

            AssertSerializedFields();
        }

        private void AssertSerializedFields()
        {
            Assert.IsNotNull(_nicknameInputField,
                $"{nameof(_nicknameInputField)} is not assigned in {nameof(NicknameManager)}");
            Assert.IsNotNull(_validateButton,
                $"{nameof(_validateButton)} is not assigned in {nameof(NicknameManager)}");
            Assert.IsNotNull(_uiClassSelectorManager, "_uiClassSelectorManager is not assigned in NicknameManager");
            Assert.IsNotNull(_nicknameCanvasGroup,
                $"{nameof(_nicknameCanvasGroup)} is not assigned in {nameof(NicknameDisplay)}");
        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            _validateButton.onClick.AddListener(ValidateNickname);
            _nicknameCanvasGroup.interactable = true;
            _nicknameCanvasGroup.blocksRaycasts = true;
            _nicknameCanvasGroup.alpha = 1;
        }

        private void Start()
        {
            
            _nicknameCanvasGroup.interactable = false;
            _nicknameCanvasGroup.blocksRaycasts = false;
            _nicknameCanvasGroup.alpha = 0;
        }
        private void ValidateNickname()
        {
            _nickname = _nicknameInputField.text;
            _nicknameCanvasGroup.interactable = false;
            _nicknameCanvasGroup.blocksRaycasts = false;
            _nicknameCanvasGroup.alpha = 0;
            _uiClassSelectorManager.ToggleClassSelectorCanvas(true, true);
        }
    }
}