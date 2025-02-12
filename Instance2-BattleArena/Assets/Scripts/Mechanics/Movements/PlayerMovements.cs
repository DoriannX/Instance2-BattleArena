using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerMovements : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private InputActionReference _playerMovement;
    private Rigidbody2D _playerRigidbody;
    private Transform _playerTransform;

    [Header("Movement")]
    [SerializeField] private float _playerAcceleration = 1f;
    [SerializeField] private float _playerMaxSpeed = 1f;

    [Header("Animator")]
    [SerializeField] private Animator _animator;

    private float _originalAcceleration;
    private float _originalMaxSpeed;

    private float _originalSlow;
    private float _originalSlowMaxSpeed;

    private bool _isBoostActive = false;
    private bool _isSlow = false;

    private void Awake()
    {       
        Assert.IsNotNull(_playerMovement, "_inputAction is missing");

        _playerRigidbody = GetComponent<Rigidbody2D>();
        _playerTransform = transform;

        _originalAcceleration = _playerAcceleration;
        _originalMaxSpeed = _playerMaxSpeed;

        _originalSlow = _playerAcceleration;
        _originalSlowMaxSpeed = _playerMaxSpeed;
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
        _animator.SetBool("IsMoving", true);

        if(_playerRigidbody.linearVelocity.magnitude <= 3) 
        {
            _animator.SetBool("IsMoving", false);
        }
    }

    public void ApplyMovementBoost()
    {
        if (!_isBoostActive)
        {
            _isBoostActive = true;
            _playerAcceleration *= 1.5f;  
            _playerMaxSpeed *= 1.5f;  
            StartCoroutine(ResetMovementBoostAfterDuration(10f));
        }
    }

    private IEnumerator ResetMovementBoostAfterDuration(float duration)
    {
        yield return new WaitForSeconds(duration);

        _playerAcceleration = _originalAcceleration;
        _playerMaxSpeed = _originalMaxSpeed;
        _isBoostActive = false; 
    }

    public void ApplyMovementSlow()
    {
        if (!_isSlow)
        {
            _isSlow = true;
            _playerAcceleration *= 0.5f;
            _playerMaxSpeed *= 0.5f;
            StartCoroutine(ResetMovementSlowAfterDuration(6f));
        }
    }

    private IEnumerator ResetMovementSlowAfterDuration(float duration)
    {
        yield return new WaitForSeconds(duration);

        _playerAcceleration = _originalSlow;
        _playerMaxSpeed = _originalSlowMaxSpeed;
        _isSlow = false;
    }

    public void ResetMovementSpeed()
    {
        _playerAcceleration = _originalAcceleration;
        _playerMaxSpeed = _originalMaxSpeed;
        _isSlow = false;
    }
}
