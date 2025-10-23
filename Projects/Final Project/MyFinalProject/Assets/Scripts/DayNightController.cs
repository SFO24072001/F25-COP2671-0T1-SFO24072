using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Light2D))]
public class DayNightController : MonoBehaviour
{
    public Gradient dayNightColors;
    public AnimationCurve lightIntensityCurve;

    private Light2D _light;

    private void Start()
    {
        _light = GetComponent<Light2D>();
    }
    private void Update()
    {
        _light.color = dayNightColors.Evaluate(TimeManager.Now);
        _light.intensity = lightIntensityCurve.Evaluate(TimeManager.Now);
    }
}
