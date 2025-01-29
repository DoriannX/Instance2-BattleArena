using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovements : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private InputActionReference _inputAction;
    private Rigidbody2D _rigidbody;

    [Header("Movement")]
    [SerializeField] private float _acceleration = 1f;
    [SerializeField] private float _maxSpeed = 1f;

    private void Awake()
    {       
        Assert.IsNotNull(_inputAction, "_inputAction is missing");
        Assert.IsNotNull(_maxSpeed, "_maxSpeed is null");
        Assert.IsNotNull(_acceleration, "_acceleration is null");

        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        Vector2 moveDirection = _inputAction.action.ReadValue<Vector2>().normalized;
        _rigidbody.linearVelocity += _acceleration * Time.deltaTime * new Vector2(moveDirection.x, moveDirection.y);
        if (_rigidbody.linearVelocity.magnitude > _maxSpeed ) _rigidbody.linearVelocity = Vector2.ClampMagnitude(_rigidbody.linearVelocity, _maxSpeed);

        Vector3 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotZ = Quaternion.AngleAxis(angle - 90, Vector3.forward);
        transform.rotation = rotZ;
    }
}
