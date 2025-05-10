using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private int maxHP = 100;
    [SerializeField] private int currentHP;
    [SerializeField] private Color hitColor = Color.red;
    
    [Header("Stun Settings")]
    [SerializeField] private GameObject stunEffectPrefab;
    [SerializeField] private Transform stunEffectPos;
    
    private bool isStunned = false;
    
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private Color originalColor;
    private Tween flashTween;
    private Tween knockbackTween;
    private GameObject stunEffectInstance;
    
    public int MaxHP => maxHP;
    public int CurrentHP => currentHP;
    public bool IsStunned => isStunned;
    
    public UnityEvent<int> OnHealthChanged;
    
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        originalColor = spriteRenderer.color;
        currentHP = maxHP;
        OnHealthChanged?.Invoke(currentHP);
    }
    
    public void TakeDamage(int amount, Vector2 hitSource, float  knockbackDistance, float  knockbackDuration, float stunDuration)
    {
        currentHP -= amount;
        currentHP = Mathf.Max(0, currentHP);
        OnHealthChanged?.Invoke(currentHP);
        
        PlayHitFlash();

        if (currentHP <= 0)
        {
            Debug.Log("Player died");
        }
        
        if (isStunned) return;
        isStunned = true;
        
        ApplyKnockback(hitSource, knockbackDistance,  knockbackDuration, stunDuration);
        
    }
    
    private void ApplyKnockback(Vector2 hitSource, float distance, float duration, float stunDuration)
    {
        rb.velocity = Vector2.zero;

        float directionX = transform.position.x < hitSource.x ? -1f : 1f;
        Vector3 targetPosition = transform.position + new Vector3(directionX * distance, 0f, 0f);

        knockbackTween?.Kill();
        knockbackTween = transform.DOMoveX(targetPosition.x, duration)
            .SetEase(Ease.OutQuad)
            .OnComplete(() => StartCoroutine(StunTimer(stunDuration)));
    }
    
    private void PlayHitFlash()
    {
        flashTween?.Kill();
        flashTween = spriteRenderer.DOColor(hitColor, 0.1f)
            .SetLoops(4, LoopType.Yoyo)
            .OnComplete(() => spriteRenderer.color = originalColor);
    }
    
    private IEnumerator StunTimer(float stunDuration)
    {
        
        if (stunEffectPrefab != null)
        {
            stunEffectInstance = Instantiate(stunEffectPrefab, stunEffectPos.position, Quaternion.identity, transform);
        }
        
        yield return new WaitForSeconds(stunDuration);
        
        if (stunEffectInstance != null)
        {
            Destroy(stunEffectInstance);
        }

        isStunned = false;
    }
    
    public void ResetHealth()
    {
        currentHP = maxHP;
        OnHealthChanged?.Invoke(currentHP);
    }
}
