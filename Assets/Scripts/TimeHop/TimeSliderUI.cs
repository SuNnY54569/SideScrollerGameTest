using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeSliderUI : MonoBehaviour
{
    [SerializeField] private Slider morningSlider;
    [SerializeField] private Slider afternoonSlider;
    [SerializeField] private Slider eveningSlider;
    
    private TimeManager timeManager;
    
    private void Awake()
    {
        timeManager = FindObjectOfType<TimeManager>();
    }
    
    private void Start()
    {
        if (timeManager == null) return;

        timeManager.OnTimeChanged.AddListener(OnTimeChanged);
        timeManager.OnDayChanged.AddListener(OnDayChanged);
        ResetSliders();
    }
    
    private void Update()
    {
        if (timeManager == null) return;

        float progress = timeManager.CurrentTimeProgress;

        switch (timeManager.CurrentTime)
        {
            case TimeManager.TimeOfDay.Morning:
                morningSlider.value = progress;
                break;
            case TimeManager.TimeOfDay.Afternoon:
                afternoonSlider.value = progress;
                break;
            case TimeManager.TimeOfDay.Evening:
                eveningSlider.value = progress;
                break;
        }
    }
    
    private void OnTimeChanged(TimeManager.TimeOfDay _)
    {
        morningSlider.value = timeManager.CurrentTime > TimeManager.TimeOfDay.Morning ? 1f : morningSlider.value;
        afternoonSlider.value = timeManager.CurrentTime > TimeManager.TimeOfDay.Afternoon ? 1f : afternoonSlider.value;
    }
    
    private void OnDayChanged(TimeManager.DayOfWeek _, int __)
    {
        ResetSliders();
    }
    
    private void ResetSliders()
    {
        morningSlider.value = 0f;
        afternoonSlider.value = 0f;
        eveningSlider.value = 0f;
    }
}
