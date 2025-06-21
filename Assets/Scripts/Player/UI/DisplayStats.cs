using TMPro;
using System.Collections;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using System.Collections.Generic;


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

    private readonly List<MonoBehaviour> disabled = new();
    [SerializeField] GameObject playerUI;



    private Player _player;
    private Rigidbody2D _rb;

    private IEnumerator Start()     
    {
        _player = player.GetComponent<Player>();
        _rb = player.GetComponent<Rigidbody2D>();
        panel.SetActive(false);

        yield return null;          

        UpdateStatsUI();
    }

    void Update()
    {
        if (Input.GetKey(displayKey))
        {
            player.GetComponent<Movement>().ResetVelocity();
            player.GetComponent<Movement>().enabled = false;
            _rb.bodyType = RigidbodyType2D.Static;

            ToggleUI(false);


            panel.SetActive(true);
            UpdateStatsUI();
        }
        else if (Input.GetKeyUp(displayKey))
        {
            ToggleUI(true);


            player.GetComponent<Movement>().enabled = true;
            _rb.bodyType = RigidbodyType2D.Dynamic;
            panel.SetActive(false);
        }
    }

    private void UpdateStatsUI()
    {
        Name.text = _player.player_name;
        AccountBalance.text = "$" + _player.accountBalance.ToString();
        Endurance.text = _player.Endurance.ToString() + "\\100";
        Sanity.text = _player.Wisdom.ToString() + "\\100";

        // EGZAMINY + ETCS
        int passedExams = 0;
        int ects = 0;

        int totalExams = (_player.player_exams != null && _player.player_exams.exams != null) ? _player.player_exams.exams.Count : 0;

        if (totalExams > 0)
        {
            foreach (var exam in _player.player_exams.exams)         
            {
                if (exam.passed.Length > _player.id && exam.passed[_player.id])
                {                                                   
                    passedExams++;
                    ects += exam.ects;                              
                }
            }
        }

        AllExams.text = $"{passedExams}/{totalExams}";
        ECTS.text = $"{ects}/30";


        // Czas i dzień
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

            int remainder = knowledge % 100;
            int toNext = remainder == 0 ? (knowledge == 0 ? 100 : 0) : 100 - remainder;

            Lvl.text += level.ToString() + '\n';
            NextLvl.text += toNext.ToString() + '\n';
        }

    }


    private void ToggleUI(bool state)
    {

        foreach (var s in playerUI.GetComponents<MonoBehaviour>())
        {
            if (s == this) continue;

            if (s.GetType().Name.StartsWith("Display"))
            {
                if (state)
                {
                    if (disabled.Contains(s)) s.enabled = true;
                }
                else
                {
                    if (s.enabled)
                    {
                        s.enabled = false;
                        disabled.Add(s);
                    }
                }
            }
        }

        if (state)
            disabled.Clear();
    }
}
