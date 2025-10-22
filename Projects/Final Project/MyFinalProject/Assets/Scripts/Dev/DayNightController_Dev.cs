public class DayNightController_Dev : SingletonMonoBehaviour<DayNightController_Dev>
{
    public static DayNightEvents_Dev Events => Instance._dayNightEvents;
    public static DayNightLighting_Dev Lighting => Instance._dayNightLighting;

    private DayNightEvents_Dev _dayNightEvents;
    private DayNightLighting_Dev _dayNightLighting;

    private void Start()
    {
        _dayNightEvents = GetComponent<DayNightEvents_Dev>();
        _dayNightLighting = GetComponent<DayNightLighting_Dev>();

        TimeManager_Dev.Instance.SetStartHour(6);
        TimeManager_Dev.OnTimeUpdated = (time) => 
        {
            if (_dayNightEvents != null)
                Events.Evaluate(time);

            if (_dayNightLighting != null)
                Lighting.Evaluate(time);
        };
    }
}
