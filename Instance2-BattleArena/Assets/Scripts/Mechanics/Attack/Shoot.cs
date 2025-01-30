using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;
using UnityEngine.Pool;

public class Shoot : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private InputActionReference _shoot;
    [HideInInspector] public Transform PlayerTransform;
    private BulletSpawner _bulletSpawner;
    public Bullet BulletPrefab;

    private void Awake()
    {
        Assert.IsNotNull(BulletPrefab, "_bulletPrefab is missing");
        Assert.IsNotNull(_shoot, "_inputAction is missing");

        _bulletSpawner = GetComponent<BulletSpawner>();

        PlayerTransform = transform;
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
        _bulletSpawner.BulletSpawnerPool.Get();
    }       
}
