using UnityEngine;

public class Beer : Interactable
{

    [SerializeField] int wisdom_decrease;
    [SerializeField] int endurance_increase;
    public override void React(GameObject player)
    {
        player.GetComponent<Player>().DrinkBeer(wisdom_decrease, endurance_increase);
    }
}
