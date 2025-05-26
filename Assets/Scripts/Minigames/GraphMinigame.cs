using System.Linq;
using TMPro;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GraphMinigame : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject _panel;
    [SerializeField] private TextMeshProUGUI displayKeys;
    [SerializeField] private TextMeshProUGUI displayLvl;
    [SerializeField] private TextMeshProUGUI displayExp;
    [SerializeField] private TextMeshProUGUI TimeNow;
    [SerializeField] private TextMeshProUGUI MinigameName;

    [Header("Gameplay")]
    [SerializeField] private GameObject _player;
    [SerializeField] private int comboGain = 15;
    [SerializeField] private int comboLength = 3;
    [SerializeField] private int knowledgeIndex = 3;

    [Header("keys")]
    public KeyCode exit;

    // Obs³uga blokady ruchu i UI
    private bool minigameActive = false;
    private List<MonoBehaviour> previouslyDisabled = new List<MonoBehaviour>();
    private Movement playerMovement;
    private Rigidbody2D playerRb;

    private readonly KeyCode[] _possibleKeys_Player1 =
    {
        KeyCode.Alpha0, KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4,
        KeyCode.Alpha5, KeyCode.Alpha6, KeyCode.Alpha7, KeyCode.Alpha8, KeyCode.Alpha9
    };

    private KeyCode[] _currentCombo;
    private Player _playerScript;

    private IEnumerator Start()
    {
        _playerScript = _player.GetComponent<Player>();
        playerMovement = _player.GetComponent<Movement>();
        playerRb = _player.GetComponent<Rigidbody2D>();

        yield return null;

        GenerateCombo();
        UpdateHud();
    }

    private void Update()
    {
        if (_currentCombo == null) return;

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

        bool allPressed = _currentCombo.All(Input.GetKey);
        bool anyPressedNow = _currentCombo.Any(Input.GetKeyDown);

        if (allPressed && anyPressedNow)
        {
            _playerScript.exams_knowledge[knowledgeIndex] += comboGain;
            UpdateHud();
            GenerateCombo();
        }
    }

    private void GenerateCombo()
    {
        _currentCombo = _possibleKeys_Player1
            .OrderBy(_ => Random.value)
            .Take(comboLength)
            .ToArray();

        displayKeys.text = string.Join(" + ",
            _currentCombo.Select(k => k.ToString().Replace("Alpha", "")));
    }

    private void UpdateHud()
    {
        var result = _playerScript.LvlIncrease(_playerScript.exams_knowledge[knowledgeIndex]);
        displayLvl.text = $"{result.lvl}";
        displayExp.text = $"{result.exp} / {result.divide}";

        TimeNow.text = Time.Time_now;
        MinigameName.text = "Grafika";
    }

    public void FocusInput() { }

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
