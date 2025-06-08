using UnityEngine;

public class MinigameLauncher : Interactable
{
    public BaseMinigame minigame;     // wska¿ konkretn¹ klasê (MathMinigame itd.)
    public MinigameID id;           // która to minigra?

    public override void React(GameObject playerGO)
    {
        var p = playerGO.GetComponent<Player>();
        var cfg = p.GetConfig(id);
        minigame.Boot(playerGO, cfg);
    }
}
