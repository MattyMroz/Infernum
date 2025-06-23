using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndOfDayManager : MonoBehaviour
{
    [Header("Fade & UI")]
    [SerializeField] private GameObject endOfDayObject;
    [SerializeField] private Image fadeImage; // Czarny Image na Canvasie
    [SerializeField] private TextMeshProUGUI dayText; // Tekst z informacj¹ o dniu
    [SerializeField] private float fadeDuration = 1.5f;
    [SerializeField] private float messageDuration = 2.5f;

    [Header("Gracze i start")]
    [SerializeField] private Player[] players;
    [SerializeField] private GameObject[] playerUIs;
    [SerializeField] private Transform[] startPositions;
    [SerializeField] private DisplayExams[] displayExams;
    [SerializeField] private DisplayStats[] displayStats;

    private readonly List<MonoBehaviour> disabled = new();

    private int currentDay = 0;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            StartEndOfDay();
        }

        currentDay = Time.Days + 1;
    }

    public void StartEndOfDay()
    {
        StartCoroutine(EndOfDayRoutine());
    }

    private IEnumerator EndOfDayRoutine()
    {
        endOfDayObject.SetActive(true);

        Time.PauseTime();

        for(int i = 0; i < players.Length; i++)
        {
            players[i].GetComponent<InputManager>().enabled = false;
            players[i].GetComponent<Movement>().ResetVelocity(); players[i].GetComponent<Movement>().enabled = false;
            players[i].GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
        }

        ToggleUI(false);
        // Fade-in
        yield return StartCoroutine(FadeScreen(0f, 1f));

        // Poka¿ informacjê o dniu
        dayText.text = $"Dzieñ {currentDay}";
        dayText.gameObject.SetActive(true);
        yield return new WaitForSeconds(messageDuration);
        dayText.gameObject.SetActive(false);


        // Teleportacja graczy
        for (int i = 0; i < players.Length; i++)
        {
            if (i < startPositions.Length)
                players[i].transform.position = startPositions[i].position;
        }

        // Fade-out
        yield return StartCoroutine(FadeScreen(1f, 0f));

        endOfDayObject.SetActive(false);

        for (int i = 0; i < players.Length; i++)
        {
            players[i].GetComponent<InputManager>().enabled = true;
            players[i].GetComponent<Movement>().ResetVelocity(); players[i].GetComponent<Movement>().enabled = true;
            players[i].GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;

            players[i].ResetEndurance();
        }

        ToggleUI(true);

        Time.ResumeTime();
    }

    private IEnumerator FadeScreen(float fromAlpha, float toAlpha)
    {


        Color color = fadeImage.color;
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            ToggleUI(false);
            float t = elapsed / fadeDuration;
            color.a = Mathf.Lerp(fromAlpha, toAlpha, t);
            fadeImage.color = color;
            elapsed += UnityEngine.Time.deltaTime;
            yield return null;
        }

        color.a = toAlpha;
        fadeImage.color = color;
    }

    public void SetDay(int day)
    {
        currentDay = day;
    }

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
                        if (disabled.Contains(s)) s.enabled = true;
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

            if (state)
                disabled.Clear();
        }


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
