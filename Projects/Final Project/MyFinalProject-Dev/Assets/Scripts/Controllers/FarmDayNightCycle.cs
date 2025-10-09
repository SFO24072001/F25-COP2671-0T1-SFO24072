using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Light2D))]
public class FarmDayNightCycle : MonoBehaviour
{
    [Header("Cycle Settings")]
    public float fullDayLength = 120f; // Real-time seconds for a full 24-hour cycle
    private float timeOfDay = 0f; // 0 to 24

    [Header("Lighting")]
    public Light2D directionalLight;
    private Gradient lightColor;
    private AnimationCurve lightIntensityCurve;

    [Header("Fog Settings")]
    public AnimationCurve fogDensityCurve;
    public Color fogColor = new Color(0.7f, 0.8f, 0.9f); // Light blue-gray

    [Header("Ambient Sounds")]
    public AudioSource morningAmbience;
    public AudioSource nightAmbience;
    private bool isDaytime;

    void Start()
    {
        directionalLight = GetComponent<Light2D>();

        SetupGradient();
        SetupIntensityCurve();
        SetupFogCurve();
        RenderSettings.fogColor = fogColor;
    }

    void Update()
    {
        timeOfDay += (24f / fullDayLength) * Time.deltaTime;
        if (timeOfDay >= 24f) timeOfDay -= 24f;

        float normalizedTime = timeOfDay / 24f;

        directionalLight.color = lightColor.Evaluate(normalizedTime);
        directionalLight.intensity = lightIntensityCurve.Evaluate(normalizedTime);
        directionalLight.transform.rotation = Quaternion.Euler(new Vector3((normalizedTime * 360f) - 90f, 170f, 0));

        RenderSettings.fog = true;
        RenderSettings.fogDensity = fogDensityCurve.Evaluate(normalizedTime);

        bool currentlyDay = lightIntensityCurve.Evaluate(normalizedTime) > 0.5f;
        if (currentlyDay != isDaytime)
        {
            isDaytime = currentlyDay;
            if (isDaytime)
            {
                morningAmbience.Play();
                nightAmbience.Stop();
            }
            else
            {
                nightAmbience.Play();
                morningAmbience.Stop();
            }
        }
    }

    void SetupGradient()
    {
        GradientColorKey[] colorKeys = new GradientColorKey[7];
        GradientAlphaKey[] alphaKeys = new GradientAlphaKey[2];

        colorKeys[0].color = new Color32(10, 15, 30, 255);    colorKeys[0].time = 0.00f;
        colorKeys[1].color = new Color32(255, 140, 80, 255);  colorKeys[1].time = 0.20f;
        colorKeys[2].color = new Color32(255, 220, 160, 255); colorKeys[2].time = 0.35f;
        colorKeys[3].color = new Color32(255, 255, 230, 255); colorKeys[3].time = 0.50f;
        colorKeys[4].color = new Color32(255, 220, 160, 255); colorKeys[4].time = 0.65f;
        colorKeys[5].color = new Color32(255, 100, 100, 255); colorKeys[5].time = 0.80f;
        colorKeys[6].color = new Color32(10, 15, 30, 255);    colorKeys[6].time = 1.00f;

        alphaKeys[0].alpha = 1.0f; alphaKeys[0].time = 0.0f;
        alphaKeys[1].alpha = 1.0f; alphaKeys[1].time = 1.0f;

        lightColor = new Gradient();
        lightColor.SetKeys(colorKeys, alphaKeys);
    }

    void SetupIntensityCurve()
    {
        lightIntensityCurve = new AnimationCurve(
            new Keyframe(0.00f, 0.0f),
            new Keyframe(0.20f, 0.3f),
            new Keyframe(0.35f, 1.0f),
            new Keyframe(0.65f, 1.0f),
            new Keyframe(0.80f, 0.3f),
            new Keyframe(1.00f, 0.0f)
        );

        for (int i = 0; i < lightIntensityCurve.length; i++)
        {
            lightIntensityCurve.SmoothTangents(i, 0.5f);
        }
    }

    void SetupFogCurve()
    {
        fogDensityCurve = new AnimationCurve(
            new Keyframe(0.00f, 0.02f),
            new Keyframe(0.20f, 0.04f),
            new Keyframe(0.35f, 0.01f),
            new Keyframe(0.65f, 0.01f),
            new Keyframe(0.80f, 0.03f),
            new Keyframe(1.00f, 0.02f)
        );
    }
}
