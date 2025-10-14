using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Light2D))]
public class DayNightCycle : MonoBehaviour
{
    private const int MIDNIGHT = 0;

    [Header("Time Settings")]
    [Tooltip("Real-time minutes for one full in-game day")]
    public float realTimeMinutesPerDay = 15f;
    public int gameHoursPerDay = 24;
    public int sunRiseHour = 6;
    public int nextDayHour = 23;
    public float timeMultiplier = 1f;

    [Header("Lighting")]
    public Light2D globalLight;
    public Gradient lightColorGradient;
    public Color[] dayNightColors;
    public AnimationCurve lightIntensityCurve;

    [Header("Events")]
    public UnityEvent OnSunrise;
    public UnityEvent OnNextDay;

    private float dayDurationInSeconds;
    private float calculateTime;
    private float hoursInDay;
    private int currentHour;
    private int previousHour;
    private float currentIntensity;
    private bool nextDayTrigger;
    private bool sunriseTrigger;

    private void Start()
    {
        globalLight = GetComponent<Light2D>();
        SetupGradient();
        SetupIntensityCurve();

        dayDurationInSeconds = realTimeMinutesPerDay * 60f;
        hoursInDay = gameHoursPerDay;

        calculateTime = (sunRiseHour / hoursInDay) * dayDurationInSeconds + 0.01f;

        Debug.Log("Game start");
        StartCoroutine(DayNightCycleRoutine());
    }

    private IEnumerator DayNightCycleRoutine()
    {
        while (true)
        {
            float normalizedTime = (calculateTime % dayDurationInSeconds) / dayDurationInSeconds;

            UpdateLighting(normalizedTime);
            UpdateHourOfDay(normalizedTime);

            calculateTime += Time.deltaTime * timeMultiplier;
            yield return null;
        }
    }

    private void UpdateHourOfDay(float normalizedTime)
    {
        currentHour = Mathf.FloorToInt(normalizedTime * hoursInDay) % gameHoursPerDay;

        // Sunrise trigger
        if (!sunriseTrigger && previousHour < sunRiseHour && currentHour >= sunRiseHour)
        {
            OnSunrise?.Invoke();
            sunriseTrigger = true;
            Debug.Log("Sunrise triggered!");
        }
        else if (sunriseTrigger && currentHour != sunRiseHour)
        {
            sunriseTrigger = false;
        }

        // Next day trigger
        if (!nextDayTrigger && previousHour == nextDayHour && currentHour == MIDNIGHT)
        {
            OnNextDay?.Invoke();
            nextDayTrigger = true;
            Debug.Log("Next Day triggered!");
        }
        else if (nextDayTrigger && currentHour != MIDNIGHT)
        {
            nextDayTrigger = false;
        }

        previousHour = currentHour;
    }

    private void UpdateLighting(float normalizedTime)
    {
        globalLight.color = lightColorGradient.Evaluate(normalizedTime);
        currentIntensity = lightIntensityCurve.Evaluate(normalizedTime);
        globalLight.intensity = currentIntensity;
    }

    private void SetupGradient()
    {
        int colorCount = dayNightColors.Length;
        GradientColorKey[] colorKeys = new GradientColorKey[colorCount];
        GradientAlphaKey[] alphaKeys = new GradientAlphaKey[2];

        for (int i = 0; i < colorCount; i++)
        {
            float time = i / (float)(colorCount - 1);
            colorKeys[i] = new GradientColorKey(dayNightColors[i], time);
        }

        alphaKeys[0] = new GradientAlphaKey(1f, 0f);
        alphaKeys[1] = new GradientAlphaKey(1f, 1f);

        lightColorGradient = new Gradient();
        lightColorGradient.SetKeys(colorKeys, alphaKeys);
    }

    private void SetupIntensityCurve()
    {
        lightIntensityCurve = new AnimationCurve(
            new Keyframe(0.00f, 0.005f),
            new Keyframe(0.20f, 0.25f),
            new Keyframe(0.35f, 1.0f),
            new Keyframe(0.65f, 1.0f),
            new Keyframe(0.80f, 0.25f),
            new Keyframe(1.00f, 0.005f)
        );

        for (int i = 0; i < lightIntensityCurve.length; i++)
        {
            lightIntensityCurve.SmoothTangents(i, 0.5f);
        }
    }
}
