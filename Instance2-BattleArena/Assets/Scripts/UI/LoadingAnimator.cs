using System;
using DG.Tweening;
using UnityEngine;

namespace UI
{
    public class LoadingAnimator : MonoBehaviour
    {
        [SerializeField] private float _speed;
        private RectTransform _transform;

        private void Awake()
        {
            _transform = GetComponent<RectTransform>();
        }

        private void OnEnable()
        {
            Debug.Log("Loading animator enabled");
            _transform.DORotate(new Vector3(0, 0, -360), 1/_speed, RotateMode.FastBeyond360)
                .SetLoops(-1, LoopType.Restart)
                .SetEase(Ease.Linear);
        }
    }
}