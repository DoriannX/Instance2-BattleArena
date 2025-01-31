using UnityEngine;
using System.Collections;

public class PlayerStats : MonoBehaviour
{
    public int Attack;
    public int CurrentHealth; 
    public int MaxHealth;
    private int _initialAttack;
    private int _initialHeal;

    private bool _isDamageBonusActive = false;

    public void SetStats(int attack, int maxHealth)
    {
        Attack = attack;
        MaxHealth = maxHealth;
        CurrentHealth = MaxHealth;
        _initialAttack = attack;
        _initialHeal = maxHealth;
    }

    public void IncreaseStats()
    {
        Attack = Mathf.RoundToInt(Attack * 1.05f);
        MaxHealth = Mathf.RoundToInt(MaxHealth * 1.05f);  
    }

    public void ResetStats(int baseAttack, int baseMaxHealth)
    {
        Attack = baseAttack;
        MaxHealth = baseMaxHealth;
        CurrentHealth = MaxHealth;  
    }

    public void ApplyHealBonus(float healAmount)
    {
        if (CurrentHealth < MaxHealth)
        {
            int healToApply = Mathf.RoundToInt(healAmount);
            CurrentHealth = Mathf.Min(CurrentHealth + healToApply, MaxHealth); 
        }
    }
    public void ApplyDamageBonus(float damageMultiplier, float duration)
    {
        if (_isDamageBonusActive) return;  

        _isDamageBonusActive = true;  
        _initialAttack = Attack; 
        Attack = Mathf.RoundToInt(Attack * damageMultiplier);  

        StartCoroutine(RemoveDamageBonusAfterDuration(duration));
    }

    private IEnumerator RemoveDamageBonusAfterDuration(float duration)
    {
        yield return new WaitForSeconds(duration);  

        Attack = _initialAttack;  
        _isDamageBonusActive = false;  
    }

}
