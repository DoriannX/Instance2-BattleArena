using UnityEngine;

public class HealBonus : BonusEffects
{
 
    public override void ApplyEffect(GameObject player)
    {
        PlayerStats playerStats = player.GetComponent<PlayerStats>();

        if (playerStats != null)
        {
            playerStats.ApplyHealBonus(75);
        }
    }
}
