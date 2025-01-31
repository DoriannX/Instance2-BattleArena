using UnityEngine;
using System.Collections;

public class BoxOfTacks : TrapEffects
{
    public float DamagePerTick = 20f;
    public float TickInterval = 2f;

    public override void ApplyEffect(GameObject player)
    {
        PlayerStats playerStats = player.GetComponent<PlayerStats>();
        PlayerMovements playerMovements = player.GetComponent<PlayerMovements>();

        if (playerStats != null)
        {
            playerStats.StartCoroutine(DealDamageOverTime(playerStats));
        }

        if (playerMovements != null)
        {
            playerMovements.ApplyMovementSlow();
        }
    }

    private IEnumerator DealDamageOverTime(PlayerStats playerStats)
    {
        int ticks = 2; 
        for (int i = 0; i < ticks; i++)
        {
            playerStats.CurrentHealth -= Mathf.RoundToInt(DamagePerTick);
            yield return new WaitForSeconds(TickInterval);
        }
        Destroy(gameObject);
    }
}
