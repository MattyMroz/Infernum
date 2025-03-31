using UnityEngine;

public class Chinese : Interactable
{

    [SerializeField] int endurance_increase;
    public override void React(GameObject player)
    {
        player.GetComponent<Player>().IncreaseEndurance(endurance_increase);
    }
}
