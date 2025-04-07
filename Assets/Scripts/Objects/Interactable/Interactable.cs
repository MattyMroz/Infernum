using System.Collections;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public bool[] used = new bool[2];

    public virtual void React()
    {
        return;
    }

    public virtual void React(GameObject player)
    {
        return;
    }

    public IEnumerator Wait(int seconds, int player_id)
    {
        used[player_id] = true;
        yield return new WaitForSeconds(seconds);
        used[player_id] = false;
    }
}
