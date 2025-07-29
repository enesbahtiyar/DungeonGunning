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
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
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


    public GameObject spawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("Böyle bir pool yok " + tag);
            return null;
        }
        else
        {
            GameObject objectToPool = poolDictionary[tag].Dequeue();
            objectToPool.SetActive(true);
            objectToPool.transform.position = position;
            objectToPool.transform.rotation = rotation;
            poolDictionary[tag].Enqueue(objectToPool);
            return objectToPool;

        }
    }
}