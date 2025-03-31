using UnityEngine;

public class Teleport : Interactable
{

    [SerializeField] Transform other;

    public override void React(GameObject player)
    {
        player.transform.position = other.position;
        player.GetComponent<Player>().DecreaseEndurance(1);

        StartCoroutine(Wait(1));
        Interactable other_interactable = other.GetComponent<Interactable>();
        other_interactable.StartCoroutine(other_interactable.Wait(1));
    }
}
