using UnityEngine;

public class Chinese : Interactable
{
    [SerializeField] int increase_endurance;
    [SerializeField] int cost = 20;

    private void Awake()
    {
        displayName = "Chińskie";
    }

    public override void React(GameObject playerObj)
    {
        Player player = playerObj.GetComponent<Player>();

        if (player.accountBalance >= cost)
        {
            player.SpendMoney(cost);
            player.IncreaseEndurance(increase_endurance);
            StartCoroutine(Wait(1, player.id));
        }
        else
        {
            Debug.Log($"Gracz {player.id} nie ma wystarczająco pieniędzy na jedzenie Chińskie.");
        }
    }
}
