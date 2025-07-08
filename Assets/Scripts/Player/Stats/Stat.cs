using UnityEngine;
using System.Collections.Generic;
[System.Serializable]
public class Stat
{
   public float baseValue;
    private readonly List<StatModifier> modifiers = new List<StatModifier>();
    public float Value
    {
        get
        {
            float finalValue = baseValue;
            foreach (var modifier in modifiers)
            {
                if (modifier.type == ModifierType.Flat)
                {
                    finalValue += modifier.value;
                }
                else if (modifier.type == ModifierType.Percent)
                {
                    finalValue *= 1 + modifier.value;
                }
            }
            return finalValue;
        }
    }
    public void AddModifier(StatModifier modifier)
    {
        modifiers.Add(modifier);
    }
    public void RemoveModifierFromSource(object source)
    {
        modifiers.RemoveAll(m => m.source == source);
    }
    public void ClearModifiers()
    {
        modifiers.Clear();
    }
    public void ResetBaseValue()
    {
        baseValue = 0f;
    }

}
