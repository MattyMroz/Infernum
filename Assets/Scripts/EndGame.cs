using UnityEngine;
using TMPro;

public class EndGame : MonoBehaviour
{
    [SerializeField] int lastDay = 14;
    [SerializeField] Exams examsRoot;
    [SerializeField] Player[] players;   // P1 = 0, P2 = 1 …
    [SerializeField] TextMeshProUGUI[] lines;     // jedno pole tekstowe na gracza
    [SerializeField] GameObject endPanel;

    bool finished;

    void Update()
    {
        if (!finished && Time.Days >= lastDay)
            ShowResults();
    }

    void ShowResults()
    {
        finished = true;
        endPanel.SetActive(true);

        int examsTotal = examsRoot.exams.Count;
        int slots = Mathf.Min(players.Length, lines.Length);

        Time.PauseTime();             
        Time.ResetClock();          

        for (int i = 0; i < slots; i++)
        {
            Player p = players[i];
            int idx = Mathf.Clamp(p.id, 0, 1);

            int passed = 0;
            foreach (var ex in examsRoot.exams)
                if (idx < ex.passed.Length && ex.passed[idx])
                    passed++;

            lines[i].text = $"{p.player_name}:  {passed} / {examsTotal} zdanych";
        }

        for (int i = slots; i < lines.Length; i++)
            lines[i].text = "";
    }

    public void ResetFlag()
    {
        finished = false;
    }
}
