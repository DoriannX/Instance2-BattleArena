using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;

public class Bullet : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float _speed;

    private void Awake()
    {
        Assert.IsNotNull(_speed, "_speed is null");
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        transform.position += transform.up * _speed * Time.deltaTime;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Touched");
        //Destroy(gameObject);
    }
}
