using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private int maxHealth = 5;
    [SerializeField] private int currentHealth;
    [SerializeField] protected SlimeAI slimeAI;
    
    protected bool isDying = false;
    public bool IsDying => isDying;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        if (isDying) return;
        
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        if (isDying) return;
        isDying = true;
        
        Debug.Log($"{gameObject.name} died.");

        TriggerDeathAnimation();
        DisableCollisionsAndPhysics();
        
        StartCoroutine(WaitForDeathAnimation());
    }
    
    protected void TriggerDeathAnimation()
    {
        slimeAI.animator.SetTrigger("Die");
    }
    
    protected void DisableCollisionsAndPhysics()
    {
        foreach (Collider2D col in GetComponentsInChildren<Collider2D>())
        {
            col.enabled = false;
        }

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.isKinematic = true;
        }
    }
    
    protected virtual IEnumerator WaitForDeathAnimation()
    {
        float clipLength = GetAnimationClipLength("Die");
        yield return new WaitForSeconds(clipLength);

        Destroy(gameObject);
    }
    
    protected float GetAnimationClipLength(string clipName)
    {
        foreach (var clip in slimeAI.animator.runtimeAnimatorController.animationClips)
        {
            if (clip.name == clipName)
                return clip.length;
        }
        return 0.5f;
    }
}
