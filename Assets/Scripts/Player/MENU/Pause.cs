using TMPro;
using UnityEngine;

public class Pause : MonoBehaviour
{
    [SerializeField] private GameObject _pauseScreen;
    [SerializeField] private TextMeshProUGUI _day;
    [SerializeField] private TextMeshProUGUI _hour;

    private bool isPaused = false;

    void Start()
    {
        _pauseScreen.SetActive(false);
        UnityEngine.Time.timeScale = 1f;
        Time.ResumeTime();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPaused)
                PauseGame();
            else
                ResumeGame();
        }

        if (isPaused)
            UpdateHud();
    }

    private void PauseGame()
    {
        isPaused = true;
        _pauseScreen.SetActive(true);
        UnityEngine.Time.timeScale = 0f;
        Time.PauseTime();
    }

    public void ResumeGame()
    {
        if (!isPaused) return;

        isPaused = false;
        _pauseScreen.SetActive(false);
        UnityEngine.Time.timeScale = 1f;
        Time.ResumeTime();
    }

    private void UpdateHud()
    {
        _day.text = Time.Days.ToString();
        _hour.text = Time.Time_now;
    }
}
