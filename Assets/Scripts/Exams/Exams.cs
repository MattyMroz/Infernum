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
