using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [System.Serializable]
    public struct EnemyTypeData
    {
        public string typeName;      // Örn: "Zombie", "Robot", "Boss"
        public GameObject prefab;    // Prefab referansı
    }

    public struct EnemyInfo
    {
        public string name;
        public string type;
        public Vector3 position;

        public EnemyInfo(string name, string type, Vector3 position)
        {
            this.name = name;
            this.type = type;
            this.position = position;
        }
    }

    [Header("Spawner Ayarları")]
    [SerializeField] private GameObject player;
    [SerializeField] private float spawnInterval = 2f;
    [SerializeField] private Vector2 spawnOffset = new Vector2(10f, 5f);
    [SerializeField] private EnemyTypeData[] enemyTypes;

    private float timer;
    private List<EnemyInfo> spawnedEnemies = new List<EnemyInfo>();

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
        if (player == null || enemyTypes.Length == 0) return;

        Vector3 playerPos = player.transform.position;

        Vector3 spawnPosition = new Vector3(
            Random.Range(playerPos.x - spawnOffset.x, playerPos.x + spawnOffset.x),
            Random.Range(playerPos.y - spawnOffset.y, playerPos.y + spawnOffset.y),
            0f // 2D için Z sabit
        );

        // Rastgele düşman türü seç
        EnemyTypeData chosen = enemyTypes[Random.Range(0, enemyTypes.Length)];

        GameObject newEnemy = Instantiate(chosen.prefab, spawnPosition, Quaternion.identity);

        // Düşmanın varsa EnemyBase gibi bir script'i, oraya player'ı gönder
        if (newEnemy.TryGetComponent(out EnemyBase baseScript))
        {
            baseScript.player = player;
        }

        // Bilgiyi listeye ekle
        EnemyInfo info = new EnemyInfo(newEnemy.name, chosen.typeName, newEnemy.transform.position);
        spawnedEnemies.Add(info);

        Debug.Log($"[Spawned] {info.type}: {info.name} at {info.position}");
    }
}
    // Tüm düşmanları yazdırmak için opsiyone
