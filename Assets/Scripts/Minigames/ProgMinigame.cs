using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ProgMinigame : Interactable
{
    [Header("UI")]
    [SerializeField] private GameObject _panel;
    [SerializeField] private TextMeshProUGUI displayLvl;
    [SerializeField] private TextMeshProUGUI displayExp;
    [SerializeField] private TextMeshProUGUI TimeNow;
    [SerializeField] private TextMeshProUGUI MinigameName;

    [Header("Gameplay")]
    [SerializeField] private GameObject _player;
    [SerializeField] private int keyGain = 1;

    [Header("keys")]
    public KeyCode exit;
    public KeyCode CURRENT_KEY;


    private int _clicks = 0;
    private Player _playerScript;

    // Blokada ruchu i UI
    private bool minigameActive = false;
    private List<MonoBehaviour> previouslyDisabled = new List<MonoBehaviour>();
    private Movement playerMovement;
    private Rigidbody2D playerRb;

    public override void React(GameObject player)
    {
        StartMinigame();
    }

    private IEnumerator Start()
    {
        _playerScript = _player.GetComponent<Player>();
        playerMovement = _player.GetComponent<Movement>();
        playerRb = _player.GetComponent<Rigidbody2D>();

        yield return null;

        UpdateHud();
    }

    private void Update()
    {
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

        if (Input.GetKeyDown(CURRENT_KEY))
        {
            _playerScript.exams_knowledge[2] += keyGain; // programming - id : 2
            _clicks++;

            UpdateHud();
        }
    }

    public void StartMinigame()
    {
        OpenMinigame();
        _clicks = 0;
        UpdateHud();
    }

    private void UpdateHud()
    {
        var result = _playerScript.LvlIncrease(_playerScript.exams_knowledge[2]);
        displayLvl.text = $"{result.lvl}";
        displayExp.text = $"{result.exp} / {result.divide}";

        TimeNow.text = Time.Time_now;
        MinigameName.text = "Programowanie";
    }

    /* ---------- wy³¹czanie innych UI i ruchu ---------- */
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
