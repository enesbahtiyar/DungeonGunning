using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(OsmanHealth))]
public class EnemyBase : MonoBehaviour
{
    [SerializeField] private float movementSpeed;
    [SerializeField] private float hitRange;
    [SerializeField] private float maxHealth;
    [SerializeField] private Animator animator;
    [SerializeField] private Collider2D hitbox;
    [SerializeField] private GameObject xpOrb;
    [SerializeField, Range(0f, 1f)] protected float xpDropChance = 0.2f; // XP orb drop chance
    [SerializeField] private GameObject coinPrefab;
    [SerializeField, Range(0f, 1f)] protected float coinDropChance = 0.3f; // Coin drop chance
    public int coinValue = 10;

    [SerializeField] private GameObject healthOrbPrefab;
    [SerializeField, Range(0f, 1f)] protected float healthDropChance = 0.05f; // Health orb drop chance
    [SerializeField] private GameObject ammoPrefab;
    [SerializeField, Range(0f, 1f)] protected float ammoDropChance = 0.05f; // Ammo drop chance
    public int xpReward;
    private float currentHealth;
    [SerializeField, Tooltip("Attack Per Second")] private float attackRate;
    private float nextAttackTime;
    public GameObject player;
    private SpriteRenderer spriteRenderer;
    private bool isDead = false;
    private bool playerInHitRange;
    private float baseMovementSpeed;
    
    [Header("Speed Boost")]
    public bool farFromPlayer = false;
    [SerializeField] private float speedBoostDistance = 10f;
    [SerializeField] private float normalDistance = 5f;
    [SerializeField] private float speedAmount = 2f;
    private bool isSpeedBoosted = false;
    private bool playingState = false;
    
    private EnemyPool enemyPool;

    [SerializeField] EnemyState state;
    public virtual void Start()
    {
        baseMovementSpeed = movementSpeed;
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentHealth = maxHealth;
        state = EnemyState.chasing;
        enemyPool = GameObject.Find("GameManager").GetComponent<EnemyPool>();
        movementSpeed = Random.Range(movementSpeed * 0.8f, movementSpeed * 1.2f);

    }

    public void OnEnable()
    {
        GameManager.Instance.OnGameStateChanged += Instance_OnGameStateChanged;
        ChangeEnemyState(EnemyState.chasing);
        isDead = false;
        hitbox.enabled = true;
    }

    private void Instance_OnGameStateChanged(GameState obj)
    {
        playingState = obj == GameState.Playing;
    }

    public virtual void Update()
    {
        if (!playingState) return;
        if (!isDead)
        {
            LookToPlayer();
            //TODO: Can be OPTIMIZED with SEP(Single Entry Point)
            float distanceToPlayer = Vector2.Distance(player.transform.position, transform.position);
            playerInHitRange = distanceToPlayer < hitRange;
            
            if (farFromPlayer)
            {
                HandleSpeedBoost(distanceToPlayer);
            }
        }
        switch (state)
        {
            case EnemyState.idle:
                Idle();
                break;
            case EnemyState.chasing:
                ChasePlayer();
                break;
            case EnemyState.attacking:
                AttackToPlayer();
                break;
            case EnemyState.die:
                Die();
                break;

        }
    }
    public virtual void Idle()
    {
        if (player == null)
        {
            animator.SetTrigger("isIdle");
        }
        else
        {
            ChangeEnemyState(EnemyState.chasing);
        }
    }
    public virtual void ChasePlayer()
    {
        EnemyAttack enemyAttack = GetComponent<EnemyAttack>();
        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);
        
        if (enemyAttack != null && enemyAttack.attackType == AttackType.Ranged)
        {
            if (distanceToPlayer <= enemyAttack.rangedAttackRange)
            {
                ChangeEnemyState(EnemyState.attacking);
                return;
            }
        }
        
        if (!playerInHitRange)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, movementSpeed * Time.deltaTime);
            animator.SetTrigger("isChasing");
        }
        else
        {
            ChangeEnemyState(EnemyState.attacking);
        }
    }
    public virtual void AttackToPlayer()
    {
        EnemyAttack enemyAttack = GetComponent<EnemyAttack>();
        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);
        
        if (enemyAttack != null && enemyAttack.attackType == AttackType.Ranged)
        {
            if (distanceToPlayer > enemyAttack.rangedAttackRange)
            {
                ChangeEnemyState(EnemyState.chasing);
                return;
            }
        }
        else if (!playerInHitRange)
        {
            ChangeEnemyState(EnemyState.chasing);
            return;
        }

        if (Time.time > nextAttackTime)
        {
            animator.SetTrigger("isAttacking");
            nextAttackTime = Time.time + 1f / attackRate;
            
            if (enemyAttack != null)
            {
                enemyAttack.TryAttack();
            }
        }
        else
        {
            animator.SetTrigger("isIdle");
        }
    }
   
      public virtual void Die()
    {
        if (isDead) return;
        isDead = true;

        hitbox.enabled = false;
        animator.SetTrigger("isDead");
        Invoke("DeactivateObject", 2f);

        
        
        // 🟢 Oyuncuya direkt XP veriyoruz
        PlayerStats.Instance.GainXp(xpReward);

        // Sadece bir drop olacak şekilde sırayla kontrol et
        float roll = Random.value;
        if (roll < xpDropChance && xpOrb != null)
        {
            GameObject xp = Instantiate(xpOrb, transform.position, Quaternion.identity);
            xp.GetComponent<XpOrb>().xpAmount = xpReward;
        }
        else if (roll < xpDropChance + coinDropChance && coinPrefab != null)
        {
            GameObject droppedCoin = Instantiate(coinPrefab, transform.position, Quaternion.identity);
            if (droppedCoin != null)
            {
                droppedCoin.GetComponent<Coin>().coinValue = coinValue;
            }
        }
        else if (roll < xpDropChance + coinDropChance + healthDropChance && healthOrbPrefab != null)
        {
            Instantiate(healthOrbPrefab, transform.position, Quaternion.identity);
        }
        else if (roll < xpDropChance + coinDropChance + healthDropChance + ammoDropChance && ammoPrefab != null)
        {
            Instantiate(ammoPrefab, transform.position, Quaternion.identity);
        }
    }


    public void DeactivateObject()
    {
        this.gameObject.SetActive(false);
    }
    
    public virtual void ChangeEnemyState(EnemyState state)
    {
        if (this.state == state) return;
        this.state = state;
    }
    public void TakeDamage()
    {
        animator.SetTrigger("isTakingDamage");
        StartCoroutine(StaggerEffect());
    }
    public void HitToPlayer()
    {
        if (playerInHitRange)
        {
            Debug.Log("Player got hit");
        }
    }
    private void LookToPlayer()
    {
        spriteRenderer.flipX = player.transform.position.x > transform.position.x;
    }

    private IEnumerator StaggerEffect()
    {
        movementSpeed = 0;
        yield return new WaitForSeconds(0.5f);
        movementSpeed = baseMovementSpeed;
    }
    
    private void HandleSpeedBoost(float distanceToPlayer)
    {
        if (!isSpeedBoosted && distanceToPlayer > speedBoostDistance)
        {
            movementSpeed = baseMovementSpeed * speedAmount;
            isSpeedBoosted = true;
        }
        else if (isSpeedBoosted && distanceToPlayer <= normalDistance)
        {
            movementSpeed = baseMovementSpeed;
            isSpeedBoosted = false;
        }
    }

}