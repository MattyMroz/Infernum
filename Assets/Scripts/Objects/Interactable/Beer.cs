using UnityEngine;

public class Beer : Interactable
{
    public override void React(GameObject player)
    {
        player.GetComponent<Player>().DrinkBeer();
    }
}
