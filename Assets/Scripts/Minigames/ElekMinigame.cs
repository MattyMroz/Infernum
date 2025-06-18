using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ElekMinigame : BaseMinigame
{
    [System.Serializable]
    public class PlayerSlot
    {
        public Player player;
        public GameObject panel;
        public KeyCode exitKey;
        public KeyCode actionKey;
    }

    [Header("Players (P1 = 0, P2 = 1)")]
    [SerializeField] private PlayerSlot[] slots = new PlayerSlot[2];

    [Header("Gameplay")]
    [SerializeField] private int baseGain = 25;
    [SerializeField] private float minWait = 1f;
    [SerializeField] private float maxWait = 3f;
    [SerializeField] private float nextRoundDelay = 0.5f;
    private const int KNOWLEDGE_IDX = 4;        // Electrotechnics

    private class Session
    {
        public bool active;
        public PlayerSlot slot;
        public Image indicator;
        public TextMeshProUGUI playerName, day, label, lvl, exp, time, name;
        public bool ready;
        public float goTime;
        public bool reacted;
        public Coroutine waitCoroutine;
    }

    private readonly Session[] sessions = { new Session(), new Session() };

    private void Awake()
    {
        displayName = "Elektrotechnika";
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
        BindUI(s);

        s.name.text = "Elektrotechnika";
        s.playerName.text = s.slot.player.player_name;
        s.day.text = $"Dzień: {Time.Days}";
        s.label.text = $"REAGUJ '{s.slot.actionKey}'";
        s.indicator.color = Color.red;

        ToggleMovement(s.slot.player, true);
        TogglePlayerHud(s.slot.player, false);
        s.slot.panel.SetActive(true);

        if (s.waitCoroutine != null)
            StopCoroutine(s.waitCoroutine);

        s.waitCoroutine = StartCoroutine(WaitAndGo(s));

        UpdateHud(s);
    }

    private void EndSession(int id)
    {
        var s = sessions[id];
        s.active = false;
        if (s.waitCoroutine != null)
        {
            StopCoroutine(s.waitCoroutine);
            s.waitCoroutine = null;
        }
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

            if (Input.GetKeyDown(s.slot.exitKey))
            {
                EndSession(i);
                continue;
            }

            if (Input.GetKeyDown(s.slot.actionKey))
            {
                if (s.ready && !s.reacted)
                {
                    float reaction = UnityEngine.Time.time - s.goTime;
                    int gain = Mathf.Max(1, baseGain - Mathf.RoundToInt(reaction * baseGain));
                    s.slot.player.exams_knowledge[KNOWLEDGE_IDX] += gain;
                    s.reacted = true;
                    s.ready = false;
                    UpdateHud(s);
                }
                else if (!s.ready)
                {
                    if (s.waitCoroutine != null)
                        StopCoroutine(s.waitCoroutine);

                    s.waitCoroutine = StartCoroutine(WaitAndGo(s));
                }
            }
        }
    }


    private IEnumerator WaitAndGo(Session s)
    {
        s.ready = false;
        s.reacted = false;
        s.indicator.color = Color.red;

        yield return new WaitForSeconds(Random.Range(minWait, maxWait));

        s.goTime = UnityEngine.Time.time;
        s.ready = true;
        s.indicator.color = Color.green;

        float timeout = 3f;
        float waitUntil = UnityEngine.Time.time + timeout;

        while (s.ready && !s.reacted && UnityEngine.Time.time < waitUntil)
            yield return null;

        yield return new WaitForSeconds(nextRoundDelay);

        s.waitCoroutine = StartCoroutine(WaitAndGo(s));
    }


    /* ---------- helpers ---------- */
    private void BindUI(Session s)
    {
        var t = s.slot.panel.transform;
        s.playerName = t.Find("PlayerName").GetComponent<TextMeshProUGUI>();
        s.day = t.Find("Day").GetComponent<TextMeshProUGUI>();
        s.indicator = t.Find("Image").GetComponent<Image>();
        s.label = t.Find("DisplayKeys").GetComponent<TextMeshProUGUI>();
        s.lvl = t.Find("DisplayLvl").GetComponent<TextMeshProUGUI>();
        s.exp = t.Find("DisplayExp").GetComponent<TextMeshProUGUI>();
        s.time = t.Find("Time").GetComponent<TextMeshProUGUI>();
        s.name = t.Find("MinigameName").GetComponent<TextMeshProUGUI>();
    }

    private void UpdateHud(Session s)
    {
        var res = s.slot.player.LvlIncrease(s.slot.player.exams_knowledge[KNOWLEDGE_IDX]);
        s.lvl.text = res.lvl.ToString();
        s.exp.text = $"{res.exp} / {res.divide}";
        s.time.text = Time.Time_now;
    }

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
