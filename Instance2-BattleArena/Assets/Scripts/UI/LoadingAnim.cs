using System;
using UnityEngine;

namespace UI
{
    public class LoadingAnim : MonoBehaviour
    {
        private RectTransform _loadingAnim;
        [SerializeField] private float _rotationSpeed = 180f;

        private void Awake()
        {
            _loadingAnim = GetComponent<RectTransform>();
        }

        private void Update()
        {
            _loadingAnim.Rotate(0, 0, -_rotationSpeed * Time.deltaTime);
        }
    }
}
