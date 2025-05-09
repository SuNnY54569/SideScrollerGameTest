using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WandProjectile : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private int minDamage = 3;
    [SerializeField] private int maxDamage = 5;
    [SerializeField] private float lifetime = 2f;
    
    private Vector2 direction = Vector2.right;
    private int damage;
    
    private void Start()
    {
        damage = Random.Range(minDamage, maxDamage + 1);
        Destroy(gameObject, lifetime);
    }
    
    private void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }
    
    public void SetDirection(Vector2 newDirection)
    {
        direction = newDirection.normalized;
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        /*var enemy = collision.GetComponent<EnemyHealth>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
            Destroy(gameObject);
        }*/
    }
}
