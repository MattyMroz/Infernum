using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

//minigames
public enum MinigameID
{
    Math,
    Prog,
    Inf,
    Graph,
    Elek
}

[System.Serializable]
public class MinigameConfig
{
    public MinigameID id;          // do którego trybu pasuje
    public GameObject panel;       // panel UI na Canvasie gracza
    public KeyCode exitKey;     // klawisz „wyjdŸ”
    public KeyCode[] actionKeys;  // klawisze u¿ywane w trakcie (0-n)
}

public class Player : MonoBehaviour
{
    
    public int id;
    public string player_name;

    [Header("Stats")]
    [SerializeField] int wisdom;
    [SerializeField] int endurance;

    [SerializeField] public int accountBalance = 0;

    public int[] exams_knowledge = new int[(int)ExamType.NR_TYPES]; // Math, IT, Programming, Graphics, Electrotechnics

    public List<Exam> passed_exams;
    public Exams player_exams;

    public GameObject current_map_bounds;

    public int Wisdom { get { return wisdom; } set { wisdom = value; } }
    public int Endurance { get { return endurance; } set { endurance = value; } }


    public List<MinigameConfig> minigames;
    public MinigameConfig GetConfig(MinigameID id) =>
            minigames.Find(c => c.id == id);


    private void InitStats()
    {
        wisdom = 5;

        for (int i = 0; i < (int)ExamType.NR_TYPES; i++)
            exams_knowledge[i] = 5;

        endurance = 100;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        InitStats();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D hit;

        hit = Physics2D.CircleCast(transform.position, 0.1f, UnityEngine.Vector2.zero, 0.1f, 0);

        if (hit)
        {
            if (hit.transform.CompareTag("MapBounds"))
                current_map_bounds = hit.transform.gameObject;
        }
    }

    public void Reset()
    {
        accountBalance = 5;
        wisdom = 100;
        endurance = 100;

        for (int i = 0; i < exams_knowledge.Length; i++)
            exams_knowledge[i] = 0;
    }

    public void IncreaseWisdom(int amount)
    {
        wisdom += wisdom < 100 ? amount : 0;
    }

    public void IncreaseEndurance(int amount)
    {
        endurance += endurance < 100 ? amount : 0 ;
    }

    public void DecreaseWisdom(int amount)
    {
        wisdom -= wisdom > 0 ? amount : 0;
    }

    public void DecreaseEndurance(int amount)
    {
        endurance -= endurance > 0 ? amount : 0;
    }

    public void DrinkBeer(int wisdom_increase, int endurance_increase)
    {
        wisdom += wisdom < 100 ? wisdom_increase : 0;
        endurance -= endurance > 0 ? endurance_increase : 0;
    }

    public void ResetEndurance()
    {

        endurance = 100;
        wisdom = 100;
    }

    public void SearchTrash()
    {
        int chance = Random.Range(1,101);

        if (chance <= 80)
        {
            accountBalance += Random.Range(1, 11);
        }
    }

    public void SpendMoney(int amount)
    {
        if (amount > accountBalance) return;
        accountBalance -= amount;
    }

    public void TakeExam(Exam exam)
    {

        //StartCoroutine(GetComponent<ExamDisplay>().DiceRoll(overallScore));
        GetComponent<ExamDisplay>().Open(exam);
        DecreaseWisdom(25);
    }

    public int StartExam(Exam exam)
    {

        int chance = Random.Range(1, 21); // Max value is exclusive
        int overallScore = chance;
        int knowledgeScore = 0;

        for (int i = 0; i < exam.exam_types.Count; i++)
        {
            knowledgeScore += LvlIncrease(exams_knowledge[(int)exam.exam_types[i]]).lvl;
        }

        knowledgeScore /= exam.exam_types.Count;

        overallScore += knowledgeScore;

        if (overallScore >= exam.score_to_pass)
        {

            if (overallScore > exam.max_score)
                overallScore = exam.max_score;

            exam.passed[id] = true;
            exam.failed[id] = false;
        }
        else
        {
            exam.passed[id] = false;
            exam.failed[id] = true;
        }

        exam.score[id] = overallScore;

        float grade = 2f;

        if (exam.score[id] < exam.score_to_pass)
            grade = 2f;
        else if (exam.score[id] >= exam.max_score) grade = 5f;
        else if (exam.score[id] >= (float)exam.score_to_pass * 1.75f) grade = 4.5f;
        else if (exam.score[id] >= (float)exam.score_to_pass * 1.5f) grade = 4.0f;
        else if (exam.score[id] >= (float)exam.score_to_pass * 1.25f) grade = 3.5f;
        else if (exam.score[id] >= exam.score_to_pass) grade = 3f;

        exam.grade[id] = grade;

        return overallScore;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("MapBounds"))
        {
            current_map_bounds = collision.gameObject;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("MapBounds"))
        {
            current_map_bounds = collision.gameObject;
        }
    }

    // lvl_increace
    public (int lvl, int exp, int divide) LvlIncrease(int exp)
    {
        int level = 0;
        int divide = 100;

        int i = 1;
        while (exp >= i * 100)
        {
            divide = i * 100;
            exp -= divide;
            level++;
            i++;
        }

        divide = i * 100;

        return (level, exp, divide);
    }
}
