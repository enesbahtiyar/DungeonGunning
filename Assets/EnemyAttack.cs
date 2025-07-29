using UnityEngine;
//Osman'ın Hasar sistemi ödevi
public class EnemyAttack : MonoBehaviour
{
    public float attackRange = 1.5f;
    public float attackCooldown = 1.0f;
    private float lastAttackTime = 0f;
    public int attackDamage = 10;
    public LayerMask playerLayer;
    public PlayerStats playerStats;
    public StatModifier attackModifier;
    public StatModifier cooldownModifier;
    public StatModifier attackRangeModifier;
    public StatModifier attackDamageModifier;
    public StatModifier attackSpeedModifier;

    void Start()
    {
playerStats = PlayerStats.Instance;
        if (playerStats == null)
        {
            Debug.LogError("PlayerStats instance not found!");
            return;
        }

        attackModifier = new StatModifier(attackDamage, ModifierType.Flat, this);
        cooldownModifier = new StatModifier(0.1f, ModifierType.Percent, this);
        attackRangeModifier = new StatModifier(0.5f, ModifierType.Flat, this);
        attackDamageModifier = new StatModifier(5, ModifierType.Flat, this);
        attackSpeedModifier = new StatModifier(0.2f, ModifierType.Flat, this);
    }

    // Update is called once per frame
    void Update()
    {
        if (Physics2D.OverlapCircle(transform.position, attackRange, playerLayer))
        {
            if (Time.time >= lastAttackTime + attackCooldown)
            {
                Attack();
                lastAttackTime = Time.time;
            }
        }
    }
    void Attack()
    {
        // Oyuncunun OsmanHealth bileşenini bul ve can azalt
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            OsmanHealth osmanHealth = player.GetComponent<OsmanHealth>();
            if (osmanHealth != null)
            {
                osmanHealth.DecreaseHealth(attackDamage);
            }
        }
    }
}
