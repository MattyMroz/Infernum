using UnityEngine;

public class Chinese : Interactable
{
    public override void React(GameObject player)
    {
        player.GetComponent<Player>().IncreaseEndurance(10);
    }
}
