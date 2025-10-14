public class DayNightController : SingletonMonoBehaviour<DayNightController>
{
    public static DayNightEvents Events => Instance._dayNightEvents;
    public static DayNightLighting Lighting => Instance._dayNightLighting;

    private DayNightEvents _dayNightEvents;
    private DayNightLighting _dayNightLighting;

    private void Start()
    {
        _dayNightEvents = GetComponent<DayNightEvents>();
        _dayNightLighting = GetComponent<DayNightLighting>();

        TimeManager.Instance.SetStartHour(6);
        TimeManager.OnTimeUpdated = (time) => 
        {
            if (_dayNightEvents != null)
                Events.Evaluate(time);

            if (_dayNightLighting != null)
                Lighting.Evaluate(time);
        };
    }
}
