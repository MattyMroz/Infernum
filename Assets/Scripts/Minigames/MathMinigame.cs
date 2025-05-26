using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class MathMinigame : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject _panel;
    [SerializeField] private TextMeshProUGUI displayNumber;
    [SerializeField] private TMP_InputField inputField;

    [SerializeField] private TextMeshProUGUI DisplayLvl;
    [SerializeField] private TextMeshProUGUI DisplayExp;
    [SerializeField] private TextMeshProUGUI TimeNow;
    [SerializeField] private TextMeshProUGUI MinigameName;

    [Header("Gameplay")]
    [SerializeField] private GameObject _player;
    [SerializeField] private int mathGain = 10;
    [SerializeField] private int knowledgeIndex = 0;

    [Header("keys")]
    public KeyCode exit;

    private int randomNumber;
    private Player _playerScript;

    // Blokada ruchu i UI
    private bool minigameActive = false;
    private List<MonoBehaviour> previouslyDisabled = new List<MonoBehaviour>();
    private Movement playerMovement;
    private Rigidbody2D playerRb;

    private IEnumerator Start()
    {
        _playerScript = _player.GetComponent<Player>();
        playerMovement = _player.GetComponent<Movement>();
        playerRb = _player.GetComponent<Rigidbody2D>();

        inputField.contentType = TMP_InputField.ContentType.IntegerNumber;
        inputField.characterValidation = TMP_InputField.CharacterValidation.Digit;
        inputField.onValidateInput += OnlyDigits;

        yield return null;

        GenerateNumber();

        inputField.onEndEdit.AddListener(CheckInput);
        inputField.ActivateInputField();
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

        if (!inputField.isFocused) return;

        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            int len = inputField.text.Length;
            inputField.caretPosition =
            inputField.selectionAnchorPosition =
            inputField.selectionFocusPosition = len;
        }
    }

    void GenerateNumber()
    {
        randomNumber = Random.Range(1_000_000, 10_000_000);
        displayNumber.text = randomNumber.ToString();

        UpdateHud();

        inputField.text = string.Empty;
    }

    void CheckInput(string userInput)
    {
        if (string.IsNullOrWhiteSpace(userInput))
            return;

        if (int.TryParse(userInput, out int inputNumber) && inputNumber == randomNumber)
        {
            _playerScript.exams_knowledge[knowledgeIndex] += mathGain;
            UpdateHud();
        }

        GenerateNumber();

        inputField.ActivateInputField();
    }

    private char OnlyDigits(string text, int index, char addedChar) => char.IsDigit(addedChar) ? addedChar : '\0';

    private void UpdateHud()
    {
        var result = _playerScript.LvlIncrease(_playerScript.exams_knowledge[knowledgeIndex]);
        DisplayLvl.text = $"{result.lvl}";
        DisplayExp.text = $"{result.exp} / {result.divide}";

        TimeNow.text = Time.Time_now;
        MinigameName.text = "Matematyka";
    }

    public void FocusInput() => inputField.ActivateInputField();

    /* ---------- wyłączanie innych UI i ruchu ---------- */
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
