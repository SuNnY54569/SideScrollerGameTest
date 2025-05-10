using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigSlimeHealth : EnemyHealth
{
    [Header("Split Settings")]
    [SerializeField] private GameObject smallSlimePrefab;
    [SerializeField] private int numberOfSplits = 2;
    [SerializeField] private float splitOffset = 0.5f;
    
    public override void Die()
    {
        if (isDying) return;
        isDying = true;

        TriggerDeathAnimation();
        DisableCollisionsAndPhysics();

        StartCoroutine(WaitForDeathAnimation());
    }
    
    protected override IEnumerator WaitForDeathAnimation()
    {
        float clipLength = GetAnimationClipLength("Die");
        yield return new WaitForSeconds(clipLength);

        SplitIntoSmallSlimes();

        Destroy(gameObject);
    }
    
    private void SplitIntoSmallSlimes()
    {
        for (int i = 0; i < numberOfSplits; i++)
        {
            Vector3 spawnOffset = new Vector3((i - (numberOfSplits - 1) / 2f) * splitOffset, 0f, 0f);
            GameObject smallSlime = Instantiate(smallSlimePrefab, transform.position + spawnOffset, Quaternion.identity);

            SlimeAI spawnedSlimeAI = smallSlime.GetComponent<SlimeAI>();
            if (spawnedSlimeAI != null)
            {
                spawnedSlimeAI.SetBoundary(slimeAI.GetBoundary(false), slimeAI.GetBoundary(true));
            }
        }
    }
}
