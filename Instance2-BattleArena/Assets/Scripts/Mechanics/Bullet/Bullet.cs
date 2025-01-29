using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

public class Bullet : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float _speedBullet;
    private Transform _bulletTransform;

    private void Awake()
    {
        _bulletTransform = transform;
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        _bulletTransform.position += _bulletTransform.up * _speedBullet * Time.deltaTime;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Touched");
        //Destroy(gameObject);
    }
}
