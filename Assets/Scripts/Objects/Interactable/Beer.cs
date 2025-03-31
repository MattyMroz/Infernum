using UnityEngine;

public class Beer : Interactable
{
    [SerializeField] int endurance_increase;
    [SerializeField] int wisdom_decrease;

    public override void React(GameObject player)
    {
        player.GetComponent<Player>().DrinkBeer(wisdom_decrease,endurance_increase);
    }
}
