using TMPro;
using UnityEngine;

public class DisplayStats : MonoBehaviour
{
    //Text field to display
    [SerializeField] private TextMeshProUGUI statsText;

    [SerializeField] private GameObject panel;

    // player script
    [SerializeField] private Player player;

    public KeyCode display;

    

    private void Update()
    {
        if (Input.GetKey(display))
        {
            statsText.gameObject.SetActive(true);
            panel.SetActive(true);

            statsText.text =
                "Wisdom: " + player.Wisdom + "\n" +
                "Hunger: " + player.Hunger + "\n" +
                "Endurance: " + player.Endurance;
        }
        else
        {
            statsText.gameObject.SetActive(false);
            panel.SetActive(false);
        }
    }
}
