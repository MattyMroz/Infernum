using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseMinigame : Interactable
{
    [SerializeField] protected GameObject[] playerUI;

    protected GameObject panel;
    protected Player player;
    protected KeyCode exitKey;

    protected Movement playerMovement;
    protected Rigidbody2D playerRb;
    protected bool active;

    private readonly List<MonoBehaviour> disabled = new();
    private Coroutine enduranceCoroutine;

    public void Boot(GameObject playerGO, MinigameConfig cfg)
    {
        player = playerGO.GetComponent<Player>();
        panel = cfg.panel;
        exitKey = cfg.exitKey;

        playerMovement = playerGO.GetComponent<Movement>();
        playerRb = playerGO.GetComponent<Rigidbody2D>();

        OnBoot(cfg);
        Open();
    }

    private void Start()
    {
        displayName = gameObject.name;
    }

    protected virtual void Update()
    {
        if (active && !panel.activeSelf)
        {
            Debug.LogWarning("Panel został wyłączony poza systemem. Zamykanie minigry...");
            Close();
            playerMovement.ResetVelocity();
            playerMovement.enabled = false;

            ToggleUI(player, false);
        }

        if (Input.GetKeyDown(exitKey) && active)
            Close();
    }

    protected void Open()
    {
        active = true;
        ToggleUI(player, false); 
        panel.SetActive(true);

        if (playerMovement) { playerMovement.ResetVelocity(); playerMovement.enabled = false; }
        if (playerRb)
        {
            playerRb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
            playerRb.bodyType = RigidbodyType2D.Dynamic;
        }
        enduranceCoroutine = StartCoroutine(DrainEndurance());
        OnOpen();
    }

    protected void Close()
    {
        active = false;
        ToggleUI(player, true); 
        panel.SetActive(false);

        if (playerMovement) { playerMovement.ResetVelocity(); playerMovement.enabled = true; }
        if (playerRb)
        {
            playerRb.constraints = RigidbodyConstraints2D.FreezeRotation;
            playerRb.bodyType = RigidbodyType2D.Dynamic;
        }
        if (enduranceCoroutine != null)
            StopCoroutine(enduranceCoroutine);

        OnClose();
    }

    protected void ToggleUI(Player p, bool state)
    {
        if (playerUI == null || p.id < 0 || p.id >= playerUI.Length) return;
        if (playerUI[p.id] == null) return;

        foreach (var mb in playerUI[p.id].GetComponentsInChildren<MonoBehaviour>(true))
        {
            if (mb == this) continue;                      

            if (mb.transform.IsChildOf(panel.transform))      
                continue;
            Transform t = mb.transform;
            while (t != null)
            {
                if (t.name == "Minigames") { mb.enabled = true; goto next; }
                t = t.parent;
            }

            // 2️⃣  zwykła obsługa włącz / wyłącz
            if (state)
            {
                if (!mb.enabled) mb.enabled = true;
            }
            else
            {
                if (mb.enabled)
                {
                    mb.enabled = false;
                    disabled.Add(mb);
                }
            }
        next:;
        }

        if (state) disabled.Clear();
    }



    private IEnumerator DrainEndurance()
    {
        while (active && player != null)
        {
            yield return new WaitForSeconds(5f);

            player.DecreaseEndurance(1);

            if (player.Endurance <= 0)
            {
                Close();
                yield break;
            }
        }
    }

    protected virtual void OnBoot(MinigameConfig cfg) { }
    protected virtual void OnOpen() { }
    protected virtual void OnClose() { }

}
