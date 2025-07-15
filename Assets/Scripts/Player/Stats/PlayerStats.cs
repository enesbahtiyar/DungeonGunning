using System;
using UnityEngine;

public class PlayerStats : SingletonMonoBehaviour<PlayerStats>
{
    [Header("Player Stats")]
    public Stat maxHealth;
    public Stat attackPower;
    public Stat fireRate;
    public Stat movementSpeed;
    public Stat attackRange;
    //public Stat criticalChance;
    //public Stat criticalDamage;
    //public Stat gold;
    [Header("Level & XP")]
    public int Level = 1;
    public int CurrentXP = 0;
    public int XPToNextLevel = 100;

    public event Action<int> OnLevelUp;
    public event Action<int, int> OnXPChanged;
    public Stat cooldownModifier;

    private void Start()
    {
        ApplyModifier(StatType.AttackPower, new StatModifier(10, ModifierType.Flat, this));
    }
    public void GainXp(int amount)
    {
        CurrentXP += amount;
        OnXPChanged?.Invoke(CurrentXP, XPToNextLevel);
        while (CurrentXP >= XPToNextLevel)
        {
            CurrentXP -= XPToNextLevel;
            Level++;
            XPToNextLevel = CalculateXPNeeded(Level);

            ApplyLevelUpBonusses();
            OnLevelUp?.Invoke(Level);
            OnXPChanged?.Invoke(CurrentXP, XPToNextLevel);
        }
    }
    private int CalculateXPNeeded(int level)
    {
        return (int)(100 * Mathf.Pow(1.1f, level - 1)); // Example formula for XP needed per level
    }
    public void ApplyLevelUpBonusses()
    {
        maxHealth.baseValue += 10; // Example bonus
        attackPower.baseValue += 2; // Example bonus
        fireRate.baseValue += 0.1f; // Example bonus
        movementSpeed.baseValue += 0.1f; // Example bonus
        attackRange.baseValue += 0.1f; // Example bonus
        cooldownModifier.baseValue += 0.05f; // Example bonus
    }
    public void ApplyModifier(StatType statType, StatModifier modifier)
    {
        GetStat(statType)?.AddModifier(modifier);
    }

    public void RemoveModifierFromSource(object source)
    {
        
        //TODO: Check Here if you are having problems with optimisation
        
        maxHealth.RemoveModifierFromSource(source);
        attackPower.RemoveModifierFromSource(source);
        fireRate.RemoveModifierFromSource(source);
        movementSpeed.RemoveModifierFromSource(source);
        attackRange.RemoveModifierFromSource(source);
        cooldownModifier.RemoveModifierFromSource(source);

    }
    public Stat GetStat(StatType statType)
    {
        return statType switch
        {
            StatType.Health => maxHealth,
            StatType.AttackPower => attackPower,
            StatType.FireRate => fireRate,
            StatType.MoveSpeed => movementSpeed,
            StatType.CooldownModifier => cooldownModifier,
            StatType.AttackRange => attackRange,
            _ => throw new ArgumentOutOfRangeException(nameof(statType), statType, null)
        };
    }
}
