using System.Collections.Generic;
using UnityEngine;

public abstract class BaseMinigame : Interactable
{
    /*  pola wspólne  */
    protected GameObject panel;
    protected Player player;
    protected KeyCode exitKey;

    protected Movement playerMovement;
    protected Rigidbody2D playerRb;
    protected bool active;

    private readonly List<MonoBehaviour> disabled = new();

    /*  wywo³uje Interactable.React()  */
    public void Boot(GameObject playerGO, MinigameConfig cfg)
    {
        player = playerGO.GetComponent<Player>();
        panel = cfg.panel;
        exitKey = cfg.exitKey;

        playerMovement = playerGO.GetComponent<Movement>();
        playerRb = playerGO.GetComponent<Rigidbody2D>();

        OnBoot(cfg);          // hook dla pochodnych
        Open();
    }

    /* ====== baza otwierania / zamykania ====== */
    protected virtual void Update()
    {
        if (Input.GetKeyDown(exitKey) && active)
            Close();
    }
    protected void Open()
    {
        active = true;
        ToggleUI(false);
        panel.SetActive(true);

        if (playerMovement) { playerMovement.ResetVelocity(); playerMovement.enabled = false; }
        if (playerRb) playerRb.bodyType = RigidbodyType2D.Static;


        OnOpen();
    }
    protected void Close()
    {
        active = false;
        ToggleUI(true);
        panel.SetActive(false);

        if (playerMovement) { playerMovement.ResetVelocity(); playerMovement.enabled = true; }
        if (playerRb) playerRb.bodyType = RigidbodyType2D.Dynamic;

        OnClose();
    }
    private void ToggleUI(bool state)
    {
        disabled.Clear();

        foreach (var s in player.GetComponentsInChildren<MonoBehaviour>(true))
        {
            if (s == this) continue;
            if (panel != null && s.transform.IsChildOf(panel.transform)) continue;

            if (s.GetType().Name.StartsWith("Display"))
            {
                if (state)            // przywracamy
                {
                    if (disabled.Contains(s)) s.enabled = true;
                }
                else                  // wy³¹czamy
                {
                    if (s.enabled)
                    {
                        s.enabled = false;
                        disabled.Add(s);
                    }
                }
            }
        }
    }

    /* ====== wirtualne hooki ====== */
    protected virtual void OnBoot(MinigameConfig cfg) { }
    protected virtual void OnOpen() { }
    protected virtual void OnClose() { }
}
