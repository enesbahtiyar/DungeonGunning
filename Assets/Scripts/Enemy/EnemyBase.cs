using System.Collections;
using UnityEngine;
[RequireComponent(typeof(Health))]
public class EnemyBase : MonoBehaviour
{
    [SerializeField] private float movementSpeed;
    [SerializeField] private float hitRange;
    [SerializeField] private float maxHealth;
    [SerializeField] private Animator animator;
    private float currentHealth;
    [SerializeField, Tooltip("Attack Per Second")] private float attackRate;
    private float nextAttackTime;
    public GameObject player;
    private SpriteRenderer spriteRenderer;
    private bool isDead = false;
    private bool playerInHitRange;
    private float baseMovementSpeed;

    public enum EnemyState
    {
        idle,
        chasing,
        attacking,
        die
    }
    [SerializeField] EnemyState state;
    public virtual void Start()
    {
        baseMovementSpeed = movementSpeed;
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentHealth = maxHealth;
        state = EnemyState.chasing;
        movementSpeed = Random.Range(movementSpeed * 0.8f, movementSpeed * 1.2f);

    }
    public virtual void Update()
    {
        if (!isDead)
        {
            LookToPlayer();
            playerInHitRange = Vector2.Distance(player.transform.position, transform.position) < hitRange;
        }
        switch (state)
        {
            case EnemyState.idle:

                break;
            case EnemyState.chasing:
                ChasePlayer();
                break;
            case EnemyState.attacking:
                AttackToPlayer();
                break;
            case EnemyState.die:
                isDead = true;
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
        if (!playerInHitRange)
        {
            ChangeEnemyState(EnemyState.chasing);

            return;
        }

        if (Time.time > nextAttackTime)
        {
            animator.SetTrigger("isAttacking");
            nextAttackTime = Time.time + 1f / attackRate;
            //Start attacl animation and fire projectile?
        }
        else
        {
            animator.SetTrigger("isIdle");
        }
    }
    public virtual void Die()
    {
        //Death animation?
        //Coin drop or exp reward?
        animator.SetTrigger("isDead");
        Destroy(gameObject, 3);
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
}
