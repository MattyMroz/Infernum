using System.Collections;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public bool used;

    public virtual void React()
    {
        return;
    }

    public virtual void React(GameObject player)
    {
        return;
    }

    public IEnumerator Wait(int seconds)
    {
        used = true;
        yield return new WaitForSeconds(seconds);
        used = false;
    }
}
