using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static UnityEngine.Rendering.DebugUI;

public class ExamDisplay : Interactable
{

    [SerializeField] GameObject examUI;

    [SerializeField] Sprite[] diceSprites;
    [SerializeField] Sprite defaultDiceSprite;
    [SerializeField] UnityEngine.UI.Image diceImage;
    [SerializeField] TextMeshProUGUI diceText;
    [SerializeField] TextMeshProUGUI playerName;
    [SerializeField] TextMeshProUGUI examName;
    [SerializeField] TextMeshProUGUI passRate;
    [SerializeField] TextMeshProUGUI takes;
    [SerializeField] TextMeshProUGUI skills;
    [SerializeField] TextMeshProUGUI day;
    [SerializeField] TextMeshProUGUI hour;

    [SerializeField] GameObject passed;
    [SerializeField] GameObject failed;
    [SerializeField] GameObject wisdomChecker;

    [SerializeField] Player player;

    private readonly List<MonoBehaviour> disabled = new();
    [SerializeField] GameObject playerUI;

    [SerializeField] KeyCode takeExamButton;
    [SerializeField] KeyCode exitButton;

    private int diceResult = 0;
    [SerializeField] private bool opened = false;
    private bool _using = false;

    private Exam _exam;


    private void Start()
    {
        displayName = gameObject.name;
        player = GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (opened)
        {
            if (!examUI.activeSelf && opened)
            {
                Close();
                GetComponent<InputManager>().enabled = false;
                GetComponent<Movement>().ResetVelocity(); GetComponent<Movement>().enabled = false;
                GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
                //ToggleUI(false);
            }

            if (!_using && Input.GetKeyDown(takeExamButton) && player.Wisdom >= 25)
                StartCoroutine(DiceRoll());
            if (!_using && Input.GetKeyDown(exitButton))
                Close();

            day.text = (Time.Days + 1).ToString();
            hour.text = Time.Time_now;
        }
    }

    public IEnumerator DiceRoll()
    {
        if (_exam.takes[player.id] >= 2 || _exam.passed[player.id]) yield break;

        int takesExam = _exam.takes[player.id];

        takesExam += 1;
        _using = true;

        diceResult = player.StartExam(_exam);

        diceImage.sprite = defaultDiceSprite;
        diceText.text = "";

        foreach (Sprite dice in diceSprites)
        {
            diceImage.sprite = dice;
            yield return new WaitForSeconds(0.25f);
        }

        diceImage.sprite = defaultDiceSprite;
        diceText.text = diceResult.ToString();
        takes.text = _exam.takes[player.id] >= 2 ? "2" : (_exam.takes[player.id] + 1).ToString();

        bool passedExam = diceResult >= _exam.score_to_pass;
        bool failedExam = diceResult < _exam.score_to_pass;

        if (passedExam)
        {
            passed.SetActive(true);
            _exam.PlayPassedAudio();
            _exam.passed[player.id] = true;
            _exam.failed[player.id] = false;

        }
        else
            passed.SetActive(false);
        if (failedExam && takesExam >= 2)
        {
            failed.SetActive(true);
            _exam.PlayFailedAudio();
            _exam.passed[player.id] = false;
            _exam.failed[player.id] = true;
        }
        else
            failed.SetActive(false);

        if(!passedExam)
            _exam.takes[player.id] = takesExam;

        if (player.Wisdom < 25 && !_exam.passed[player.id] && !_exam.failed[player.id])
            wisdomChecker.SetActive(true);
        else
            wisdomChecker.SetActive(false);


        yield return new WaitForSeconds(1.5f);


        _using = false;
    }

    public void Open(Exam exam)
    {
        opened = true;

        _exam = exam;

        if (_exam.passed[player.id])
            passed.SetActive(true);
        else
            passed.SetActive(false);
        if (_exam.failed[player.id] && _exam.takes[player.id] >= 2)
            failed.SetActive(true);
        else
            failed.SetActive(false);

        if (player.Wisdom < 25 && !_exam.passed[player.id] && !_exam.failed[player.id])
            wisdomChecker.SetActive(true);
        else
            wisdomChecker.SetActive(false);

        examUI.SetActive(true);
        diceText.text = "";
        takes.text = _exam.takes[player.id] >= 2 ? "2" : (_exam.takes[player.id] + 1).ToString();
        playerName.text = player.player_name;
        examName.text = exam.gameObject.name;
        passRate.text = _exam.score_to_pass + "/" + exam.max_score;

        skills.text = "";

        for(int type = 0; type < _exam.exam_types.Count; type++)
        {
            string examTypeString = "";

            switch (_exam.exam_types[type]) {

                case ExamType.Math:
                    examTypeString = "Math: ";
                    break;
                case ExamType.IT:
                    examTypeString = "IT: ";
                    break;
                case ExamType.Electrotechnics:
                    examTypeString = "Electrotechnics: ";
                    break;
                case ExamType.Programming:
                    examTypeString = "Programming: ";
                    break;
                case ExamType.Graphics:
                    examTypeString = "Graphics: ";
                    break;
            }


            skills.text += examTypeString + player.LvlIncrease(player.exams_knowledge[(int)_exam.exam_types[type]]).lvl.ToString() + (type >= (_exam.exam_types.Count - 1) ? "" :  ", ");
        }

        ToggleUI(false);

        GetComponent<InputManager>().enabled = false;
        GetComponent<Movement>().ResetVelocity(); GetComponent<Movement>().enabled = false;
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;

    }

    void Close()
    {
        examUI.SetActive(false);
        opened = false;

        ToggleUI(true);

        GetComponent<InputManager>().enabled = true;
        GetComponent<Movement>().ResetVelocity(); GetComponent<Movement>().enabled = true;
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;

        _using = false;
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

        if(state)
            disabled.Clear();
    }
}
