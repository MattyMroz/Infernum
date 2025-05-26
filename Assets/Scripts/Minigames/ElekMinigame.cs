using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ElekMinigame : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject _panel;
    [SerializeField] private Image indicator;
    [SerializeField] private TextMeshProUGUI label;
    [SerializeField] private TextMeshProUGUI displayLvl;
    [SerializeField] private TextMeshProUGUI displayExp;
    [SerializeField] private TextMeshProUGUI TimeNow;
    [SerializeField] private TextMeshProUGUI MinigameName;  

    [Header("Gameplay")]
    [SerializeField] private GameObject _player;
    [SerializeField] private int baseGain = 25;
    [SerializeField] private float minWait = 1f;
    [SerializeField] private float maxWait = 3f;
    [SerializeField] private int knowledgeIndex = 4;

    [Header("keys")]
    public KeyCode exit;
    public KeyCode CURRENT_KEY;


    private Player _playerScript;
    private bool _ready;
    private float _goTime;

    // obsługa wyłączania skryptów i ruchu
    private bool minigameActive = false;
    private List<MonoBehaviour> previouslyDisabled = new List<MonoBehaviour>();
    private Movement playerMovement;
    private Rigidbody2D playerRb;

    private IEnumerator Start()
    {
        _playerScript = _player.GetComponent<Player>();
        playerMovement = _player.GetComponent<Movement>();
        playerRb = _player.GetComponent<Rigidbody2D>();

        yield return null;

        indicator.color = Color.red;
        label.text = $"REAGUJ '{CURRENT_KEY}'";

        UpdateHud();

        StartCoroutine(WaitAndGo());
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
            if (_ready)
            {
                float reaction = UnityEngine.Time.time - _goTime;
                int gain = Mathf.Max(1, baseGain - Mathf.RoundToInt(reaction * baseGain));

                _playerScript.exams_knowledge[knowledgeIndex] += gain;
                UpdateHud();
                StartCoroutine(WaitAndGo());     // kolejny cykl
            }
            else                                 // przedwczesny klik
            {
                StopAllCoroutines();
                StartCoroutine(WaitAndGo());
            }
        }
    }

    private IEnumerator WaitAndGo()
    {
        _ready = false;
        indicator.color = Color.red;
        yield return new WaitForSeconds(Random.Range(minWait, maxWait));

        _goTime = UnityEngine.Time.time;
        _ready = true;
        indicator.color = Color.green;
    }

    private void UpdateHud()
    {
        var result = _playerScript.LvlIncrease(_playerScript.exams_knowledge[knowledgeIndex]);
        displayLvl.text = $"{result.lvl}";
        displayExp.text = $"{result.exp} / {result.divide}";

        TimeNow.text = Time.Time_now;
        MinigameName.text = "Elektrotechnika";
    }

    /* ---------- wylaczanie innych UI i ruchu ---------- */
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
