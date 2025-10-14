using System.Collections;
using UnityEngine;

public class TimeManager : SingletonMonoBehaviour<TimeManager>
{
    public const int HOURSPERDAY = 24;
    public const float SECONDSINMINUTE = 60f;

    public static System.Action<float> OnTimeUpdated;
    public static float Now => Instance._normalizedTime;

    [Header("Time Settings")]
    [SerializeField] private float realTimeMinutesPerDay = 15f;
    [SerializeField] private float timeMultiplier = 1f;
    [SerializeField] private bool isCycleActive = true;
    [SerializeField] private float updateInterval = 0.1f;

    private float DurationInSeconds => realTimeMinutesPerDay * SECONDSINMINUTE;
    private float _calculateTime;
    [SerializeField] private float _normalizedTime;
    private float _updateTimer;

    private void Start()
    {
        StartCoroutine(TimerRoutine());
    }
    private IEnumerator TimerRoutine()
    {
        while (true)
        {
            if (isCycleActive)
            {
                _updateTimer += Time.deltaTime;

                if (_updateTimer >= updateInterval)
                {
                    _normalizedTime = (_calculateTime % DurationInSeconds) / DurationInSeconds;
                    OnTimeUpdated?.Invoke(_normalizedTime);

                    _calculateTime += _updateTimer * timeMultiplier;
                    _calculateTime %= DurationInSeconds;
                    _updateTimer = 0f;
                }
            }

            yield return null;
        }
    }
    public void SetStartHour(int hour)
    {
        Mathf.Clamp(hour, 0, HOURSPERDAY - 1);

        _calculateTime = (DurationInSeconds * hour / HOURSPERDAY);
    }
    public void PauseCycle() => isCycleActive = false;
    public void ResumeCycle() => isCycleActive = true;
}
