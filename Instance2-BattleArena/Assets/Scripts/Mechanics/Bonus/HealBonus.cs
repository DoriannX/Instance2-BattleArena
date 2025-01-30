using UnityEngine;

public class HealBonus : BonusEffects
{
    public float HealAmount = 20f;

    public override void ApplyEffect(GameObject player)
    {
            Debug.Log("Soin appliqué !");
    }
}
