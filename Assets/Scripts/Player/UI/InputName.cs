using UnityEngine;
using TMPro;

public class InputName : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI greetingText;

    [SerializeField] private GameObject player;

    private string enteredName;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void OnNameEntered(string input)
    {

        enteredName = input;


        greetingText.text = "Witaj, " + enteredName + "!";

           
        Player playerScript = player.GetComponent<Player>();

        playerScript.player_name = enteredName;
    }
}
