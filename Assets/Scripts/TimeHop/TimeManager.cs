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
    
    [Header("Events")]
    public UnityEvent<TimeOfDay> OnTimeChanged;
    public UnityEvent<DayOfWeek, int> OnDayChanged;
    
    private float timer;
    
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
        Debug.Log($"Time changed to: {CurrentTime}, Day: {CurrentDay} (Total Days: {DayCount + 1})");
    }
}
