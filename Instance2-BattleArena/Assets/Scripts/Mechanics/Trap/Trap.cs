using UnityEngine;

public class Trap : MonoBehaviour
{
    private TrapEffects _trapEffects;
    private bool _isPlayerInside = false;

    private void Start()
    {
        _trapEffects = GetComponent<TrapEffects>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && _trapEffects != null)
        {
            _isPlayerInside = true;
            _trapEffects.ApplyEffect(other.gameObject);
        }
    }
}
