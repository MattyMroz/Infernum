using System.Collections;
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

    [Header("Gameplay")]
    [SerializeField] private GameObject _player;
    [SerializeField] private int mathGain = 10;

  
    private int randomNumber;
    private Player _playerScript;


    private IEnumerator Start()
    {
        _playerScript = _player.GetComponent<Player>();

        inputField.contentType = TMP_InputField.ContentType.IntegerNumber;
        inputField.characterValidation = TMP_InputField.CharacterValidation.Digit;
        inputField.onValidateInput += OnlyDigits;

        yield return null;

        GenerateNumber();

        inputField.onEndEdit.AddListener(CheckInput);
        inputField.ActivateInputField();          
    }

    void Update()
    {
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
            _playerScript.exams_knowledge[0] += mathGain;
            UpdateHud();
        }

        GenerateNumber();

        inputField.ActivateInputField();

    }

    private char OnlyDigits(string text, int index, char addedChar) => char.IsDigit(addedChar) ? addedChar : '\0';

    private void UpdateHud()
    {
        int lvl = _playerScript.exams_knowledge[0] / 100;
        DisplayLvl.text = $"{lvl}";
        int exp = _playerScript.exams_knowledge[0] % 100;
        DisplayExp.text = $"{exp} / 100";
    }

    public void FocusInput() => inputField.ActivateInputField();
}
