using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

public class Shoot : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Bullet _bulletPreFab;
    [SerializeField] private InputActionReference _shoot;
    private Transform _playerTransform;

    private void Awake()
    {
        Assert.IsNotNull(_bulletPreFab, "_bulletPreFab is missing");
        Assert.IsNotNull(_shoot, "_inputAction is missing");

        _playerTransform = transform;
    }

    private void OnEnable()
    {
        _shoot.action.started += Fire;
    }

    private void OnDisable()
    {
        _shoot.action.started -= Fire;
    }

    private void Fire(InputAction.CallbackContext context)
    {      
       Bullet bullet = Instantiate(_bulletPreFab, _playerTransform.position + _playerTransform.up, _playerTransform.rotation);        
    }    
}
