using UnityEngine;

// TODO: Dodac inputy na rozne akcje
public class InputManager : MonoBehaviour
{

    [Header("Inputs")]
    public KeyCode key_interact;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        if (Input.GetKey(key_interact))
            return; // do smth
    }
}
