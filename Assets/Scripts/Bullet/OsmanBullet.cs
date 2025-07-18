using UnityEngine;

public class OsmanBullet : MonoBehaviour
{
    [Header("Bullet Settings")]
    public float speed = 10f;
    public float lifetime = 3f;
    public LayerMask targetLayer;

    private float damage;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, lifetime);
    }

    public void Fire(Vector2 direction, float _damage)
    {
        damage = _damage;
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = direction * speed;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // targetLayer kontrolü eklendi
        //if (((1 << other.gameObject.layer) & targetLayer.value) == 0)
        //    return;

        //OsmanHealth health = other.GetComponent<OsmanHealth>();

        //if (health != null && health.entityType == EntityType.AI)
        //{
        //    health.DecreaseHealth(damage);
        //    Destroy(gameObject);
        //}
        if (other.gameObject.TryGetComponent<OsmanHealth>(out OsmanHealth health) && (targetLayer.value & (1 << other.gameObject.layer)) != 0)
        {
            health.DecreaseHealth(damage);
            Destroy(gameObject);
        }
    }
}