using System.Collections;
using UnityEngine;

public class Beer : Interactable
{
    [SerializeField] int wisdom_decrease;
    [SerializeField] int endurance_increase;
    [SerializeField] int cost = 5;

    private void Awake()
    {
        displayName = "Piwo";
    }

    public override void React(GameObject playerObj)
    {
        Player player = playerObj.GetComponent<Player>();

        if (player.accountBalance >= cost)
        {
            player.SpendMoney(cost);
            player.DrinkBeer(wisdom_decrease, endurance_increase);
            StartCoroutine(Wait(1, player.id));
        }
        else
        {
            Debug.Log($"Gracz {player.id} nie ma wystarczająco pieniędzy na piwo.");
        }
    }
}
