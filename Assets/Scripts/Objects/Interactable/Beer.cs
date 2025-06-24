using System.Collections;
using UnityEngine;

public class Beer : Interactable
{
    [SerializeField] int wisdom_decrease;
    [SerializeField] int endurance_increase;
    [SerializeField] int cost = 5;

    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip errorAudio;
    [SerializeField] AudioClip useAudio;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public override void React(GameObject playerObj)
    {
        Player player = playerObj.GetComponent<Player>();

        if (player.accountBalance >= cost)
        {
            player.SpendMoney(cost);
            player.DrinkBeer(wisdom_decrease, endurance_increase);

            audioSource.clip = useAudio;
            audioSource.PlayOneShot(audioSource.clip);
            StartCoroutine(Wait(1, player.id));
        }
        else
        {
            audioSource.clip = errorAudio;
            audioSource.PlayOneShot(audioSource.clip);
            Debug.Log($"Gracz {player.id} nie ma wystarczająco pieniędzy na piwo.");
        }
    }
}
