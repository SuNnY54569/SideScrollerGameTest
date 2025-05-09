using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class TimeHopTrigger : MonoBehaviour
{
    [Header("Float Animation Settings")]
    [SerializeField] private float moveDistance = 0.5f;
    [SerializeField] private float moveDuration = 1f;
    
    private void Start()
    {
        // Ping-pong vertical movement
        transform.DOMoveY(transform.position.y + moveDistance, moveDuration)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutSine);
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            TimeManager timeManager = FindObjectOfType<TimeManager>();
            if (timeManager != null)
            {
                timeManager.AdvanceTime();
            }
        }
    }
}
