using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Light2D))]
public class DayNightLighting_Dev : MonoBehaviour
{
    [Header("Lighting")]
    [SerializeField] private AnimationCurve lightIntensityCurve;
    [SerializeField] private Color[] dayNightColors;
    [SerializeField] private Gradient lightColorGradient;

    private Light2D _globalLight;
    private void Awake()
    {
        _globalLight = GetComponent<Light2D>();        
    }
    private void Start()
    {
        SetupGradient();
        SetupIntensityCurve();
    }
    public void Evaluate(float normalizedTime)
    {
        _globalLight.color = lightColorGradient.Evaluate(normalizedTime);
        _globalLight.intensity = lightIntensityCurve.Evaluate(normalizedTime);
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
