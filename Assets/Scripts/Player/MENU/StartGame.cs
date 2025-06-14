using UnityEngine;

public class StartGame : MonoBehaviour
{
    [SerializeField] private GameObject menuPanel; // np. ekran startowy
    [SerializeField] private GameObject player1;    // Twój gracz
    [SerializeField] private GameObject player2;

    public void StartGameNow()
    {
        menuPanel.SetActive(false);
        player1.GetComponent<Movement>().enabled = true;
        player2.GetComponent<Movement>().enabled = true;
        Time.ResumeTime();
        UnityEngine.Time.timeScale = 1f;
    }
}
