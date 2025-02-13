using Unity.Netcode;
using UnityEngine;
using UnityEngine.Pool;

public class Bullet : NetworkBehaviour
{
    [Header("Settings")]
    [SerializeField] private float _speedBullet = 10f;
    public float Speed => _speedBullet;  // Getter pour la vitesse

    private Rigidbody2D _bulletRigidbody;
    private Transform _bulletTransform;
    private ObjectPool<Bullet> _bulletPool;

    private void Awake()
    {
        _bulletRigidbody = GetComponent<Rigidbody2D>();
        _bulletTransform = transform;
    }

    public void SetPool(ObjectPool<Bullet> pool)
    {
        _bulletPool = pool;
    }

    private void OnEnable()
    {
        if (IsServer)
        {
            ApplyInitialVelocity();  // Applique la vélocité uniquement côté serveur
        }
    }

    private void ApplyInitialVelocity()
    {
        _bulletRigidbody.linearVelocity = _bulletTransform.up * _speedBullet;  // Applique une vélocité initiale
    }

    private void Update()
    {
        if (IsOwner)
        {
            Debug.Log(_bulletRigidbody.linearVelocity);
            CheckOutOfBounds();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Wall") || collision.CompareTag("Enemy"))
        {
            ReturnToPool();
        }
    }

    private void CheckOutOfBounds()
    {
        Vector3 viewportPosition = Camera.main.WorldToViewportPoint(_bulletTransform.position);
        if (viewportPosition.x < 0 || viewportPosition.x > 1 || viewportPosition.y < 0 || viewportPosition.y > 1)
        {
            ReturnToPool();
        }
    }

    private void ReturnToPool()
    {
        _bulletRigidbody.linearVelocity= Vector2.zero;
        _bulletPool.Release(this);  // Retourne la balle au pool
    }

    public void ResetBullet()
    {
        _bulletRigidbody.linearVelocity = Vector2.zero;
        _bulletTransform.position = Vector3.zero;
        _bulletTransform.rotation = Quaternion.identity;
    }
}
