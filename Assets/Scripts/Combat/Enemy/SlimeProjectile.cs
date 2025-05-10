using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeProjectile : MonoBehaviour
{
    [Header("Projectile Settings")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float lifetime = 3f;

    [Header("Damage Settings")]
    [SerializeField] private int damage = 10;
    [SerializeField] private float knockbackDistance = 0.5f;
    [SerializeField] private float knockbackDuration = 0.2f;
    [SerializeField] private float stunDuration = 0.5f;

    private Vector2 direction;

    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
    }
    
    private void OnEnable()
    {
        PoolReturnUtility.ReturnAfterDelay("SlimeProjectile", gameObject, lifetime);
    }

    private void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        
        HandlePlayerHit(other);
        ObjectPoolManager.Instance.ReturnToPool("SlimeProjectile", gameObject);
    }
    
    private void HandlePlayerHit(Collider2D playerCollider)
    {
        PlayerHealth player = playerCollider.GetComponent<PlayerHealth>();
        if (player == null) return;

        player.TakeDamage(damage, transform.position, knockbackDistance, knockbackDuration, stunDuration);

        GameObject impact = ObjectPoolManager.Instance.Get("SlimeImpact");
        if (impact != null)
        {
            impact.transform.position = player.transform.position;
            impact.transform.rotation = Quaternion.identity;
        }
    }
}
