using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;

public class HitMele : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private InputActionReference _inputAction;

    private void Awake()
    {        
        Assert.IsNotNull(_inputAction, "_inputAction is missing");
    }

    private void OnEnable()
    {
        _inputAction.action.started += Hit;
    }

    private void OnDisable()
    {
        _inputAction.action.started -= Hit;
    }

    private void Hit(InputAction.CallbackContext context)
    {
        Debug.Log("Hit");
    }
}
