using UnityEngine;

public class OsmanBullet : MonoBehaviour
{
    [Header("Bullet Settings")]
    public float speed = 10f;
    public float lifetime = 3f;
    public LayerMask targetLayer;
    public bool canRicochet = false;
    public int maxRicochetCount = 1;

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
        // Hedef katman değilse hiçbir şey yapma
        if ((targetLayer.value & (1 << other.gameObject.layer)) == 0)
            return;

        // Düşmana çarptıysa hasar ver
        if (other.gameObject.TryGetComponent<OsmanHealth>(out OsmanHealth health))
        {
            health.DecreaseHealth(damage);

            // Sekme yapılacak mı?
            if (canRicochet && maxRicochetCount > 0)
            {
                GameObject nearestEnemy = FindNearestEnemyExcluding(other.gameObject);

                if (nearestEnemy != null)
                {
                    Vector2 directionToEnemy = (nearestEnemy.transform.position - transform.position).normalized;
                    rb.linearVelocity = directionToEnemy * speed;

                    float angle = Mathf.Atan2(directionToEnemy.y, directionToEnemy.x) * Mathf.Rad2Deg;
                    transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

                    maxRicochetCount--;
                    return;
                }
            }

            Destroy(gameObject);
        }
    }
    private GameObject FindNearestEnemyExcluding(GameObject exclude)
    {
        float closestDistance = float.MaxValue;
        GameObject nearestEnemy = null;

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 10f, targetLayer);

        foreach (var hit in hits)
        {
            if (hit.gameObject == exclude)
                continue;

            if (hit.TryGetComponent<OsmanHealth>(out OsmanHealth health))
            {
                float distance = Vector2.Distance(transform.position, hit.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    nearestEnemy = hit.gameObject;
                }
            }
        }

        return nearestEnemy;
    }

}