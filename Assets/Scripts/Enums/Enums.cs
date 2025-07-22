public enum StatType
{
    MaxHealth,
    AttackPower,
    FireRate,
    MoveSpeed,
    CooldownModifier,
    AttackRange
}

public enum ModifierType
{
    Flat,
    Percent
}

public enum EntityType
{
    AI,
    Player
}

public enum EnemyState
{
    idle,
    chasing,
    attacking,
    die
}