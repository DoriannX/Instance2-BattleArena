using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Assertions;

public class OnHoverButtons : MonoBehaviour,IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private RectTransform _statPanel;

    private void Awake()
    {
        Assert.IsNotNull(_statPanel, "_statPanel is null");
    }

    public void OnPointerEnter(PointerEventData data)
    {
        _statPanel.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData data)
    {
        _statPanel.gameObject.SetActive(false);
    }
}
