using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public string key;
        public GameObject prefab;
        public int initialSize = 10;
    }
    
    public static ObjectPoolManager Instance { get; private set; }

    [SerializeField] private List<Pool> poolsConfig = new List<Pool>();
    
    private Dictionary<string, Queue<GameObject>> poolDictionary = new Dictionary<string, Queue<GameObject>>();
    private Dictionary<string, GameObject> prefabDictionary = new Dictionary<string, GameObject>();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        
        InitializePools();
    }
    
    private void InitializePools()
    {
        foreach (var pool in poolsConfig)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.initialSize; i++)
            {
                GameObject obj = CreatePooledObject(pool.prefab);
                objectPool.Enqueue(obj);
            }

            poolDictionary[pool.key] = objectPool;
            prefabDictionary[pool.key] = pool.prefab;
        }
    }
    
    private GameObject CreatePooledObject(GameObject prefab)
    {
        GameObject obj = Instantiate(prefab);
        obj.SetActive(false);
        return obj;
    }

    
    public GameObject Get(string key)
    {
        if (!poolDictionary.ContainsKey(key))
        {
            Debug.LogWarning("Pool with key " + key + " doesn't exist.");
            return null;
        }

        if (poolDictionary[key].Count == 0)
        {
            GameObject newObj = CreatePooledObject(prefabDictionary[key]);
            poolDictionary[key].Enqueue(newObj);
        }

        GameObject pooledObj = poolDictionary[key].Dequeue();
        pooledObj.SetActive(true);
        return pooledObj;
    }
    
    public void ReturnToPool(string key, GameObject obj)
    {
        if (!poolDictionary.ContainsKey(key))
        {
            Debug.LogWarning($"Trying to return object to non-existent pool with key '{key}'.");
            Destroy(obj);
            return;
        }

        obj.SetActive(false);
        poolDictionary[key].Enqueue(obj);
    }
}
