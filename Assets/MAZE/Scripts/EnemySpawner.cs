using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawner Ayarları")]
    [SerializeField] private GameObject player;
    [SerializeField] private ObjectPooler enemyPool;
    [SerializeField] private TimerDisplay timerDisplay;
    [SerializeField] private OsmanHealth playerHealth; 
    [System.Serializable]
    public class EnemySpawnSetting
    {
        public string poolTag; 
        public float startTime;
        public float interval;
        [HideInInspector] public float timer;
    }
    [SerializeField] public List<EnemySpawnSetting> spawnSettings;

    private bool spawningStopped = false;

    void Update()
    {
        if (playerHealth != null && playerHealth.health <= 0)
        {
            DeactivateAllEnemies();
            return;
        }

        if (timerDisplay != null && timerDisplay.IsMaxTimeReached())
        {
            if (!spawningStopped)
            {
                Debug.Log("Maksimum süreye ulaşıldı, üretim durdu.");
                spawningStopped = true;
            }
            return;
        }

        if (player == null || enemyPool.pools.Count == 0) return;

        foreach (var setting in spawnSettings)
        {
            setting.timer += Time.deltaTime;
            if (timerDisplay != null && timerDisplay.GetElapsedTime() >= setting.startTime)
            {
                if (setting.timer >= setting.interval)
                {
                    SpawnEnemy(setting.poolTag);
                    setting.timer = 0f;
                }
            }
        }
    }

    void SpawnEnemy(string poolTag)
    {
        Vector3 playerPos = player.transform.position;
        float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
        float distance = Random.Range(8f, 20f);
        
        float spawnX = playerPos.x + Mathf.Cos(angle) * distance;
        float spawnY = playerPos.y + Mathf.Sin(angle) * distance;
        
        Vector3 spawnPosition = new Vector3(spawnX, spawnY, 0f);
        enemyPool.spawnFromPool(poolTag, spawnPosition, Quaternion.identity);
    }

    void DeactivateAllEnemies()
    {
        foreach (var poolQueue in enemyPool.poolDictionary.Values)
        {
            foreach (var enemy in poolQueue)
            {
                if (enemy.activeInHierarchy)
                {
                    enemy.SetActive(false);
                }
            }
        }
    }
}
