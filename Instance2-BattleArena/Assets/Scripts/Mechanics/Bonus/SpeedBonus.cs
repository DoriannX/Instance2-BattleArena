using UnityEngine;

public class SpeedBonus : BonusEffects
{
    public override void ApplyEffect(GameObject player)
    {
        PlayerMovements playerMovements = player.GetComponent<PlayerMovements>();
        if (playerMovements != null)
        {
            playerMovements.ApplyMovementBoost();
        }
    }
}
