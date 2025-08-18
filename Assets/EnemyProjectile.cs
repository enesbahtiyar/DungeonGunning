using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public int damage = 10;
    public float lifetime = 5f;
    public float speed = 5f;
    private Vector2 direction;

    public void Initialize(Vector2 dir, float spd, int dmg, float life)
    {
        direction = dir;
        speed = spd;
        damage = dmg;
        lifetime = life;
        GetComponent<Rigidbody2D>().linearVelocity = direction * speed;
        
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        
        Destroy(gameObject, lifetime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            OsmanHealth osmanHealth = other.GetComponent<OsmanHealth>();
            if (osmanHealth != null)
            {
                osmanHealth.DecreaseHealth(damage);
            }
            Destroy(gameObject);
        }
    }
}