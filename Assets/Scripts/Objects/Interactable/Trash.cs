using JetBrains.Annotations;
using System.Collections;
using UnityEngine;

public class Trash : Interactable
{

    [SerializeField] AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        displayName = "Kosz na œmieci";
    }

    public override void React(GameObject player)
    {
        Player playerScript = player.GetComponent<Player>();

        if (this.used[playerScript.id])
            return;

        playerScript.SearchTrash();

        audioSource.PlayOneShot(audioSource.clip);

        this.used[playerScript.id] = true;
    }
}
