using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolReturnUtility : MonoBehaviour
{
    private static PoolReturnUtility instance;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }
    
    public static void ReturnAfterDelay(string key, GameObject obj, float delay)
    {
        if (instance == null)
        {
            Debug.LogWarning("No PoolReturnUtility instance found in the scene.");
            return;
        }

        instance.StartCoroutine(instance.ReturnCoroutine(key, obj, delay));
    }

    private IEnumerator ReturnCoroutine(string key, GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (ObjectPoolManager.Instance != null)
        {
            ObjectPoolManager.Instance.ReturnToPool(key, obj);
        }
        else
        {
            Debug.LogWarning("ObjectPoolManager instance not found when trying to return object.");
            Destroy(obj);
        }
    }
}
