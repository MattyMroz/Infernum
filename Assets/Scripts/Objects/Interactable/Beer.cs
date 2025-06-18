using JetBrains.Annotations;
using System.Collections;
using UnityEngine;

public class Beer : Interactable
{
    [SerializeField] int wisdom_decrease;
    [SerializeField] int endurance_increase;


    private void Awake()
    {
        displayName = "Piwo";
    }


    public override void React(GameObject player)
    {
        player.GetComponent<Player>().DrinkBeer(wisdom_decrease, endurance_increase);

        StartCoroutine(Wait(1, player.GetComponent<Player>().id));
    }
}
