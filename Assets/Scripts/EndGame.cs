using UnityEngine;
using TMPro;

public class EndGame : MonoBehaviour
{
    [SerializeField] int lastDay = 14;
    [SerializeField] Exams examsRoot;
    [SerializeField] Player[] players;          // P1 = 0, P2 = 1 …
    [SerializeField] TextMeshProUGUI[] lines;           // jedno pole tekstowe na gracza
    [SerializeField] GameObject endPanel;
    [SerializeField] SoundtrackManager soundtrackManager;

    bool finished;

    void Update()
    {
        if (!finished && Time.Days >= lastDay)
            ShowResults();
    }

    void ShowResults()
    {
        soundtrackManager.StartCoroutine(soundtrackManager.FadeMusic(soundtrackManager.baseAudioVolume, 0f));

        finished = true;
        endPanel.SetActive(true);

        int slots = Mathf.Min(players.Length, lines.Length);

        Time.PauseTime();
        Time.ResetClock();
        Time.PauseTime();

        for (int i = 0; i < slots; i++)
        {
            Player p = players[i];
            int id = Mathf.Clamp(p.id, 0, 1);

            int totalPts = 0;
            foreach (var ex in examsRoot.exams)
            {
                if (id < ex.grade.Length)
                    totalPts += Mathf.RoundToInt(ex.grade[id] * ex.exams_ects);
            }

            lines[i].text = $"{p.player_name}:  {totalPts} pkt";
        }

        for (int i = slots; i < lines.Length; i++)
            lines[i].text = "";
    }

    public void ResetFlag() => finished = false;
}
