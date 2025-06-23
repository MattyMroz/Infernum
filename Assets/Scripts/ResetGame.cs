using UnityEngine;

public class ResetGame : MonoBehaviour
{
    [SerializeField] Player[] players;
    [SerializeField] Interactable[] interactableObjects;
    [SerializeField] Exam[] exams;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Reset()
    {

        for(int i = 0; i < players.Length; i++) {
            players[i].Reset();
        }

        for(int i = 0; i < interactableObjects.Length; i++)
        {
            interactableObjects[i].Reset();
        }
        
        for (int i = 0;i < exams.Length; i++)
        {
            exams[i].Reset();
        }
    }
}
