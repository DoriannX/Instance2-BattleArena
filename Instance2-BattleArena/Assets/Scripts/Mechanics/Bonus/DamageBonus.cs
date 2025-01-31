using UnityEngine;

public class DamageBonus : BonusEffects
{
    public float DamageMultiplier = 1.2f;  
    private float _duration = 10f;  

    public override void ApplyEffect(GameObject player)
    {
        PlayerStats playerStats = player.GetComponent<PlayerStats>();

        if (playerStats != null)
        {
            playerStats.ApplyDamageBonus(DamageMultiplier, _duration);
        }
    }
}
