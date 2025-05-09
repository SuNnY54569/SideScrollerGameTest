using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeHeadStomp : MonoBehaviour
{
    [SerializeField] private float bounceForce = 10f;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Stomp!!");
            Rigidbody2D playerRb = other.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                playerRb.velocity = new Vector2(playerRb.velocity.x, bounceForce);
            }
            
            EnemyHealth enemyHealth = GetComponentInParent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.Die();
                Debug.Log("Enemy die");
            }
        }
    }
}
