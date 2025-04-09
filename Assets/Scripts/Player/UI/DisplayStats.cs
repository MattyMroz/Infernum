using TMPro;
using UnityEngine;

public class DisplayStats : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI statsText;
    [SerializeField] private GameObject panel;

    // player script
    [SerializeField] private GameObject player;
    private Player _player_script;
    private Rigidbody2D _rb;

    public KeyCode display;

    private void Start()
    {
        _player_script = player.GetComponent<Player>();
        _rb = player.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Display();
    }


    // Nadpisuje wylaczanie movementu w DisplayExams - ?
    private void Display()
    {
        if (Input.GetKey(display))
        {
            player.GetComponent<Movement>().ResetVelocity();
            player.GetComponent<Movement>().enabled = false;
            _rb.bodyType = RigidbodyType2D.Static;

            panel.SetActive(true);

            statsText.text =
                "Wisdom: " + _player_script.Wisdom + "\n" +
                "Hunger: " + _player_script.Hunger + "\n" +
                "Endurance: " + _player_script.Endurance;
        }
        else
        {
            player.GetComponent<Movement>().enabled = true;
            _rb.bodyType = RigidbodyType2D.Dynamic;

            panel.SetActive(false);
        }
    }
}
