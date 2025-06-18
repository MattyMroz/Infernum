using JetBrains.Annotations;
using System.Collections;
using UnityEngine;

public class Trash : Interactable
{

    private void Awake()
    {
        displayName = "Kosz na œmieci";
    }

    public override void React(GameObject player)
    {
        Player playerScript = player.GetComponent<Player>();

        if (this.used[playerScript.id])
            return;

        playerScript.SearchTrash();

        this.used[playerScript.id] = true;
    }
}
