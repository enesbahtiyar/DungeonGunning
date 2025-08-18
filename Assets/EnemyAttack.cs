using UnityEngine;

public enum AttackType
{
    Melee,
    Ranged
}

public class EnemyAttack : MonoBehaviour
{
    public AttackType attackType = AttackType.Melee;
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
    public AudioClip attackSound;
    private AudioSource audioSource;
    
    [Header("Ranged Attack Settings")]
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float projectileSpeed = 5f;
    public float projectileLifetime = 3f;
    public float rangedAttackRange = 5f;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
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

    public bool CanAttack()
    {
        return Time.time >= lastAttackTime + attackCooldown;
    }
    
    public void TryAttack()
    {
        if (CanAttack())
        {
            Attack();
            lastAttackTime = Time.time;
        }
    }
    void Attack()
    {
        if (attackSound != null)
        {
            audioSource.PlayOneShot(attackSound);
        }
        
        if (attackType == AttackType.Melee)
        {
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
        else if (attackType == AttackType.Ranged)
        {
            if (projectilePrefab != null && firePoint != null)
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                if (player != null)
                {
                    Vector2 direction = (player.transform.position - firePoint.position).normalized;
                    GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
                    EnemyProjectile projScript = projectile.GetComponent<EnemyProjectile>();
                    if (projScript != null)
                    {
                        projScript.Initialize(direction, projectileSpeed, attackDamage, projectileLifetime);
                    }
                }
            }
        }
    }
}
