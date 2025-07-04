using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    
    [Header("Spawner Ayarları")]
    [SerializeField] private GameObject player;
    [SerializeField] private float spawnInterval = 2f;
    [SerializeField] private Vector2 spawnOffset = new Vector2(10f, 5f);
    [SerializeField] private ObjectPooler enemyPool;
    private float timer;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnEnemy();
            timer = 0f;
        }
    }

    void SpawnEnemy()
    {
        if (player == null || enemyPool.pools.Count == 0) return;

        Vector3 playerPos = player.transform.position;

        Vector3 spawnPosition = new Vector3(
            Random.Range(playerPos.x - spawnOffset.x, playerPos.x + spawnOffset.x),
            Random.Range(playerPos.y - spawnOffset.y, playerPos.y + spawnOffset.y),
            0f // 2D için Z sabit
        );

        // Rastgele düşman türü seç
        ObjectPooler.Pool enemyToPool = enemyPool.pools[Random.Range(0, enemyPool.pools.Count)];
        string enemyToPoolName = enemyToPool.tag;

        enemyPool.spawnFromPool(enemyToPoolName, spawnPosition, Quaternion.identity);


    }
}
    // Tüm düşmanları yazdırmak için opsiyone
