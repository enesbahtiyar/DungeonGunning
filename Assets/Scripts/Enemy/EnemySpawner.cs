using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

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

        
        float spawnX = Random.Range(playerPos.x - spawnOffset.x, playerPos.x + spawnOffset.x);
        float spawnY = Random.Range(playerPos.y - spawnOffset.y, playerPos.y + spawnOffset.y);

        if (spawnX <= 3 && spawnX >= -3 || spawnY <= 1.5f && spawnY >= -1.5f)
        {
            spawnX = spawnX * 2;
            spawnY = spawnY * 2;
        }
        
        Vector3 spawnPosition = new Vector3(spawnX,spawnY, 0f);

        // Rastgele düşman türü seç
        ObjectPooler.Pool enemyToPool = enemyPool.pools[Random.Range(0, enemyPool.pools.Count)];
        string enemyToPoolName = enemyToPool.tag;

        enemyPool.spawnFromPool(enemyToPoolName, spawnPosition, Quaternion.identity);


    }
}
    // Tüm düşmanları yazdırmak için opsiyone
