using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;
using UnityEngine.Pool;

public class Bullet : MonoBehaviour
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

    private void OnEnable()
    {
        SetVelocity();
    }

    private void Update()
    {        
        LimitMapDeactivate(); // test system, not permanent
    }

    private void SetVelocity()
    {
        _bulletRigidbody.linearVelocity = _bulletTransform.up * _speedBullet;     
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Touched");
        //BulletPool.Release(this);
    }

    private void LimitMapDeactivate() // test system, not permanent
    {
        Vector3 viewportPosition = Camera.main.WorldToViewportPoint(_bulletTransform.position);

        if (viewportPosition.x < 0 || viewportPosition.x > 1 || viewportPosition.y < 0 || viewportPosition.y > 1) BulletPool.Release(this);
    }

    public void SetPool(ObjectPool<Bullet> pool)
    {
        BulletPool = pool;
    }
}
