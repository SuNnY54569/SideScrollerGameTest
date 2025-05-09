using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ArcanePowerUI : MonoBehaviour
{
    [SerializeField] private Slider apSlider;
    [SerializeField] private ArcanePower arcanePower;
    [SerializeField] private Image fillImage;
    [SerializeField] private Color normalColor = Color.yellow;
    [SerializeField] private Color lowColor = Color.red;
    [SerializeField] private float lowThreshold = 0.2f;

    private Tween pulseTween;

    private void Awake()
    {
        if (arcanePower == null)
        {
            arcanePower = FindObjectOfType<ArcanePower>();
        }
    }

    private void Start()
    {
        if (arcanePower != null)
        {
            arcanePower.OnAPChanged.AddListener(UpdateAPBar);
            UpdateAPBar(arcanePower.GetCurrentAP());
        }
    }

    private void UpdateAPBar(int newAP)
    {
        float percent = (float)newAP / arcanePower.MaxAP;
        apSlider.DOValue(percent, 0.2f);
        
        fillImage.color = Color.Lerp(lowColor, normalColor, percent);

        if (percent <= lowThreshold)
        {
            StartPulse();
        }
        else
        {
            StopPulse();
        }
    }

    private void StartPulse()
    {
        if (pulseTween != null && pulseTween.IsActive()) return; 

        pulseTween = apSlider.transform.DOScale(1.05f, 0.5f)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutSine);
    }
    
    private void StopPulse()
    {
        if (pulseTween != null && pulseTween.IsActive())
        {
            pulseTween.Kill();
            apSlider.transform.localScale = Vector3.one;
        }
    }
}
