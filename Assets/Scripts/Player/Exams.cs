using UnityEngine;

[System.Serializable]
public class Exam {
    public string name;
    public int score;
    public int maxScore;
    public bool passed;
}

public class Exams : MonoBehaviour
{
    public string[] examNames = new string[] { "Math", "Physics", "Chemistry", "Biology", "History" };
    public Exam[] exams = new Exam[5];

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InitExams();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void InitExams()
    {
        for(int i = 0; i < examNames.Length; i++)
        {
            exams[i].name = examNames[i];
            exams[i].score = 0;
            exams[i].maxScore = 100;
            exams[i].passed = false;
        }
    }
}
