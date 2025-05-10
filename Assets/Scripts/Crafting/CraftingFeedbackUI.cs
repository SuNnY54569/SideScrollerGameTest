using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class CraftingFeedbackUI : MonoBehaviour
{
    [SerializeField] private TMP_Text feedbackText;
    [SerializeField] private CanvasGroup canvasGroup;
    private Tween fadeTween;

    public void ShowFeedback(string message)
    {
        feedbackText.text = message;
        
        fadeTween?.Kill();
        canvasGroup.alpha = 1;
        fadeTween = canvasGroup
            .DOFade(0f, 1f)
            .SetDelay(1f)
            .SetUpdate(true); 
    }
}
