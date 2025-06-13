using TMPro;
using UnityEngine;

public class ProgMinigame : BaseMinigame
{
    [System.Serializable]
    public class PlayerSlot
    {
        public Player player;      // obiekt Player
        public GameObject panel;       // panel UI gracza
        public KeyCode exitKey;     // klawisz wyjścia
        public KeyCode actionKey;   // klawisz „klik”
    }

    [Header("Players (P1 = 0, P2 = 1)")]
    [SerializeField] private PlayerSlot[] slots = new PlayerSlot[2];

    [Header("Gameplay")]
    [SerializeField] private int keyGain = 1;
    private const int KNOWLEDGE_IDX = 2;   // Programming

    /* --------- stan jednej sesji --------- */
    private class Session
    {
        public bool active;
        public PlayerSlot slot;
        public TextMeshProUGUI playerName, day, lvl, exp, time, name;
    }

    private readonly Session[] sessions = { new Session(), new Session() };

    /* --------- React --------- */
    public override void React(GameObject playerGO)
    {
        for (int i = 0; i < slots.Length; i++)
            if (playerGO == slots[i].player.gameObject && !sessions[i].active)
            {
                StartSession(i);
                return;
            }
    }

    /* --------- Start / End --------- */
    private void StartSession(int id)
    {
        var s = sessions[id];
        s.active = true;
        s.slot = slots[id];

        BindUI(s);
        UpdateHud(s);

        ToggleMovement(s.slot.player, true);
        TogglePlayerHud(s.slot.player, false);
        s.slot.panel.SetActive(true);
    }

    private void EndSession(int id)
    {
        var s = sessions[id];
        s.active = false;
        s.slot.panel.SetActive(false);

        ToggleMovement(s.slot.player, false);
        TogglePlayerHud(s.slot.player, true);
    }

    /* --------- Update --------- */
    protected override void Update()  
    {
        for (int i = 0; i < sessions.Length; i++)
        {
            var s = sessions[i];
            if (!s.active) continue;


            UpdateHud(s);

            if (Input.GetKeyDown(s.slot.exitKey)) { EndSession(i); continue; }

            if (Input.GetKeyDown(s.slot.actionKey))
            {
                s.slot.player.exams_knowledge[KNOWLEDGE_IDX] += keyGain;
                UpdateHud(s);
            }
        }
    }

    /* --------- UI / HUD --------- */
    private void BindUI(Session s)
    {
        Transform t = s.slot.panel.transform;
        s.playerName = t.Find("PlayerName").GetComponent<TextMeshProUGUI>();
        s.day = t.Find("Day").GetComponent<TextMeshProUGUI>();
        s.lvl = t.Find("DisplayLvl").GetComponent<TextMeshProUGUI>();
        s.exp = t.Find("DisplayExp").GetComponent<TextMeshProUGUI>();
        s.time = t.Find("Time").GetComponent<TextMeshProUGUI>();
        s.name = t.Find("MinigameName").GetComponent<TextMeshProUGUI>();
        s.name.text = "Programowanie";
    }

    private void UpdateHud(Session s)
    {
        var res = s.slot.player.LvlIncrease(s.slot.player.exams_knowledge[KNOWLEDGE_IDX]);
        s.playerName.text = s.slot.player.player_name;
        s.day.text = $"Dzień: {Time.Days}";
        s.lvl.text = res.lvl.ToString();
        s.exp.text = $"{res.exp} / {res.divide}";
        s.time.text = Time.Time_now;
    }

    /* --------- helpers --------- */
    private static void ToggleMovement(Player p, bool freeze)
    {
        var mv = p.GetComponent<Movement>(); if (mv) { if (freeze) mv.ResetVelocity(); mv.enabled = !freeze; }
        var rb = p.GetComponent<Rigidbody2D>(); if (rb) rb.bodyType = freeze ? RigidbodyType2D.Static : RigidbodyType2D.Dynamic;
    }

    private static void TogglePlayerHud(Player p, bool show)
    {
        foreach (var c in p.GetComponentsInChildren<MonoBehaviour>(true))
            if (c.GetType().Name.StartsWith("Display"))
                c.enabled = show;
    }
}
