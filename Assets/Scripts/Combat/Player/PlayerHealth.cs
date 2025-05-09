using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHP = 100;
    [SerializeField] private int currentHP;
    [SerializeField] private Color hitColor = Color.red;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private Rigidbody2D rb;
    private Tween flashTween;
    
    public int MaxHP => maxHP;
    public int CurrentHP => currentHP;
    
    public UnityEvent<int> OnHealthChanged;
    
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
        rb = GetComponent<Rigidbody2D>();
        currentHP = maxHP;
        OnHealthChanged?.Invoke(currentHP);
    }
    
    public void TakeDamage(int amount, Vector2 hitSource, float knockbackForce)
    {
        currentHP -= amount;
        currentHP = Mathf.Max(0, currentHP);
        OnHealthChanged?.Invoke(currentHP);
        
        Vector2 playerPos = new Vector2(transform.position.x, transform.position.y);
        Vector2 hitDir = (playerPos - hitSource).normalized;
        
        float angleInDegrees = 45f;
        Vector2 rotatedDir = Quaternion.Euler(0, 0, angleInDegrees) * hitDir;

        rb.AddForce(rotatedDir * knockbackForce, ForceMode2D.Impulse);
        PlayHitFlash();

        if (currentHP <= 0)
        {
            Debug.Log("Player died");
        }
    }
    
    private void PlayHitFlash()
    {
        if (flashTween != null && flashTween.IsActive())
            flashTween.Kill();

        flashTween = spriteRenderer.DOColor(hitColor, 0.1f)
            .SetLoops(4, LoopType.Yoyo)
            .OnComplete(() => spriteRenderer.color = originalColor);
    }
    
    public void ResetHealth()
    {
        currentHP = maxHP;
        OnHealthChanged?.Invoke(currentHP);
    }
}
