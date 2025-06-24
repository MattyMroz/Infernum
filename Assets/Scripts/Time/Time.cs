using UnityEngine;
using TMPro;

public class Time : MonoBehaviour
{
    private static Time instance;

    public static float timer;
    public static bool timeStarted = true;
    private static bool paused = true;
    public static int maxDays = 1;

    private const float multiplier = (60 * 8) / 0.2f; // base 3
    private readonly System.TimeSpan startTime = new System.TimeSpan(8, 0, 0);
    [SerializeField] private int days;
    [SerializeField] private string the_time;

    [SerializeField] private int GameDays;

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
            if (days < maxDays - 1)
                endOfDayManager.StartEndOfDay();

            days++;
            timer = 0;  
        }

        if (days == GameDays)
        {
            simulatedTime = new System.TimeSpan(16, 0, 0);
            timeStarted = false;
        }

        //statsText.text = $"Day: {days}\nTime: {simulatedTime:hh\\:mm}";
        the_time = simulatedTime.ToString(@"hh\:mm");
    }

    public static void ResetClock()
    {
        timer = 0;
        instance.days = 0;

        timeStarted = true;
        paused = false;

        instance.the_time = instance.startTime.ToString(@"hh\:mm");
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
