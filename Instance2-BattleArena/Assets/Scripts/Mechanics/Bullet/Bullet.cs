using Unity.Netcode;
using UnityEngine;
using UnityEngine.Pool;

public class Bullet : NetworkBehaviour
{
    [Header("References")]
    private Rigidbody2D _bulletRigidbody;
    private Transform _bulletTransform;
    private ObjectPool<Bullet> BulletPool;

    [Header("Settings")]
    [SerializeField] private float _speedBullet;

    private void Awake()
    {
        _bulletRigidbody = GetComponent<Rigidbody2D>();
        _bulletTransform = transform;
    }

    private void Update()
    {
        LimitMapDeactivate();
    }

    private void OnEnable()
    {
        SetVelocity();
    }

    private void SetVelocity()
    {
        _bulletRigidbody.linearVelocity = _bulletTransform.up * _speedBullet; // Assurez-vous d'utiliser "velocity" et non "linearVelocity"
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerStats playerStats = collision.gameObject.GetComponent<PlayerStats>();
            if (playerStats != null)
            {
                playerStats.TakeDamage(playerStats.Attack);
                if (playerStats.CurrentHealth <= 0)
                {
                    ExpManager.Instance.GainExperience(50);
                }
            }
        }
        BulletPool.Release(this);
    }

    private void LimitMapDeactivate() 
    {
        Vector3 viewportPosition = Camera.main.WorldToViewportPoint(_bulletTransform.position);
        if (viewportPosition.x < 0 || viewportPosition.x > 1 || viewportPosition.y < 0 || viewportPosition.y > 1)
        {
            BulletPool.Release(this);
        }
    }
    public void SetPool(ObjectPool<Bullet> pool)
    {
        BulletPool = pool;
    }
}
