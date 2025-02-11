using Managers;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace UI
{
    public class UiClassSelectorManager : NetworkBehaviour
    {
        [SerializeField] private CanvasGroup _classSelectorCanvasGroup;
        [SerializeField] private Button _meleeBtn, _archerBtn, _gunnerBtn;
        [SerializeField] private PlayerClassManager _playerClassManager;
        
        

        private void Awake()
        {
            Assert.IsNotNull(_playerClassManager, "_playerClassManager is missing");
            Assert.IsNotNull(_classSelectorCanvasGroup, "_classSelectorCanvasGroup is missing");
            Assert.IsNotNull(_meleeBtn, "_meleeBtn is missing");
            Assert.IsNotNull(_archerBtn, "_archerBtn is missing");
            Assert.IsNotNull(_gunnerBtn, "_gunnerBtn is missing");
            ToggleClassSelectorCanvas(false);
        }

        private void ToggleClassSelectorCanvas(bool state, bool affectTransparency = false)
        {
            if (affectTransparency)
            {
                _classSelectorCanvasGroup.alpha = (state) ? 1: 0;
            }
            _classSelectorCanvasGroup.interactable = state;
            _classSelectorCanvasGroup.blocksRaycasts = state;
        }

        private void Start()
        {
            _meleeBtn.onClick.AddListener(() => SelectClass(0));
            _archerBtn.onClick.AddListener(() => SelectClass(1));
            _gunnerBtn.onClick.AddListener(() => SelectClass(2));
            NetworkManager.Singleton.OnClientConnectedCallback += HandleClientConnected;
        }

        private void HandleClientConnected(ulong id)
        {
            ToggleClassSelectorCanvas(true, true);
        }

        private void SelectClass(int classIndex)
        {
            _playerClassManager.SelectClass(classIndex);
        }
    }
}
