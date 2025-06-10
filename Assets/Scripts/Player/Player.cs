using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

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
    [SerializeField] int hunger;
    [SerializeField] int endurance;

    [SerializeField] public int accountBalance = 0;

    public int[] exams_knowledge = new int[(int)ExamType.NR_TYPES]; // Math, IT, Programming, Graphics, Electrotechnics

    public List<Exam> passed_exams;
    public Exams player_exams;

    public GameObject current_map_bounds;

    public int Wisdom { get { return wisdom; } set { wisdom = value; } }
    public int Endurance { get { return endurance; } set { endurance = value; } }
    public int Hunger { get { return hunger; } set { hunger = value; } } //Using in DisplayStats


    public List<MinigameConfig> minigames;
    public MinigameConfig GetConfig(MinigameID id) =>
            minigames.Find(c => c.id == id);


    private void InitStats()
    {
        wisdom = 5;

        for (int i = 0; i < (int)ExamType.NR_TYPES; i++)
            exams_knowledge[i] = 5;

        hunger = 0;
        endurance = 5;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InitStats();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void IncreaseWisdom(int amount)
    {
        wisdom += amount;
    }

    public void IncreaseEndurance(int amount)
    {
        endurance += amount;
    }

    public void DecreaseWisdom(int amount)
    {
        wisdom -= wisdom > 0 ? amount : 0;
    }

    public void DecreaseEndurance(int amount)
    {
        endurance -= endurance > 0 ? amount : 0;
    }

    public void DrinkBeer(int wisdom_decrease, int endurance_increase)
    {
        wisdom -= wisdom > 0 ? wisdom_decrease : 0;
        endurance += endurance_increase;
    }

    public void SearchTrash()
    {
        int chance = Random.Range(1,101);

        if (chance <= 80)
        {
            accountBalance += Random.Range(1, 11);
        }
    }
    public void TakeExam(Exam exam)
    {

        //StartCoroutine(GetComponent<ExamDisplay>().DiceRoll(overallScore));
        GetComponent<ExamDisplay>().Open(exam);
    }

    public int StartExam(Exam exam)
    {

        int chance = Random.Range(1, 21); // Max value is exclusive
        int overallScore = chance;

        for (int i = 0; i < exam.exam_types.Count; i++)
        {
            overallScore += (int)Mathf.Pow(LvlIncrease(exams_knowledge[(int)exam.exam_types[i]]).lvl, 2f);

            if (overallScore >= exam.score_to_pass)
            {

                if (overallScore > exam.max_score)
                    overallScore = exam.max_score;

                exam.passed[id] = true;
                exam.score[id] = overallScore;
                exam.failed[id] = false;
                //player_exams.SetListText();

                break;
            }
            else
            {
                exam.failed[id] = true;
            }

            exam.score[id] = overallScore;
        }

        return overallScore;
    }

    private void OnTriggerEnter2D(Collider2D collision)
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
