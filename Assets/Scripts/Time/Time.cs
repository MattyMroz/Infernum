using UnityEngine;
using TMPro;

public class Time : MonoBehaviour
{
    private static Time instance;

    public static float timer;
    public static bool timeStarted = true;
    private static bool paused = false;

    private const float multiplier = (60 * 8) / 3f; // base 3
    private readonly System.TimeSpan startTime = new System.TimeSpan(8, 0, 0);
    [SerializeField] private int days;
    [SerializeField] private string the_time;

    [SerializeField] private EndOfDayManager endOfDayManager;

    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        if (timeStarted && !paused)
        {
            timer += UnityEngine.Time.deltaTime * multiplier;
        }


        UpdateUI();
    }

    private void UpdateUI()
    {
        System.TimeSpan currentTime = new System.TimeSpan(0, 0, (int)timer);
        System.TimeSpan simulatedTime = startTime.Add(currentTime);

        if (simulatedTime.TotalHours >= 16)
        {
            days++;
            timer = 0;

            endOfDayManager.StartEndOfDay();
        }

        if (days == 14)
        {
            simulatedTime = new System.TimeSpan(16, 0, 0);
            timeStarted = false;
        }

        //statsText.text = $"Day: {days}\nTime: {simulatedTime:hh\\:mm}";
        the_time = simulatedTime.ToString(@"hh\:mm");
    }

    public static int Days => instance.days;
    public static string Time_now => instance.the_time;

    public static void PauseTime()
    {
        paused = true;
    }

    public static void ResumeTime()
    {
        if (timeStarted)
        {
            paused = false;
        }
    }
}
