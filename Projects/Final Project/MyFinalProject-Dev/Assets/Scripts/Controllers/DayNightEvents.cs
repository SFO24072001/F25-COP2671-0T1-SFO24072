using UnityEngine;
using UnityEngine.Events;

public class DayNightEvents : MonoBehaviour
{
    private const int MIDNIGHT = 0;

    [Header("Event Hours")]
    [SerializeField] private int _sunRiseHour = 6;    
    [SerializeField] private int _sunSetHour = 18;

    [Header("Day-Night Events")]
    public UnityEvent OnSunrise;
    public UnityEvent OnSunset;
    public UnityEvent<int> OnHour;
    public UnityEvent OnNewDay;

    [field: SerializeField] public int CurrentHour { get; private set; }

    public void Evaluate(float normalizedTime)
    {
        // Calculate the current hour based on normalized time
        int calculatedHour = Mathf.FloorToInt(normalizedTime * TimeManager.HOURSPERDAY) % TimeManager.HOURSPERDAY;

        if (calculatedHour == CurrentHour) return;

        TriggerMidnightEvent(calculatedHour);
        TriggerSunriseEvent(calculatedHour);
        TriggerSunset(calculatedHour);
        TriggerHourlyEvent(calculatedHour);

        CurrentHour = calculatedHour;
    }
    public void ResetHourTracking()
    {
        CurrentHour = -1;
    }
    private void TriggerSunset(int hour)
    {
        if (hour == _sunSetHour)
            OnSunset?.Invoke();
    }
    private void TriggerSunriseEvent(int hour)
    {
        if (hour == _sunRiseHour)
            OnSunrise?.Invoke();
    }
    private void TriggerMidnightEvent(int hour)
    {
        if (hour == MIDNIGHT)
            OnNewDay?.Invoke();
    }
    private void TriggerHourlyEvent(int hour)
    {
        OnHour?.Invoke(hour);
    }
}
