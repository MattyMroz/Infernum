using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class PauseGame : MonoBehaviour
{
    [SerializeField] private GameObject _pauseMenuUI;
    [SerializeField] KeyCode pauseButton;

    [SerializeField] private GameObject player1;
    [SerializeField] private GameObject player2;


    private void Update()
    {
        if(Input.GetKeyDown(pauseButton))
        {
            if (Time.timeStarted == true)
            {
                player1.GetComponent<Movement>().enabled = false;
                player2.GetComponent<Movement>().enabled = false; 
                
                player1.GetComponent<Movement>().ResetVelocity();
                player2.GetComponent<Movement>().ResetVelocity();

                Time.timeStarted = false;
                _pauseMenuUI.SetActive(true);
            }
            else
            {
                player1.GetComponent<Movement>().enabled = true;
                player2.GetComponent<Movement>().enabled = true;

                Time.timeStarted = true;
                _pauseMenuUI.SetActive(false);
            }
        }
    }
}
