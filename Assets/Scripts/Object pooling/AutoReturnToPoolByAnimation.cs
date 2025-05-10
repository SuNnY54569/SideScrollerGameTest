using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoReturnToPoolByAnimation : MonoBehaviour
{
    [SerializeField] private string poolKey;
    
    private void OnEnable()
    {
        Animator animator = GetComponent<Animator>();
        if (animator != null && animator.runtimeAnimatorController != null)
        {
            AnimationClip clip = animator.runtimeAnimatorController.animationClips[0];
            float clipLength = clip.length;
            Invoke(nameof(ReturnToPool), clipLength);
        }
        else
        {
            Debug.LogWarning($"{gameObject.name} has no Animator or Clip. Using fallback.");
            Invoke(nameof(ReturnToPool), 1f);
        }
    }

    private void ReturnToPool()
    {
        ObjectPoolManager.Instance.ReturnToPool(poolKey, gameObject);
    }
}
