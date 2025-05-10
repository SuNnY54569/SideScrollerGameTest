using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class TimeManager : MonoBehaviour
{
    public enum TimeOfDay { Morning, Afternoon, Evening }
    public enum DayOfWeek { Mon, Tue, Wed, Thu, Fri, Sat, Sun }
    
    public TimeOfDay CurrentTime { get; private set; } = TimeOfDay.Morning;
    public int DayCount { get; private set; } = 0;
    public DayOfWeek CurrentDay => (DayOfWeek)(DayCount % 7);
    
    public float CurrentTimeProgress { get; private set; }
    
    [Header("Settings")]
    [SerializeField] private float timeStepDuration = 10f;
    [SerializeField] private PlayerHealth playerHealth;
    
    [Header("Period Colors")]
    [SerializeField] private Color morningColor = Color.yellow;
    [SerializeField] private Color afternoonColor = Color.cyan;
    [SerializeField] private Color eveningColor = Color.blue;
    private Tween colorTween;
    
    [Header("Target Background")]
    [SerializeField] private SpriteRenderer backgroundSprite;
    
    [Header("Events")]
    public UnityEvent<TimeOfDay> OnTimeChanged;
    public UnityEvent<DayOfWeek, int> OnDayChanged;
    
    private float timer;

    private void Awake()
    {
        if (playerHealth == null)
        {
            playerHealth = FindObjectOfType<PlayerHealth>();
        }
    }
    
    private void Start()
    {
        // Set initial color
        if (backgroundSprite != null)
            backgroundSprite.color = GetPeriodColor(CurrentTime);

        StartBlendingToNextPeriod();
    }

    private void Update()
    {
        timer += Time.deltaTime;
        CurrentTimeProgress = timer / timeStepDuration;

        if (timer >= timeStepDuration)
        {
            AdvanceTime();
            timer = 0f;
            CurrentTimeProgress = 0f;
        }
    }
    
    public void AdvanceTime()
    {
        if (CurrentTime == TimeOfDay.Evening)
        {
            DayCount++;
            CurrentTime = TimeOfDay.Morning;
            OnDayChanged?.Invoke(CurrentDay, DayCount);
        }
        else
        {
            CurrentTime++;
        }
        
        CurrentTimeProgress = 0f;
        timer = 0f;
        OnTimeChanged?.Invoke(CurrentTime);
        
        if (playerHealth != null)
        {
            playerHealth.RegenerateHealth();
        }
        
        StartBlendingToNextPeriod();
        Debug.Log($"Time changed to: {CurrentTime}, Day: {CurrentDay} (Total Days: {DayCount + 1})");
    }
    
    private void StartBlendingToNextPeriod()
    {
        if (backgroundSprite == null) return;

        Color fromColor = GetPeriodColor(CurrentTime);
        Color toColor = GetPeriodColor(GetNextPeriod(CurrentTime));

        backgroundSprite.color = fromColor; // Ensure correct start color

        colorTween?.Kill();
        colorTween = DOTween.To(
            () => backgroundSprite.color,
            x => backgroundSprite.color = x,
            toColor,
            timeStepDuration
        ).SetEase(Ease.Linear);
    }
    
    private Color GetPeriodColor(TimeOfDay period)
    {
        switch (period)
        {
            case TimeOfDay.Morning:
                return morningColor;
            case TimeOfDay.Afternoon:
                return afternoonColor;
            case TimeOfDay.Evening:
                return eveningColor;
            default:
                return Color.white;
        }
    }
    private TimeOfDay GetNextPeriod(TimeOfDay current)
    {
        switch (current)
        {
            case TimeOfDay.Morning:
                return TimeOfDay.Afternoon;
            case TimeOfDay.Afternoon:
                return TimeOfDay.Evening;
            case TimeOfDay.Evening:
            default:
                return TimeOfDay.Morning;
        }
    }
}
