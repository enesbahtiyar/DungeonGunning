using UnityEngine;

public class KnifesOsman : MonoBehaviour
{
    public Transform player;
    public float rotationSpeed = 50f;
    public float damage = 10f;
    public LayerMask enemyLayer;

    void Start()
    {
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
            }
        }
        
        foreach (Transform child in transform)
        {
            if (!child.GetComponent<KnifeAttack>())
            {
                KnifeAttack attack = child.gameObject.AddComponent<KnifeAttack>();
                attack.damage = damage;
                attack.enemyLayer = enemyLayer;
            }
        }
    }

    void Update()
    {
        if (player != null)
        {
            transform.position = player.position;
            transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
        }
    }
}

public class KnifeAttack : MonoBehaviour
{
    public float damage = 10f;
    public LayerMask enemyLayer;
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & enemyLayer) != 0)
        {
            OsmanHealth enemyHealth = other.GetComponent<OsmanHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.DecreaseHealth(damage);
            }
        }
    }
}
