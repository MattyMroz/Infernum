using UnityEngine;

// TODO: Dodac inputy na rozne akcje
public class InputManager : MonoBehaviour
{

    [Header("Inputs")]
    public KeyCode key_interact;

    [Header("Layers")]
    public LayerMask layer_mask;

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
        if (Input.GetKeyDown(key_interact))
        {

            RaycastHit2D hit;

            hit = Physics2D.CircleCast(transform.position, 5f, Vector2.zero, 1f, layer_mask);
            if (hit)
            {
                
                Interactable interact_script = hit.transform.GetComponent<Interactable>();

                switch (hit.transform.tag)
                {
                    case "Teleport":
                        interact_script.React(transform);
                        break;
                    default:
                        break;
                }
            }
        }
            
    }
}
