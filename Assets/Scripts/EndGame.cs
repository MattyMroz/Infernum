﻿using UnityEngine;
using TMPro;
using System.Collections.Generic;
using Unity.VisualScripting;

public class EndGame : MonoBehaviour
{
    [SerializeField] int lastDay = 14;
    [SerializeField] Exams examsRoot;
    [SerializeField] Player[] players;          // P1 = 0, P2 = 1 …
    [SerializeField] TextMeshProUGUI[] lines;           // jedno pole tekstowe na gracza
    [SerializeField] TextMeshProUGUI winWho;
    [SerializeField] GameObject endPanel;
    [SerializeField] SoundtrackManager soundtrackManager;

    [SerializeField] private GameObject[] playerUIs;
    private readonly List<MonoBehaviour> disabled = new();

    bool finished;

    void Update()
    {
        if (!finished && Time.Days >= Time.maxDays) // git gud
            ShowResults();
    }

    void ShowResults()
    {
        soundtrackManager.StartCoroutine(soundtrackManager.FadeMusic(soundtrackManager.baseAudioVolume, 0f));

        ToggleUI(false);

        for (int i = 0; i < players.Length; i++)
        {
            players[i].GetComponent<InputManager>().enabled = false;
            players[i].GetComponent<Movement>().ResetVelocity(); players[i].GetComponent<Movement>().enabled = false;
            players[i].GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
            players[i].GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        }

        finished = true;
        endPanel.SetActive(true);

        int slots = Mathf.Min(players.Length, lines.Length);

        Time.PauseTime();
        Time.ResetClock();
        Time.PauseTime();

        int[] playerPoints = new int[players.Length];
        for (int i = 0; i < slots; i++)
        {
            Player p = players[i];
            int id = Mathf.Clamp(p.id, 0, 1);

            int totalPts = 0;
            foreach (var ex in examsRoot.exams)
            {
                if (ex.grade[id] > 2)
                {
                    if (id < ex.grade.Length)
                        totalPts += Mathf.RoundToInt(ex.grade[id] * ex.exams_ects);
                }

               
            }

            int totalMinusPoints = 0;
            foreach (var ex in examsRoot.exams) 
            {
                if(ex.grade[id] <= 2)
                {
                    if (id < ex.grade.Length)
                        totalMinusPoints += Mathf.RoundToInt(2 * ex.exams_ects);
                }
            }


            totalPts -= totalMinusPoints;

            playerPoints[i] = totalPts;

            lines[i].text = $"{p.player_name}:  {totalPts} pkt";
        }

        for (int i = slots; i < lines.Length; i++)
            lines[i].text = "";


        if (playerPoints[0] > playerPoints[1])
        {
            winWho.text = $"{players[0].player_name} wygrał!";
        }
        else if (playerPoints[0] < playerPoints[1])
        {
            winWho.text = $"{players[1].player_name} wygrał!";
        }
        else
        {
            winWho.text = "Remis!";
        }



            ToggleUI(false);
    }

    public void ResetFlag() => finished = false;

    private void ToggleUI(bool state)
    {

        for (int i = 0; i < playerUIs.Length; i++)
        {

            foreach (var s in playerUIs[i].GetComponents<MonoBehaviour>())
            {
                if (s == this) continue;

                if (s.GetType().Name.StartsWith("Display"))
                {
                    if (state)
                    {
                        if (!s.enabled) s.enabled = true;
                    }
                    else
                    {
                        if (s.enabled)
                        {
                            s.enabled = false;
                            disabled.Add(s);
                        }
                    }
                }
            }
        }

        if (state)
            disabled.Clear();


        for (int i = 0; i < playerUIs.Length; i++)
        {
            foreach (Transform gObject in playerUIs[i].transform)
            {
                if (!state)
                {

                    if (gObject.gameObject.name == "Minigames")
                    {
                        for (int j = 0; j < gObject.childCount; j++)
                        {
                            gObject.GetChild(j).gameObject.SetActive(false);
                        }
                    }
                    gObject.gameObject.SetActive(false);
                }
                else
                {
                    if (gObject.gameObject.name == "Minigames")
                        gObject.gameObject.SetActive(true);
                }
            }
        }
    }
}
