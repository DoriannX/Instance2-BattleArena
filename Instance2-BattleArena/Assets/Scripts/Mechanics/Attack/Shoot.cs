using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;

public class Shoot : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Bullet _bulletPreFab;
    [SerializeField] private InputActionReference _inputAction;

    private void Awake()
    {
        Assert.IsNotNull(_bulletPreFab, "_bulletPreFab is missing");
        Assert.IsNotNull(_inputAction, "_inputAction is missing");
    }

    private void OnEnable()
    {
        _inputAction.action.started += Fire;
    }

    private void OnDisable()
    {
        _inputAction.action.started -= Fire;
    }

    private void Fire(InputAction.CallbackContext context)
    {      
       Bullet bullet = Instantiate(_bulletPreFab, transform.position + transform.up, transform.rotation);        
    }    
}
