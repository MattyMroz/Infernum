using TMPro;
using UnityEngine;

public class MathMinigame : MonoBehaviour
{
    [SerializeField] private GameObject _panel;
    [SerializeField] TextMeshProUGUI displayNumber;
    [SerializeField] TMP_InputField inputField;

    [SerializeField] private GameObject _player;



    private int randomNumber;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GenerateNumber();
        inputField.onEndEdit.AddListener(CheckInput);
        inputField.ActivateInputField();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void GenerateNumber()
    {
        // Generate a random number between 100000000 and 999999999
        randomNumber = Random.Range(100000000, 999999999);

        // Display the number on the panel
        displayNumber.text = randomNumber.ToString();
    }

    void CheckInput(string userInput)
    {
        if (inputField != null)
        {
            inputField.text = userInput;
        }
        // Check if the input is correct
        if (int.TryParse(userInput, out int inputNumber))
        {
            if (inputNumber == randomNumber)
            {
                // Correct answer
                Debug.Log("Correct!");
                //_panel.SetActive(false);
                //_player.GetComponent<Player>().EndMinigame();
            }
            else
            {
                // Incorrect answer
                Debug.Log("Incorrect! Try again.");
                GenerateNumber();
            }
        }
        else
        {
            Debug.Log("Invalid input! Please enter a number.");
        }

    }
}
