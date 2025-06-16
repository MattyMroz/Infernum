using UnityEngine;

public class ExitGame : MonoBehaviour
{
    public void Quit()
    {
        Application.Quit();
        Debug.Log("Gra zosta³a zamkniêta.");
    }
}
