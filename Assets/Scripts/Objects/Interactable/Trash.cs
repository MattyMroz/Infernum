using JetBrains.Annotations;
using System.Collections;
using UnityEngine;

public class Trash : Interactable
{

    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip errorAudio;
    [SerializeField] AudioClip useAudio;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        displayName = "Kosz na œmieci";
    }

    public override void React(GameObject player)
    {
        Player playerScript = player.GetComponent<Player>();

        for(int i = 0; i < 2; i++)
        {
            if (this.used[i])
            {
                Debug.Log("???");
                audioSource.clip = errorAudio;
                audioSource.PlayOneShot(audioSource.clip);
                return;
            }
        }


        playerScript.SearchTrash();

        audioSource.clip = useAudio;
        audioSource.PlayOneShot(audioSource.clip);

        this.used[playerScript.id] = true;
    }
}
