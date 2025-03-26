using UnityEngine;

public class Teleport : Interactable
{

    [SerializeField] Transform other;

    public override void React(Transform player)
    {
        base.React(player);

        player.position = other.position;
    }
}
