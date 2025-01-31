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

    private PlayerStats _playerStats;  
    private float _fireRate; 
    private float _timeSinceLastShot;  

    private void Awake()
    {
        Assert.IsNotNull(BulletPrefab, "_bulletPrefab is missing");
        Assert.IsNotNull(_shoot, "_inputAction is missing");

        _bulletSpawner = GetComponent<BulletSpawner>();
        PlayerTransform = transform;
        _playerStats = GetComponent<PlayerStats>();
        if (_playerStats != null)
        {
            if (_playerStats.AttackSpeed > 0)
            {
                _fireRate = 1f / Mathf.Max(_playerStats.AttackSpeed, 0.1f); 
            }
            else
            {
                _fireRate = 0.1f;  
            }
        }
    }

    private void OnEnable()
    {
        _shoot.action.started += Fire;
    }

    private void OnDisable()
    {
        _shoot.action.started -= Fire;
    }

    private void Update()
    {
        _timeSinceLastShot += Time.deltaTime;
    }

    private void Fire(InputAction.CallbackContext context)
    {
        if (_timeSinceLastShot >= _fireRate)
        {
            _timeSinceLastShot = 0f;
            _bulletSpawner.BulletSpawnerPool.Get();
        }
    }
}
