using Unity.Netcode;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

namespace Mechanics.Player
{
    public class SimpleMovement : NetworkBehaviour
    {
        [SerializeField] private float _speed = 1f;
        [SerializeField] private Bullet _bulletPrefab;
        private Transform _transform;
        private Vector3 _shootDir;

        private void Awake()
        {
            _transform = transform;
            Assert.IsNotNull(_bulletPrefab, "Bullet prefab missing");
        }

        private void Update()
        {
            AskMove();
            TryShoot();
        }

        private void TryShoot()
        {
            if (!IsOwner || !Input.GetKeyDown(KeyCode.Space))
            {
                return;
            }

            Aim();

            Bullet instancedBullet = Instantiate(_bulletPrefab, _transform.position, _transform.rotation);
            instancedBullet.SetDirection(_shootDir);
        }

        private void Aim()
        {
            
            _shootDir = (Mouse.current.position.ReadValue() - new Vector2(Screen.width / 2f, Screen.height / 2f))
                .normalized;
            
            Debug.DrawRay(Vector3.zero, _shootDir * 100f, Color.red, 1f);
        }

        private void AskMove()
        {
            if (!IsOwner)
            {
                return;
            }

            Vector3 direction = Vector3.zero;
            if (Input.GetKey(KeyCode.W))
            {
                direction += Vector3.up;
            }

            if (Input.GetKey(KeyCode.A))
            {
                direction += Vector3.left;
            }

            if (Input.GetKey(KeyCode.S))
            {
                direction += Vector3.down;
            }

            if (Input.GetKey(KeyCode.D))
            {
                direction += Vector3.right;
            }

            _transform.position += _speed * Time.deltaTime * direction.normalized;
        }
    }
}