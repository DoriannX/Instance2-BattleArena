using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

public class HitMele : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private InputActionReference _hitMele;
    [SerializeField] private int attackDamage = 20;
    [SerializeField] private float attackSpeed = 0.2f; 

    private Collider2D _playerInRangeCollider2D;
    private bool _isInCollider = false;

    private float timeSinceLastHit = 0f;  

    private void Awake()
    {
        Assert.IsNotNull(_hitMele, "_inputAction is missing");
    }

    private void OnEnable()
    {
        _hitMele.action.started += Hit;
    }

    private void OnDisable()
    {
        _hitMele.action.started -= Hit;
    }

    private void Update()
    {
        timeSinceLastHit += Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _playerInRangeCollider2D = other;
            _isInCollider = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _isInCollider = false;
            _playerInRangeCollider2D = null;
        }
    }

    private void Hit(InputAction.CallbackContext context)
    {
        if (_isInCollider && _playerInRangeCollider2D != null && timeSinceLastHit >= attackSpeed)
        {
            PlayerStats playerStats = _playerInRangeCollider2D.GetComponent<PlayerStats>();

            if (playerStats != null)
            {
                playerStats.TakeDamage(playerStats.Attack);
                
            }
            timeSinceLastHit = 0f;
        }
    }
}
