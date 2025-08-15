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
    [Range(0f, 100f)]
    public float xpIncreasePercentage = 10f;

    public event Action<int> OnLevelUp;
    public event Action<int, int> OnXPChanged;
    public event Action<int> OnCoinAdded;
    public Stat cooldownModifier;

    [Header("Audio")]
    public AudioClip levelUpSound;
    public AudioClip coinSound;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        ApplyModifier(StatType.AttackPower, new StatModifier(10, ModifierType.Flat, this));
        OnCoinAdded?.Invoke(coinCount);
    }
    public void GainXp(int amount)
    {
        CurrentXP += amount;
        OnXPChanged?.Invoke(CurrentXP, XPToNextLevel);
        while (CurrentXP >= XPToNextLevel)
        {
            CurrentXP -= XPToNextLevel;
            Level++;
            if (levelUpSound != null)
            {
                audioSource.PlayOneShot(levelUpSound);
            }
            XPToNextLevel = CalculateXPNeeded(Level);

            ApplyLevelUpBonusses();
            OnLevelUp?.Invoke(Level);
            LevelUpManager.Instance.ShowUpgradeChoices(Level);
            OnXPChanged?.Invoke(CurrentXP, XPToNextLevel);
        }
    }
    private int CalculateXPNeeded(int level)
    {
        // Eski metod: return (int)(100 * Mathf.Pow(1.1f, level - 1));
        float multiplier = 1f + (xpIncreasePercentage / 100f);
        return (int)(100 * Mathf.Pow(multiplier, level - 1));
    }
    public void ApplyLevelUpBonusses()
    {
        maxHealth.SetBaseValue(maxHealth.baseValue + 10);
        attackPower.SetBaseValue(attackPower.baseValue + 2);
        fireRate.SetBaseValue(fireRate.baseValue + 0.1f);
        movementSpeed.SetBaseValue(movementSpeed.baseValue + 0.1f);
        attackRange.SetBaseValue(attackRange.baseValue + 0.1f);
        cooldownModifier.SetBaseValue(cooldownModifier.baseValue + 0.05f);
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
            StatType.MaxHealth => maxHealth,
            StatType.AttackPower => attackPower,
            StatType.FireRate => fireRate,
            StatType.MoveSpeed => movementSpeed,
            StatType.CooldownModifier => cooldownModifier,
            StatType.AttackRange => attackRange,
            
            _ => throw new ArgumentOutOfRangeException(nameof(statType), statType, null)
        };
    }
    public int coinCount = 0; // �u anki alt�n say�s�

    public void AddCoins(int amount)
    {
        coinCount += amount;
        OnCoinAdded?.Invoke(coinCount);
        if (coinSound != null)
        {
            audioSource.PlayOneShot(coinSound);
        }
    }
}
