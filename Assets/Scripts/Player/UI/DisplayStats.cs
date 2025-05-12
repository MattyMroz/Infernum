using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class DisplayStats : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI Name;
    [SerializeField] TextMeshProUGUI AccountBalance;
    [SerializeField] TextMeshProUGUI AllExams; // zamiast Strikes
    [SerializeField] TextMeshProUGUI ECTS;
    [SerializeField] TextMeshProUGUI Day;
    [SerializeField] TextMeshProUGUI TimeDisplay;
    [SerializeField] TextMeshProUGUI Endurance;
    [SerializeField] TextMeshProUGUI Sanity;
    [SerializeField] TextMeshProUGUI Lvl;
    [SerializeField] TextMeshProUGUI NextLvl;

    [SerializeField] GameObject panel;
    [SerializeField] GameObject player;
    [SerializeField] KeyCode displayKey;


    private Player _player;
    private Rigidbody2D _rb;

    void Start()
    {
        _player = player.GetComponent<Player>();
        _rb = player.GetComponent<Rigidbody2D>();
        panel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKey(displayKey))
        {
            player.GetComponent<Movement>().ResetVelocity();
            player.GetComponent<Movement>().enabled = false;
            _rb.bodyType = RigidbodyType2D.Static;

            panel.SetActive(true);
            UpdateStatsUI();
        }
        else if (Input.GetKeyUp(displayKey))
        {
            player.GetComponent<Movement>().enabled = true;
            _rb.bodyType = RigidbodyType2D.Dynamic;
            panel.SetActive(false);
        }
    }

    private void UpdateStatsUI()
    {
        Name.text = _player.player_name;
        AccountBalance.text = "$" + _player.accountBalance.ToString();
        Endurance.text = _player.Endurance.ToString();
        Sanity.text = _player.Wisdom.ToString();

        // Egzaminy (wszystkie)
        string examList = "";
        foreach (var exam in _player.passed_exams)
        {
            examList += exam.exam_name + "\n";
        }
        AllExams.text = examList;

        // Punkty ECTS
        int totalEcts = 0;
        foreach (var exam in _player.passed_exams)
            totalEcts += exam.ects;

        ECTS.text = $"{totalEcts}/30";

        // Czas i dzieñ
        int day = Time.Days;

        Day.text = Time.Days.ToString();
        TimeDisplay.text = Time.Time_now;

        // Level
        Lvl.text = "";
        NextLvl.text = "";

        for (int i = 0; i < _player.exams_knowledge.Length && i < 5; i++)
        {
            int knowledge = _player.exams_knowledge[i];
            int level = knowledge / 100;
            int toNext = 100 - (knowledge % 100);
            if (toNext == 100) toNext = 0;

            Lvl.text += level.ToString() + '\n';
            NextLvl.text += toNext.ToString() + '\n';
        }

    }
}
