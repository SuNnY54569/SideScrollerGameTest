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
            Destroy(gameObject);
        
        
        foreach (var pool in poolsConfig)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.initialSize; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(pool.key, objectPool);
            prefabDictionary.Add(pool.key, pool.prefab);
        }
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
            GameObject obj = Instantiate(prefabDictionary[key]);
            obj.SetActive(false);
            poolDictionary[key].Enqueue(obj);
        }

        GameObject pooledObj = poolDictionary[key].Dequeue();
        pooledObj.SetActive(true);
        return pooledObj;
    }
    
    public void ReturnToPool(string key, GameObject obj)
    {
        obj.SetActive(false);
        poolDictionary[key].Enqueue(obj);
    }
}
