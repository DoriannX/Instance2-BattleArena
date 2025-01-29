using UnityEngine;

public class SpeedBonus : BonusEffects
{
    public float SpeedMultiplier = 1.2f;

    public override void ApplyEffect(GameObject player)
    {
        Debug.Log("Boost de vitesse appliqué !");
    }
}
