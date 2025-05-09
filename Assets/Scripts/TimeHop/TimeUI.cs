using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimeUI : MonoBehaviour
{
    [SerializeField] private TMP_Text timeText;
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
        
        UpdateText();
    }

    private void OnTimeChanged(TimeManager.TimeOfDay _) => UpdateText();
    private void OnDayChanged(TimeManager.DayOfWeek _, int __) => UpdateText();

    private void UpdateText()
    {
        timeText.text = $"Time: {timeManager.CurrentTime}\n" +
                        $"{timeManager.CurrentDay} (Day {timeManager.DayCount + 1})";
    }
}
