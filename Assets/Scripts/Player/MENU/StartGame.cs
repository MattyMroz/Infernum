using UnityEngine;

public class StartGame : MonoBehaviour
{
    [SerializeField] private GameObject menuPanel; // np. ekran startowy
    [SerializeField] private GameObject player1;    // Twój gracz
    [SerializeField] private GameObject player2;

    [SerializeField] private GameObject player1_spawnpoint;
    [SerializeField] private GameObject player2_spawnpoint;

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
        player1.transform.position = player1_spawnpoint.transform.position;
        player2.transform.position = player2_spawnpoint.transform.position;

        player1.transform.rotation = player1_spawnpoint.transform.rotation;
        player2.transform.rotation = player2_spawnpoint.transform.rotation;


        menuPanel.SetActive(false);
        player1.GetComponent<Movement>().enabled = true;
        player1.GetComponent<InputManager>().enabled = true;
        player1.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;


        player2.GetComponent<Movement>().enabled = true;
        player2.GetComponent<InputManager>().enabled = true;
        player2.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;

        //Time.ResumeTime();
        UnityEngine.Time.timeScale = 1f;
    }
}
