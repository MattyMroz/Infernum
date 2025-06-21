using UnityEngine;

public class StartGame : MonoBehaviour
{
    [SerializeField] private GameObject menuPanel; // np. ekran startowy
    [SerializeField] private GameObject player1;    // Twój gracz
    [SerializeField] private GameObject player2;

    private void Start()
    {
        player1.GetComponent<InputManager>().enabled = false;
        player1.GetComponent<Movement>().ResetVelocity(); player1.GetComponent<Movement>().enabled = false;
        player1.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;

        player2.GetComponent<InputManager>().enabled = false;
        player2.GetComponent<Movement>().ResetVelocity(); player2.GetComponent<Movement>().enabled = false;
        player2.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
    }

    public void StartGameNow()
    {
        menuPanel.SetActive(false);
        player1.GetComponent<Movement>().enabled = true;
        player1.GetComponent<InputManager>().enabled = true;
        player1.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;


        player2.GetComponent<Movement>().enabled = true;
        player2.GetComponent<InputManager>().enabled = true;
        player2.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;

        Time.ResumeTime();
        UnityEngine.Time.timeScale = 1f;
    }
}
