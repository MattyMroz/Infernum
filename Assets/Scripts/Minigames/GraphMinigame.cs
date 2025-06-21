using System.Linq;
using TMPro;
using UnityEngine;

public class GraphMinigame : BaseMinigame
{
    [System.Serializable]
    public class PlayerSlot
    {
        public Player player;
        public GameObject panel;
        public KeyCode exitKey;
        public KeyCode[] keys = new KeyCode[10];   // klawisze 0-9
    }

    [Header("Players (P1 = 0, P2 = 1)")]
    [SerializeField] private PlayerSlot[] slots = new PlayerSlot[2];

    [Header("Gameplay")]
    [SerializeField] private int comboGain = 15;
    [SerializeField] private int comboLength = 3;
    private const int KNOWLEDGE_IDX = 3;       // Graphics

    private class Session
    {
        public bool active;
        public PlayerSlot slot;
        public TextMeshProUGUI playerName, day, keysTxt, lvl, exp, time, name;
        public KeyCode[] currentCombo;
        public bool comboReady;
        public float enduranceTimer = 5f;
    }

    private readonly Session[] sessions = { new Session(), new Session() };

    private void Awake()
    {
        displayName = "Grafika";
    }

    public override void React(GameObject playerGO)
    {
        for (int i = 0; i < slots.Length; i++)
            if (playerGO == slots[i].player.gameObject && !sessions[i].active)
                StartSession(i);
    }

    private void StartSession(int id)
    {
        var s = sessions[id];
        s.active = true;
        s.slot = slots[id];
        s.enduranceTimer = 5f;

        s.currentCombo = null;
        BindUI(s);
        GenerateCombo(s);
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

    protected override void Update()
    {
        for (int i = 0; i < sessions.Length; i++)
        {
            var s = sessions[i];
            if (!s.active) continue;

            UpdateHud(s);

            // Wyjście z minigry
            if (Input.GetKeyDown(s.slot.exitKey))
            {
                EndSession(i);
                continue;
            }

            // Odliczanie do spadku wytrzymałości
            s.enduranceTimer -= UnityEngine.Time.deltaTime;
            if (s.enduranceTimer <= 0f)
            {
                s.slot.player.DecreaseEndurance(1);
                s.enduranceTimer = 5f;

                if (s.slot.player.Endurance <= 0)
                {
                    EndSession(i);
                    continue;
                }
            }

            // Oczekiwanie na puszczenie klawiszy
            if (!s.comboReady)
            {
                if (!s.currentCombo.Any(Input.GetKey))
                {
                    s.comboReady = true;
                }
                continue;
            }

            // Kompletna sekwencja
            if (s.currentCombo.All(Input.GetKey) && s.currentCombo.Any(Input.GetKeyDown))
            {
                s.slot.player.exams_knowledge[KNOWLEDGE_IDX] += comboGain;
                UpdateHud(s);
                GenerateCombo(s);
                s.comboReady = false;
            }
        }
    }

    /* ---------- helpers ---------- */
    private void BindUI(Session s)
    {
        var t = s.slot.panel.transform;
        s.playerName = t.Find("PlayerName").GetComponent<TextMeshProUGUI>();
        s.day = t.Find("Day").GetComponent<TextMeshProUGUI>();
        s.keysTxt = t.Find("DisplayKeys").GetComponent<TextMeshProUGUI>();
        s.lvl = t.Find("DisplayLvl").GetComponent<TextMeshProUGUI>();
        s.exp = t.Find("DisplayExp").GetComponent<TextMeshProUGUI>();
        s.time = t.Find("Time").GetComponent<TextMeshProUGUI>();
        s.name = t.Find("MinigameName").GetComponent<TextMeshProUGUI>();
        s.name.text = "Grafika";
    }

    private void GenerateCombo(Session s)
    {
        s.currentCombo = s.slot.keys
            .OrderBy(_ => Random.value)
            .Take(comboLength)
            .ToArray();

        s.keysTxt.text = string.Join(" + ", s.currentCombo.Select(k => k.ToString().Replace("Alpha", "").Replace("Keypad", "")));
    }

    private void UpdateHud(Session s)
    {
        var res = s.slot.player.LvlIncrease(s.slot.player.exams_knowledge[KNOWLEDGE_IDX]);
        s.lvl.text = res.lvl.ToString();
        s.exp.text = $"{res.exp} / {res.divide}";
        s.time.text = Time.Time_now;
        s.playerName.text = s.slot.player.player_name;
        s.day.text = $"Dzień: {Time.Days}";
    }

    private static void ToggleMovement(Player p, bool freeze)
    {
        var mv = p.GetComponent<Movement>();
        if (mv) { if (freeze) mv.ResetVelocity(); mv.enabled = !freeze; }

        var rb = p.GetComponent<Rigidbody2D>();
        if (rb) rb.bodyType = freeze ? RigidbodyType2D.Static : RigidbodyType2D.Dynamic;
    }

    private static void TogglePlayerHud(Player p, bool show)
    {
        foreach (var c in p.GetComponentsInChildren<MonoBehaviour>(true))
            if (c.GetType().Name.StartsWith("Display"))
                c.enabled = show;
    }
}
