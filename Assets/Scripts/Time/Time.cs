using UnityEngine;
using TMPro;

public class Time : MonoBehaviour
{
    public static float timer;
    public static bool timeStarted = true;

    [SerializeField] TextMeshProUGUI statsText;

    void Update()
    {
        FlowOfTime();
        UpdateUI();
    }

    private static void FlowOfTime()
    {
        if (timeStarted)
        {
            timer += UnityEngine.Time.deltaTime;
        }
    }

    private void UpdateUI()
    {
        int theory_days = Mathf.FloorToInt(timer / 60F);
        statsText.text = "Day: " + theory_days;
    }
}