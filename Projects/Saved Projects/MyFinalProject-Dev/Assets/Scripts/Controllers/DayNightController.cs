using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.Universal;

[RequireComponent (typeof(Light2D))]
public class DayNightController : SingletonMonobehaviour<DayNightController>
{
    public UnityEvent OnNextDay;
    public UnityEvent OnSunrise;

    [SerializeField] private Color dayColor = Color.white;
    [SerializeField] private Color nightColor = Color.black;
    [SerializeField] private float dayIntensity = 1.5f;
    [SerializeField] private float nightIntensity = 0.5f;
    [SerializeField] private int sunRiseHour = 6;
    [SerializeField] private int nextDayHour = 24;
    [SerializeField] private float currentIntensity;
    [SerializeField] private Color currentColor;    
    [SerializeField] private int currentHour;
    [SerializeField] private string currentDayName = Constants.Sunday;

    private bool isNextDay;
    [SerializeField] private int previousHour = 24;

    private float calculateTime;
    [SerializeField] private float cycleDuration = 60f;
    private Light2D globalLight;
    private Coroutine timerCoroutine;

    private void Start()
    {
        globalLight = GetComponent<Light2D>();

        // on startup set current time equal to Sunrise
        calculateTime = (sunRiseHour / 24f) * cycleDuration;
        UpdateLighting(calculateTime);

        timerCoroutine = StartCoroutine(TimeCycleRoutine());
    }    
    private IEnumerator TimeCycleRoutine()
    {
        while (true) 
        {
            calculateTime += Time.deltaTime;
            var normalizedTime = (calculateTime % cycleDuration) / cycleDuration;
            UpdateLighting(normalizedTime);
            yield return null;
        }
    }

    private void UpdateLighting(float normalizedTime)
    {
        if (globalLight == null) return;

        currentColor = Color.Lerp(nightColor, dayColor, Mathf.Sin(normalizedTime * Mathf.PI));
        globalLight.color = currentColor;

        currentHour = (Mathf.FloorToInt(normalizedTime * 24) % 24) +1; // 1/24 not 0/23
        

        if (currentHour >= 6f && currentHour < 8f)
        {
            //currentIntensity = Mathf.InverseLerp(6f, 8f, currentHour);
            currentDayName = "Dawn";
        }
        else if (currentHour >= 8f && currentHour <= 16f)
        {
            //currentIntensity = dayIntensity;
            currentDayName = "Afternoon";
        }
        else if (currentHour > 16f && currentHour <= 18f) 
        {
            //currentIntensity = Mathf.InverseLerp(18f, 16f, currentHour);
            currentDayName = "Evening";
        }
        else
        {
            //currentIntensity = nightIntensity;
            currentDayName = "Night";
        }

        currentIntensity = Mathf.Lerp(nightIntensity, dayIntensity, Mathf.Sin(normalizedTime * Mathf.PI));
        currentIntensity = Mathf.Clamp(currentIntensity, nightIntensity, dayIntensity);

        globalLight.intensity = currentIntensity;

        if (previousHour < sunRiseHour && currentHour >= sunRiseHour)
        {
            OnSunrise.Invoke();
            Debug.Log("Sunrise triggered at 6 AM!");
        }
        if (previousHour < nextDayHour && currentHour >= nextDayHour)
        {
            OnNextDay.Invoke();
            Debug.Log("Next Day triggered at 12PM!");
        }
        previousHour = currentHour;
    }
}