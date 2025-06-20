using UnityEngine;

public class Teleport : Interactable
{

    [SerializeField] Transform other;

    private void Awake()
    {
        displayName = "Przejœcie";
    }

    public override void React(GameObject player)
    {
        player.transform.position = other.position;


        StartCoroutine(Wait(1, player.GetComponent<Player>().id));

        // Make sure to disable interacting with the other teleport
        Interactable other_interactable = other.GetComponent<Interactable>();
        other_interactable.StartCoroutine(other_interactable.Wait(1, player.GetComponent<Player>().id));
    }
}
