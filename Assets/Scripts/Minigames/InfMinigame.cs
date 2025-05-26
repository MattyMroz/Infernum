using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class InfMinigame : MonoBehaviour
{
    /* ----------  UI ---------- */
    [Header("UI")]
    [SerializeField] private GameObject _panel;
    [SerializeField] private TextMeshProUGUI displayLvl;
    [SerializeField] private TextMeshProUGUI displayExp;
    [SerializeField] private TextMeshProUGUI TimeNow;
    [SerializeField] private TextMeshProUGUI MinigameName;
    [SerializeField] private RectTransform playArea;
    [SerializeField] private Image notPlayArea;

    /* ---------- Gameplay ---------- */
    [Header("Gameplay")]
    [SerializeField] private GameObject _player;
    [SerializeField] private Image ball;
    [SerializeField] private float liftForce = 1800f;
    [SerializeField] private float gravity = 900f;
    [SerializeField] private int expPerSec = 5;
    [SerializeField] private int knowledgeIndex = 1;   // slot w exams_knowledge

    [Header("keys")]
    public KeyCode exit;
    public KeyCode CURRENT_KEY;


    /* ---------- Internal ---------- */
    private Player _playerScript;
    private Vector2 _velocity;
    private float _secTimer;
    private int playableZoneDirection = -1;

    // obsługa wyłączania skryptów i ruchu
    private bool minigameActive = false;
    private List<MonoBehaviour> previouslyDisabled = new List<MonoBehaviour>();
    private Movement playerMovement;
    private Rigidbody2D playerRb;

    /* ---------- Start (IEnumerator) ---------- */
    private IEnumerator Start()
    {

        _playerScript = _player.GetComponent<Player>();
        playerMovement = _player.GetComponent<Movement>();
        playerRb = _player.GetComponent<Rigidbody2D>();

        yield return null;

        CenterBall();
        UpdateHud();
    }

    /* ---------- Update ---------- */
    private void Update()
    {
        /* wyjście z minigry */
        if (Input.GetKeyDown(exit))
        {
            if (minigameActive)
            {
                CloseMinigame();
                return;
            }
        }

        if (!minigameActive && _panel.activeSelf)
        {
            OpenMinigame();
        }

        if (!minigameActive)
            return;

        /* sterowanie piłką */
        if (Input.GetKey(CURRENT_KEY))
            _velocity.y += liftForce * UnityEngine.Time.deltaTime;

        _velocity.y -= gravity * UnityEngine.Time.deltaTime;
        _velocity.y = Mathf.Clamp(_velocity.y, -200f, 200f);

        ball.rectTransform.anchoredPosition += _velocity * UnityEngine.Time.deltaTime;

        // Make sure the ball stays in the minigame zone
        float ballClampYBottom = notPlayArea.rectTransform.rect.yMin + ball.rectTransform.rect.height / 2f;
        float ballClampYTop = notPlayArea.rectTransform.rect.yMax - ball.rectTransform.rect.height / 2f;

        ball.rectTransform.anchoredPosition = new Vector2(
            ball.rectTransform.anchoredPosition.x,
            Mathf.Clamp(
                ball.rectTransform.anchoredPosition.y,
                ballClampYBottom,
                ballClampYTop
                ));

        if (ball.rectTransform.anchoredPosition.y == ballClampYBottom
            || ball.rectTransform.anchoredPosition.y == ballClampYTop)
            _velocity = Vector2.zero;

        // Handle playable zone
        float playableClampYBottom = notPlayArea.rectTransform.rect.yMin + playArea.rect.height / 2f;
        float playableClampYTop = notPlayArea.rectTransform.rect.yMax - playArea.rect.height / 2f;

        playArea.anchoredPosition += new Vector2(0f, 100f) * playableZoneDirection * UnityEngine.Time.deltaTime;

        playArea.anchoredPosition = new Vector2(
            playArea.anchoredPosition.x,
            Mathf.Clamp(
                playArea.anchoredPosition.y,
                playableClampYBottom,
                playableClampYTop
                ));

        if (playArea.anchoredPosition.y == playableClampYBottom
            || playArea.anchoredPosition.y == playableClampYTop)
        {
            playableZoneDirection *= -1;
        }


        /* sprawdzanie, czy piłka jest w playArea */
        bool inside = RectTransformUtility.RectangleContainsScreenPoint(
            playArea,
            ball.rectTransform.position,
            null);

        if (inside)
        {
            _secTimer += UnityEngine.Time.deltaTime;
            if (_secTimer >= 1f)
            {
                _playerScript.exams_knowledge[knowledgeIndex] += expPerSec;
                _secTimer -= 1f;
                UpdateHud();
            }
        }
        else
        {
            _secTimer = 0f;
            //_velocity = Vector2.zero;
        }
    }

    /* ---------- Helpers ---------- */
    private void CenterBall()
    {
        ball.rectTransform.anchoredPosition = playArea.rect.center;
        _velocity = Vector2.zero;
    }

    private void UpdateHud()
    {
        var result = _playerScript.LvlIncrease(_playerScript.exams_knowledge[knowledgeIndex]);
        displayLvl.text = $"{result.lvl}";
        displayExp.text = $"{result.exp} / {result.divide}";

        TimeNow.text = Time.Time_now;
        MinigameName.text = "Informatyka";
    }

    public void FocusInput() { }

    /* ---------- wylaczanie innych UI ---------- */
    private void OpenMinigame()
    {
        minigameActive = true;

        DisableOtherUIDisplays();

        if (playerMovement != null)
        {
            playerMovement.ResetVelocity();
            playerMovement.enabled = false;
        }

        if (playerRb != null)
            playerRb.bodyType = RigidbodyType2D.Static;

        _panel.SetActive(true);
    }

    private void CloseMinigame()
    {
        minigameActive = false;

        ReenableUIDisplays();

        if (playerMovement != null)
        {
            playerMovement.ResetVelocity();
            playerMovement.enabled = true;
        }

        if (playerRb != null)
            playerRb.bodyType = RigidbodyType2D.Dynamic;

        _panel.SetActive(false);
    }

    private void DisableOtherUIDisplays()
    {
        previouslyDisabled.Clear();

        MonoBehaviour[] scripts = gameObject.GetComponents<MonoBehaviour>();

        foreach (MonoBehaviour script in scripts)
        {
            if (script != this && script.enabled && script.GetType().Name.StartsWith("Display"))
            {
                script.enabled = false;
                previouslyDisabled.Add(script);
            }
        }
    }

    private void ReenableUIDisplays()
    {
        foreach (MonoBehaviour script in previouslyDisabled)
        {
            if (script != null)
            {
                script.enabled = true;
            }
        }
        previouslyDisabled.Clear();
    }
}


