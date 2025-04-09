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
        exams = GameObject.FindObjectsByType<Exam>(FindObjectsSortMode.None).ToList(); // Add all of the exams to the list
    }

    public void SetListText()
    {
        if (exams.Count == 0)
            exams_UItext.text = "No more exams";

        for (int i = 0; i < exams.Count; i++)
        {
            exams_UItext.text += exams[i].exam_name;
        }
    }
}
