using TMPro;
using UnityEngine;

public class DisplayStats : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI statsText;
    [SerializeField] private GameObject panel;

    // player script
    [SerializeField] private GameObject player;
    private Player _player_script;

    public KeyCode display;

    private void Start()
    {
        _player_script = player.GetComponent<Player>();
    }

    private void Update()
    {
        Display();
    }



    private void Display()
    {
        if (Input.GetKey(display))
        {
            player.GetComponent<Movement>().enabled = false;

            panel.SetActive(true);

            statsText.text =
                "Wisdom: " + _player_script.Wisdom + "\n" +
                "Hunger: " + _player_script.Hunger + "\n" +
                "Endurance: " + _player_script.Endurance;
        }
        else
        {
            player.GetComponent<Movement>().enabled = true;
            panel.SetActive(false);
        }
    }
}
