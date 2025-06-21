using NUnit.Framework;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public enum ExamType { Math, IT, Programming, Graphics, Electrotechnics, NR_TYPES }

public class Exam : Interactable
{
    static int s_next_id;
    public int id { get; private set; }
    public string exam_name { get; private set; }
    public List<ExamType> exam_types;
    public int score_to_pass, max_score;
    public int[] score = new int[2];
    public int ects { get; private set; }
    public bool[] passed = new bool[2], failed = new bool[2];
    public int[] takes = new int[2];
    public float[] grade = new float[2];

    [SerializeField] public int exams_ects;

    private void Start()
    {
        id = Interlocked.Increment(ref s_next_id);
        id--;

        exam_name = name;
        score_to_pass = exams_ects * 5;
        max_score = score_to_pass * 2;
    }

    public override void React(GameObject player)
    {
        Player player_script = player.GetComponent<Player>();

        player_script.TakeExam(this);
    }
}

