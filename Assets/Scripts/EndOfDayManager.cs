using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndOfDayManager : MonoBehaviour
{
    [Header("Fade & UI")]
    [SerializeField] private Image fadeImage; // Czarny Image na Canvasie
    [SerializeField] private TextMeshProUGUI dayText; // Tekst z informacj¹ o dniu
    [SerializeField] private float fadeDuration = 1.5f;
    [SerializeField] private float messageDuration = 2.5f;

    [Header("Gracze i start")]
    [SerializeField] private Player[] players;
    [SerializeField] private Transform[] startPositions;

    private int currentDay = 1;

    public void StartEndOfDay()
    {
        StartCoroutine(EndOfDayRoutine());
    }

    private IEnumerator EndOfDayRoutine()
    {
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

        currentDay++;
    }

    private IEnumerator FadeScreen(float fromAlpha, float toAlpha)
    {
        Color color = fadeImage.color;
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            float t = elapsed / fadeDuration;
            color.a = Mathf.Lerp(fromAlpha, toAlpha, t);
            fadeImage.color = color;
            elapsed += Time.deltaTime;
            yield return null;
        }

        color.a = toAlpha;
        fadeImage.color = color;
    }

    public void SetDay(int day)
    {
        currentDay = day;
    }
}
