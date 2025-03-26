using UnityEngine;

// TODO: Dodac inputy na rozne akcje
public class InputManager : MonoBehaviour
{

    [Header("Inputs")]
    public KeyCode key_interact;

    [Header("Layers")]
    public LayerMask layer_mask;

    private Player _player_script;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _player_script = GetComponent<Player>();
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

            hit = Physics2D.CircleCast(transform.position, 2.5f, Vector2.zero, 1f, layer_mask);
            if (hit)
            {
                if (_player_script.Endurance <= 0) return;

                Interactable interact_script = hit.transform.GetComponent<Interactable>();

                Debug.Log(hit.transform.tag);

                switch (hit.transform.tag)
                {
                    case "Teleport":

                        interact_script.React(transform.gameObject);

                        break;
                    default:
                        interact_script.React(transform.gameObject);
                        break;
                }
            }
        }
            
    }
}
