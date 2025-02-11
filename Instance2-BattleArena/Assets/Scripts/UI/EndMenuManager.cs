using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace UI
{
    public class EndMenuManager : MonoBehaviour
    {
        public static EndMenuManager Instance;
        [SerializeField] private Button _restartBtn, _exitBtn;
        [SerializeField] private CanvasGroup _classSelectorCanvasGroup;
        private CanvasGroup _endMenuCanvasGroup;
        private bool _state;
        
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
            Assert.IsNotNull(_restartBtn, "_restartBtn is missing");
            Assert.IsNotNull(_exitBtn, "_exitBtn is missing");
            Assert.IsNotNull(_classSelectorCanvasGroup, "_classSelectorCanvasGroup is missing");
            _restartBtn.onClick.AddListener(OnRestartBtnClicked);
            _exitBtn.onClick.AddListener(OnExitBtnClicked);
            _endMenuCanvasGroup = GetComponent<CanvasGroup>();
            ToggleCanvasGroup(_endMenuCanvasGroup, false);
        }

        public void Toggle()
        {
            ToggleCanvasGroup(_classSelectorCanvasGroup, _state);
            ToggleCanvasGroup(_endMenuCanvasGroup, !_state);
        }

        private void OnExitBtnClicked()
        {
            if(Application.isEditor)
            {
#if UNITY_EDITOR
                EditorApplication.isPlaying = false;
#endif
            }
            else
            {
                Application.Quit();
            }
        }
        
        private void ToggleCanvasGroup(CanvasGroup canvas,  bool state)
        {
            canvas.alpha = state ? 1 : 0;
            canvas.interactable = state;
            canvas.blocksRaycasts = state;
        }

        private void OnRestartBtnClicked()
        {
            ToggleCanvasGroup(_classSelectorCanvasGroup, true);
            ToggleCanvasGroup(_endMenuCanvasGroup, false);
            _state = false;
        }
    }
}
