using NUnit.Framework.Constraints;
using UnityEngine;

[CreateAssetMenu(fileName ="New Upgrade Card_",menuName = "Scriptable Objects/Upgrade Card")]
public class UpgradeCard : ScriptableObject
{
    public string cardName;
    [TextArea] public string cardDescription;
    public Sprite icon;
    public StatType statToUpgrade;
    public float amount;
    public ModifierType modifierType;
    public void Apply(PlayerStats player)
    {
        var stat = player.GetStat(statToUpgrade);
        if(stat != null)
        {
            stat.AddModifier(new StatModifier(amount, modifierType,this));
        }
    }
}
