using UnityEngine;

public class Interactable : MonoBehaviour
{
    public virtual void React()
    {
        Debug.Log("yuh");
    }

    public virtual void React(Transform player)
    {

    }
}
