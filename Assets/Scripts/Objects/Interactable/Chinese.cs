using UnityEngine;

public class Chinese : Interactable
{
    [SerializeField] int increase_endurance;

    private void Awake()
    {
        displayName = "Chiñskie";
    }

    public override void React(GameObject player)
    {
        player.GetComponent<Player>().IncreaseEndurance(increase_endurance);

        StartCoroutine(Wait(1, player.GetComponent<Player>().id));
    }
}
