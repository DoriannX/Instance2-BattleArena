using System.Collections;
using UnityEngine;

public class Bonus : MonoBehaviour
{
    public enum BonusType { Heal, DamageBoost, SpeedBoost }
    public BonusType bonusType;

    [Header("Bonus Values")]
    public float HealAmount = 20f;
    public float DamageMultiplier = 1.5f;
    public float SpeedMultiplier = 1.5f;
    public float EffectDuration = 5f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            ApplyBonus();
            RandomSpawner.Instance?.RespawnBonus(gameObject);
            Destroy(gameObject);
        }
    }

    private void ApplyBonus()
    {
        switch (bonusType)
        {
            case BonusType.Heal:
                Debug.Log("Bonus de soin appliqué !");
                break;
            case BonusType.DamageBoost:
                Debug.Log("Bonus de dégâts appliqué !");
                break;
            case BonusType.SpeedBoost:
                Debug.Log("Bonus de vitesse appliqué !");
                break;
        }
    }
}
