using UnityEngine;
using System.Collections;

public class BoxOfTacks : TrapEffects
{
    public float DamagePerTick = 20f;
    public float TickInterval = 2f;
    public float TrapDuration = 10f;

    private Coroutine _damageCoroutine;
    private bool _playerInside = false;

    private void Start()
    {
        StartCoroutine(DestroyTrapAfterDuration()); 
    }

    public override void ApplyEffect(GameObject player)
    {
        PlayerStats playerStats = player.GetComponent<PlayerStats>();
        PlayerMovements playerMovements = player.GetComponent<PlayerMovements>();

        if (playerStats != null && _damageCoroutine == null)
        {
            _playerInside = true;
            _damageCoroutine = StartCoroutine(DealDamageOverTime(playerStats));
        }

        if (playerMovements != null)
        {
            playerMovements.ApplyMovementSlow();
        }
    }

    private IEnumerator DealDamageOverTime(PlayerStats playerStats)
    {
        while (_playerInside)
        {
            playerStats.TakeDamage(20);
            playerStats.UpdateHealthBar();
            yield return new WaitForSeconds(TickInterval);
        }
    }

    private IEnumerator DestroyTrapAfterDuration()
    {
        yield return new WaitForSeconds(TrapDuration);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _playerInside = false;

            PlayerMovements playerMovements = other.GetComponent<PlayerMovements>();
            if (playerMovements != null)
            {
                playerMovements.ResetMovementSpeed(); 
                Destroy(gameObject , 2f);
            }

            if (_damageCoroutine != null)
            {
                StopCoroutine(_damageCoroutine);
                _damageCoroutine = null;
            }
        }
    }
}
