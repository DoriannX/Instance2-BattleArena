using System;
using Unity.Netcode;
using UnityEngine;

namespace Mechanics.Player
{
    public class Bullet : NetworkBehaviour  
    {
        private Transform _transform;
        [SerializeField] private float _speed;
        private Vector3 _direction;
        
        private void Move()
        {
            if (!IsOwner)
            {
                return;
            }

            _transform.position += Time.deltaTime * _speed * _direction;
        }

        private void Update()
        {
            Move();
        }

        //To init
        public void SetDirection(Vector3 direction)
        {
            _direction = direction;
        }
    }
}