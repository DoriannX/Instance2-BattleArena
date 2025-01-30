using UnityEngine;
using UnityEngine.Pool;

public class BulletSpawner : MonoBehaviour
{
    [Header("Reference")]
    private Shoot _shoot;
    public ObjectPool<Bullet> BulletSpawnerPool;

    private void Awake()
    {
        _shoot = GetComponent<Shoot>();
        BulletSpawnerPool = new(CreateBullet, OnTakeBulletFromPool, OnReturnBulletToPool, OnDestroyBullet, true, 20, 50);

    }

    //What it does when more object than in object pool
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
        bullet.transform.position = _shoot.PlayerTransform.position + _shoot.PlayerTransform.up;
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
