using TMPro;
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

    [Header("Animator")]
    [SerializeField] private Animator _animator;
    private int _switchAttack = 0;

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
        _animator.SetBool("IsAttacking", true);
    }

    private void ResetAnimFire()
    {
        _switchAttack++;
        _animator.SetBool("IsAttacking", false);
        if (_switchAttack >= 2)
        {
            _switchAttack = 0;
        }
        _animator.SetInteger("SwitchAttack", _switchAttack);
    }
}
