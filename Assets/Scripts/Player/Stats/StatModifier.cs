using UnityEngine;

public class StatModifier 
{
    public float value;
    public ModifierType type;
    public object source;
    public StatModifier(float value, ModifierType type, object source = null)
    {
        this.value = value;
        this.type = type;
        this.source = source;
    }
}
