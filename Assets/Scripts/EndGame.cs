using UnityEngine;
using TMPro;

public class EndGame : MonoBehaviour
{
    [SerializeField] private Player player;              // Referencja do gracza
    [SerializeField] private Exams exams;                // Lista wszystkich egzaminów w grze
    [SerializeField] private GameObject gameEndPanel;    // Panel czarny koñca gry
    [SerializeField] private TextMeshProUGUI resultText; // Wynik koñcowy
    [SerializeField] private TextMeshProUGUI winLoseText;// Tekst "Wygra³eœ" lub "Przegra³eœ"

    [SerializeField] private int day = 1;
    [SerializeField] private int lastDay = 14;
    [SerializeField] private int thresholdToWin = 50;

    private bool hasEnded = false;

    private void Update()
    {
        if (!hasEnded && day >= lastDay)
        {
            ShowFinalScore();
        }
    }

    public void SetDay(int newDay)
    {
        day = newDay;
    }

    private void ShowFinalScore()
    {
        hasEnded = true;
        int totalPoints = 0;

        int loopLength = Mathf.Min(player.exams_knowledge.Length, exams.exams.Count);

        for (int i = 0; i < loopLength; i++)
        {
            int score = player.exams_knowledge[i];
            float grade;

            int gradeBucket = score / 10;
            switch (gradeBucket)
            {
                case 9: grade = 5.0f; break;
                case 8: grade = 4.5f; break;
                case 7: grade = 4.0f; break;
                case 6: grade = 3.5f; break;
                case 5: grade = 3.0f; break;
                default: grade = 2.0f; break;
            }

            int subjectPoints = Mathf.RoundToInt(grade * exams.exams[i].ects);
            totalPoints += subjectPoints;
        }


        gameEndPanel.SetActive(true);
        resultText.text = $"Twoje punkty: {totalPoints}";
        winLoseText.text = totalPoints >= thresholdToWin ? "WYGRA£EŒ!" : "PRZEGRA£EŒ!";
    }
}
