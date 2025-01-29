using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

public class HitMele : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private InputActionReference _hitMele;

    private void Awake()
    {        
        Assert.IsNotNull(_hitMele, "_inputAction is missing");
    }

    private void OnEnable()
    {
        _hitMele.action.started += Hit;
    }

    private void OnDisable()
    {
        _hitMele.action.started -= Hit;
    }

    private void Hit(InputAction.CallbackContext context)
    {
        Debug.Log("Hit");
    }
}
