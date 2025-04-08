using System.Collections.Generic;
using System.Numerics;
using UnityEngine;


public class Player : MonoBehaviour
{
    public int id;

    [Header("Stats")]
    [SerializeField] int wisdom;
    [SerializeField] int hunger;
    [SerializeField] int endurance;
    public int[] exams_knowledge = new int[(int)ExamType.NR_TYPES]; // Math, IT, Programming, Graphics, Electrotechnics

    public List<Exam> passed_exams;
    public Exams player_exams;
    

    public int Wisdom { get { return wisdom; } set { wisdom = value; } }
    public int Endurance { get { return endurance; } set { endurance = value; } }
    public int Hunger { get { return hunger; } set { hunger = value; } } //Using in DisplayStats

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

    public void TakeExam(Exam exam)
    {

        int chance = Random.Range(1, 21); // Max value is exclusive
        int overallScore = chance;

        for (int i = 0; i < exam.exam_types.Count; i++)
        {
            overallScore += exams_knowledge[(int)exam.exam_types[i]];

            if (overallScore >= exam.score_to_pass)
            {

                if (overallScore > exam.max_score)
                    overallScore = exam.max_score;

                exam.passed[id] = true;
                player_exams.SetListText();

                break;
            }
            else
            {
                exam.failed[id] = true;
            }

            exam.score[id] = overallScore;
        }
    }
}
