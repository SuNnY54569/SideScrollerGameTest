using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigSlimeHealth : EnemyHealth
{
    [SerializeField] private GameObject smallSlimePrefab;
    [SerializeField] private int numberOfSplits = 2;
    [SerializeField] private float splitOffset = 0.5f;
    
    public override void Die()
    {
        if (isDying) return;
        isDying = true;
        
        slimeAI.animator.SetTrigger("Die");
        
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

        StartCoroutine(WaitForDeathAnimation());
    }
    
    private IEnumerator WaitForDeathAnimation()
    {
        float clipLength = GetAnimationClipLength("Die");

        yield return new WaitForSeconds(clipLength);

        for (int i = 0; i < numberOfSplits; i++)
        {
            Vector3 spawnOffset = new Vector3((i - (numberOfSplits - 1) / 2f) * splitOffset, 0f, 0f);
            GameObject smallSlime = Instantiate(smallSlimePrefab, transform.position + spawnOffset, Quaternion.identity);
            SlimeAI spawnedSlimeAI = smallSlime.GetComponent<SlimeAI>();
            slimeAI.SetBoundary(spawnedSlimeAI.GetBoundary(false),spawnedSlimeAI.GetBoundary(true));
        }

        Destroy(gameObject);
    }
    
    private float GetAnimationClipLength(string clipName)
    {
        foreach (var clip in slimeAI.animator.runtimeAnimatorController.animationClips)
        {
            if (clip.name == clipName)
                return clip.length;
        }
        return 0.5f; // Default fallback
    }
}
