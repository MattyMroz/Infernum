using UnityEngine;
using TMPro;

public class Time : MonoBehaviour
{
     private static Time instance;

    public static float timer;
    public static bool timeStarted = true;

    [SerializeField] TextMeshProUGUI statsText;

    private const float multiplier = (60 * 8) / 3; //8 hours in 3 minutes -> 60 minutes * 8 hours / 3 minutes
    private readonly System.TimeSpan startTime = new System.TimeSpan(8, 0, 0); // 8:00
    [SerializeField] private int days;
    [SerializeField] private string the_time;

    void Update()
    {
        FlowOfTime();
        UpdateUI();
    }

    private static void FlowOfTime()
    {
        if (timeStarted)
        {
            timer += UnityEngine.Time.deltaTime * multiplier; //* 100; add for testing
        }
    }

    private void UpdateUI()
    {
        // Convert timer (in seconds) to a TimeSpan
        System.TimeSpan currentTime = new System.TimeSpan(0, 0, (int)timer);
        System.TimeSpan simulatedTime = startTime.Add(currentTime);

        // new day
        if (simulatedTime.TotalHours >= 16)
        {
            days++;
            timer = 0;
        }

        // 2 weeks
        if(days == 14)
        {
            simulatedTime = new System.TimeSpan(16, 0, 0);
            timeStarted = false; // Stop time
        }

        // display time in hh:mm
       statsText.text = "Day: " + days + "\n" +
                         "Time: " + simulatedTime.ToString(@"hh\:mm");

        the_time = simulatedTime.ToString(@"hh\:mm");
    }


    void Awake()
    {
        instance = this;
    }

public static int Days => instance.days;
public static string Time_now => instance.the_time;
}
