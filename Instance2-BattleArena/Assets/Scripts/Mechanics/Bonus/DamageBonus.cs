using UnityEngine;

public class DamageBonus : BonusEffects
{
    public float DamageMultiplier = 1.5f;

    public override void ApplyEffect(GameObject player)
    {
            Debug.Log("Boost de dégâts appliqué !");
    }
}
