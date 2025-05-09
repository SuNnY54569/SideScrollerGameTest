using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WandProjectile : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private int minDamage = 3;
    [SerializeField] private int maxDamage = 5;
    [SerializeField] private float lifetime = 2f;
    [SerializeField] private GameObject impactAnimationPrefab;
    [SerializeField] private Vector3 impactOffset;
    
    private Vector2 direction = Vector2.right;
    private int damage;
    
    private void OnEnable()
    {
        damage = Random.Range(minDamage, maxDamage + 1);
        PoolReturnUtility.ReturnAfterDelay("Projectile", gameObject, lifetime);
    }
    
    private void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }
    
    public void SetDirection(Vector2 newDirection)
    {
        direction = newDirection.normalized;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            var enemy = other.GetComponent<EnemyHealth>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);

                GameObject impact = ObjectPoolManager.Instance.Get("Impact");
                if (impact != null)
                {
                    impact.transform.position = other.transform.position + impactOffset;
                    impact.transform.rotation = Quaternion.identity;
                }
                
                ObjectPoolManager.Instance.ReturnToPool("Projectile", gameObject);
            }
        }
    }
}
