using UnityEngine;

public class Teleport : Interactable
{

    [SerializeField] Transform other;

    public override void React(GameObject player)
    {
        player.transform.position = other.position;
        player.GetComponent<Player>().DecreaseEndurance(1);
    }
}
