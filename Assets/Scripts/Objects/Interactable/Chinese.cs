using UnityEngine;

public class Chinese : Interactable
{
    [SerializeField] int increase_endurance;
    public override void React(GameObject player)
    {
        player.GetComponent<Player>().IncreaseEndurance(increase_endurance);
    }
}
