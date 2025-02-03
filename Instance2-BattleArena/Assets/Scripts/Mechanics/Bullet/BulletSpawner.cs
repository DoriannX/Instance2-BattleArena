using UnityEngine;
using UnityEngine.Pool;

public class BulletSpawner : MonoBehaviour
{
    [Header("Reference")]
    private Shoot _shoot;
    [SerializeField] private Transform _spawnBullet;
    public ObjectPool<Bullet> BulletSpawnerPool;

    [Header("Settings")]
    [SerializeField] private int _maxSaveBullet = 20;
    [SerializeField] private int _maxBulletOverall = 50;

    private void Awake()
    {
        _shoot = GetComponent<Shoot>();
        BulletSpawnerPool = new(CreateBullet, OnTakeBulletFromPool, OnReturnBulletToPool, OnDestroyBullet, true, _maxSaveBullet, _maxBulletOverall);
    }

    //What it does when more object than in the object pool
    private Bullet CreateBullet()
    {
        Bullet bullet = Instantiate(_shoot.BulletPrefab, _shoot.PlayerTransform.position + _shoot.PlayerTransform.up, _shoot.PlayerTransform.rotation);
    
        bullet.SetPool(BulletSpawnerPool);

        return bullet;  
    }

    //What it does when object take from object pool
    private void OnTakeBulletFromPool(Bullet bullet)
    {
        //set the transform and rotation
        bullet.transform.position = _spawnBullet.transform.position + _shoot.PlayerTransform.up;
        bullet.transform.rotation = _shoot.PlayerTransform.rotation;

        //activate
        bullet.gameObject.SetActive(true);
    }

    //Whats it does when objet return to object pool
    private void OnReturnBulletToPool(Bullet bullet)
    {
        bullet.gameObject.SetActive(false);
    }

    //What it does to destroy object instead of returning in object pool
    private void OnDestroyBullet(Bullet bullet)
    {
        Destroy(bullet.gameObject);
    }
}
