using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Image fillImage;
    [SerializeField] private Color normalColor = Color.green;
    [SerializeField] private Color lowColor = Color.red;
    [SerializeField] private float lowThreshold = 0.2f;
    
    private Tween shakeTween;

    private void Awake()
    {
        if (playerHealth == null)
        {
            playerHealth = FindObjectOfType<PlayerHealth>();
        }
    }

    private void Start()
    {
        if (playerHealth != null)
        {
            playerHealth.OnHealthChanged.AddListener(UpdateHealthUI);
            UpdateHealthUI(playerHealth.CurrentHP);
        }
    }

    private void UpdateHealthUI(int currentHP)
    {
        float percent = (float)currentHP / playerHealth.MaxHP;
        healthSlider.DOValue(percent, 0.2f);

        // Color Blend
        fillImage.color = Color.Lerp(lowColor, normalColor, percent);

        // Shake when low
        if (percent <= lowThreshold)
        {
            StartShake();
        }
        else
        {
            StopShake();
        }
    }
    
    private void StartShake()
    {
        if (shakeTween != null && shakeTween.IsActive()) return;

        shakeTween = healthSlider.transform.DOShakePosition(0.5f, 5f, 10, 90, false, true)
            .SetLoops(-1, LoopType.Restart);
    }

    private void StopShake()
    {
        if (shakeTween != null && shakeTween.IsActive())
        {
            shakeTween.Kill();
            healthSlider.transform.localPosition = Vector3.zero;
        }
    }
}
