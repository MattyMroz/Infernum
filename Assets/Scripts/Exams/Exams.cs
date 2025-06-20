using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;


public class Exams : MonoBehaviour
{
    public TextMeshProUGUI exams_UItext;
    public List<Exam> exams;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InitExams();
        //SetListText();
        ResetExams();
    }

    void ResetExams()
    {

        for (int i = 0; i < transform.childCount; i++)
        {
            Exam exam = transform.GetChild(i).GetComponent<Exam>();
            if (exam != null)
            {
                for (int id = 0; id < 2; id++)
                    exam.score[id] = 0;
            }
        }
    }

    private void InitExams()
    {
        exams = new List<Exam>();
        for (int i = 0; i < transform.childCount; i++)
        {
            Exam exam = transform.GetChild(i).GetComponent<Exam>();
            if (exam != null)
            {
                exams.Add(exam);
            }
        }
    }

    public void SetListText()
    {
        exams_UItext.text = "";

        if (exams.Count == 0)
            exams_UItext.text = "No more exams";

        for (int i = 0; i < exams.Count; i++)
        {
            exams_UItext.text += exams[i].exam_name;
        }
    }
}

