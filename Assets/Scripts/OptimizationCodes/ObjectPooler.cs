using System;
using UnityEngine;
using System.Collections.Generic;

public class ObjectPooler : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }
    
    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDictionary;
    public GameObject player;
    public Transform parentTransform;

    private void Start()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (Pool pool in pools)
        {
            Queue<GameObject>objectPool = new Queue<GameObject>();
            
            for(int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab, parentTransform);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
                
                if (obj.TryGetComponent(out EnemyBase baseScript))
                {
                    baseScript.player = player;
                }
            }


            poolDictionary.Add(pool.tag, objectPool); 
        }
    }


    // Yeni: Index ile pooldan spawn et
    public GameObject spawnFromPoolByIndex(int poolIndex, Vector3 position, Quaternion rotation)
    {
        if (poolIndex < 0 || poolIndex >= pools.Count)
        {
            Debug.LogWarning("Pool index geçersiz: " + poolIndex);
            return null;
        }

        string tag = pools[poolIndex].tag;
        return spawnFromPool(tag, position, rotation);
    }

    public GameObject spawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("Böyle bir pool yok " + tag);
            return null;
        }
        
        Queue<GameObject> pool = poolDictionary[tag];
        int poolSize = pool.Count;
        
        for (int i = 0; i < poolSize; i++)
        {
            GameObject objectToPool = pool.Dequeue();
            pool.Enqueue(objectToPool);
            
            if (!objectToPool.activeInHierarchy)
            {
                objectToPool.SetActive(true);
                objectToPool.transform.position = position;
                objectToPool.transform.rotation = rotation;
                return objectToPool;
            }
        }
        
        return null; // Tüm objeler aktif
    }
}
