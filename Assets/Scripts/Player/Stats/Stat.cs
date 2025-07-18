using UnityEngine;
using System.Collections.Generic;
using System;

[System.Serializable]
public class Stat
{
    public float baseValue;

    private readonly List<StatModifier> modifiers = new List<StatModifier>();

    private bool isValueChanged = true;
    private float _value;

    public event Action<float> OnValueChanged;

    public float Value
    {
        get
        {
            if (isValueChanged)
            {
                float oldValue = _value;
                _value = CalculateFinalValue();
                isValueChanged = false;

                if (Mathf.Abs(oldValue - _value) > 0.001f)
                {
                    OnValueChanged?.Invoke(_value);
                }
            }
            return _value;
        }
    }
    public void SetBaseValue(float newValue)
    {
        baseValue = newValue;
        isValueChanged = true;
        OnValueChanged?.Invoke(Value);
    }
    public void AddModifier(StatModifier modifier)
    {
        modifiers.Add(modifier);
        isValueChanged = true;
    }

    public void RemoveModifier(StatModifier modifier)
    {
        if (modifiers.Remove(modifier))
        {
            isValueChanged = true;
        }
    }

    public void RemoveModifierFromSource(object source)
    {
        if (modifiers.RemoveAll(m => m.source == source) > 0)
        {
            isValueChanged = true;
        }
    }

    public void ClearModifiers()
    {
        if (modifiers.Count > 0)
        {
            modifiers.Clear();
            isValueChanged = true;
        }
    }

    public void ResetBaseValue()
    {
        baseValue = 0f;
        isValueChanged = true;
    }

    private float CalculateFinalValue()
    {
        float finalValue = baseValue;

        foreach (var modifier in modifiers)
        {
            switch (modifier.type)
            {
                case ModifierType.Flat:
                    finalValue += modifier.value;
                    break;
                case ModifierType.Percent:
                    finalValue *= 1 + modifier.value;
                    break;
            }
        }

        return finalValue;
    }
}
////---- Example for how to keep value updated ----
//private void Start()
//{
//    PlayerStats.Instance.movementSpeed.OnValueChanged += OnMoveSpeedChanged;
//}
//private void OnMoveSpeedChanged(float newValue)
//{
//    Debug.Log("Yeni hareket hızı: " + newValue);
//}