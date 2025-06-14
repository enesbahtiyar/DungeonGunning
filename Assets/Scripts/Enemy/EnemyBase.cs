using System.Collections;
using UnityEngine;
[RequireComponent(typeof(Health))]
public class EnemyBase : MonoBehaviour
{
    [SerializeField] private float movementSpeed;
    [SerializeField] private float hitRange;
    [SerializeField] private float maxHealth;
     private float currentHealth;
    [SerializeField, Tooltip("Attack Per Second")] private float attackRate;
    private float nextAttackTime;
    public GameObject player;
    
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
        currentHealth=maxHealth;
        state = EnemyState.chasing;
        movementSpeed = Random.Range(movementSpeed * 0.8f, movementSpeed * 1.2f);

    }
    public virtual void Update()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            TakeDamage(Random.Range(1,10));
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
                Die();
                break;

        }
    }
    public virtual void ChasePlayer()
    {
        if (Vector2.Distance(player.transform.position, transform.position) > hitRange)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, movementSpeed * Time.deltaTime);
        }
        else
        {
            ChangeEnemyState(EnemyState.attacking);
        }
    }
    public virtual void AttackToPlayer()
    {
        if (Vector2.Distance(player.transform.position, transform.position) > hitRange)
        {
            ChangeEnemyState(EnemyState.chasing);
            return;
        }

        if (Time.time > nextAttackTime)
        {
            Debug.Log("Attack");
            nextAttackTime = Time.time + 1f / attackRate;
            //Start attacl animation and fire projectile?
        }
    }
    public virtual void Die()
    {
        //Death animation?
        //Coin drop or exp reward?
        Debug.Log(gameObject.name + " died");
        Destroy(gameObject);
    }
    public virtual void ChangeEnemyState(EnemyState state)
    {
        this.state = state;
    }
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if(currentHealth<= 0)
        {
            ChangeEnemyState(EnemyState.die);
        }
    }
}
