using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

public class PlayerMovements : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private InputActionReference _playerMovement;
    private Rigidbody2D _playerRigidbody;
    private Transform _playerTransform;

    [Header("Movement")]
    [SerializeField] private float _playerAcceleration = 1f;
    [SerializeField] private float _playerMaxSpeed = 1f;

    private void Awake()
    {       
        Assert.IsNotNull(_playerMovement, "_inputAction is missing");

        _playerRigidbody = GetComponent<Rigidbody2D>();
        _playerTransform = transform;
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        Vector2 moveDirection = _playerMovement.action.ReadValue<Vector2>().normalized;
        _playerRigidbody.linearVelocity += _playerAcceleration * Time.deltaTime * new Vector2(moveDirection.x, moveDirection.y);
        if (_playerRigidbody.linearVelocity.magnitude > _playerMaxSpeed ) _playerRigidbody.linearVelocity = Vector2.ClampMagnitude(_playerRigidbody.linearVelocity, _playerMaxSpeed);

        Vector3 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - _playerTransform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotZ = Quaternion.AngleAxis(angle - 90, Vector3.forward);
        _playerTransform.rotation = rotZ;
    }
}
