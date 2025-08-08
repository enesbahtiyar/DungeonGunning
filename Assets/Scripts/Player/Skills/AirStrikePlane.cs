using System.Collections;
using UnityEngine;

public class AirStrikePlane : MonoBehaviour
{
    private Vector3 targetPos;
    private float moveSpeed;
    private bool isFlying = false;
    private Transform marker;

    [SerializeField] private GameObject explosionEffect;
    [SerializeField] private float explosionRadius = 2f;
    [SerializeField, Tooltip("Distance to target before drop the bomb")] private float bombDropRange = 3f;
    [SerializeField] private float damage = 3f;

    [SerializeField] private int numberOfBombs = 3;
    [SerializeField] private float bombDropInterval = 0.2f;

    private bool isDroppingBombs = false;

    public void FlyTo(Vector3 target, float speed, Transform markerPos)
    {
        targetPos = target;
        moveSpeed = speed;
        isFlying = true;
        marker = markerPos;
    }

    void Update()
    {
        if (!isFlying) return;
        transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
        if (!isDroppingBombs && Mathf.Abs(transform.position.x-marker.position.x) < bombDropRange)
        {
            isDroppingBombs = true;
            StartCoroutine(DropBombs());
        }
        if (Vector3.Distance(transform.position, targetPos) < 0.1f)
        {
            Destroy(gameObject);
        }

    }
    private IEnumerator DropBombs()
    {
        for (int i = 0; i < numberOfBombs; i++)
        {
            if (explosionEffect != null)
            {
               GameObject bomb= Instantiate(explosionEffect, transform.position, Quaternion.Euler(0, 0, 90));
                Destroy(bomb, 1f); 
            }

            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.TryGetComponent<OsmanHealth>(out OsmanHealth health))
                {
                    if(health.entityType != EntityType.AI) continue; 
                    health.DecreaseHealth(damage);
                }
            }

            yield return new WaitForSeconds(Random.Range(bombDropInterval+0.2f,bombDropInterval-0.2f));
        }
    }
#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0, 0, 0.4f); // Yarý þeffaf kýrmýzý
        Gizmos.DrawWireSphere(transform.position, explosionRadius); // Daire çiz
    }
#endif
}
