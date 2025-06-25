/* ---------------- ProgMinigame ---------------- */
using TMPro;
using UnityEngine;

public class ProgMinigame : BaseMinigame
{
    [System.Serializable] public class PlayerSlot { public Player player; public GameObject panel; public KeyCode exitKey; public KeyCode actionKey; }

    [SerializeField] private PlayerSlot[] slots = new PlayerSlot[2];
    [SerializeField] private int keyGain = 1;
    private const int KNOW_IDX = 2;

    private class Local
    {
        public bool active;
        public TextMeshProUGUI lvl, exp, time, day, name;          // ← ➊
    }
    
    private readonly Local[] l = { new Local(), new Local() };
    private int cur;

    public override void React(GameObject go)
    {
        for (int i = 0; i < slots.Length; i++)
            if (go == slots[i].player.gameObject && !l[i].active) StartSession(i);
    }

    private void StartSession(int i)
    {
        cur = i;
        var slot = slots[i];
        var loc = l[i];

        Boot(slot.player.gameObject, slot.player.GetConfig(MinigameID.Prog));

        var t = slot.panel.transform;
        loc.name = t.Find("PlayerName").GetComponent<TextMeshProUGUI>();
        loc.lvl = t.Find("DisplayLvl").GetComponent<TextMeshProUGUI>();
        loc.exp = t.Find("DisplayExp").GetComponent<TextMeshProUGUI>();
        loc.time = t.Find("Time").GetComponent<TextMeshProUGUI>();
        loc.day = t.Find("Day").GetComponent<TextMeshProUGUI>();          // ← ➋

        loc.active = true;
        UpdateHud(i);
        ToggleUI(slot.player, false);
    }

    private void EndSession(int i) { ToggleUI(slots[i].player, true); if (l[i].active) { cur = i; Close(); } }

    protected override void OnClose() { l[cur].active = false; }

    protected override void Update()
    {
        base.Update();
        for (int i = 0; i < slots.Length; i++)
        {
            if (!l[i].active) continue;
            UpdateHud(i);
            if (Input.GetKeyDown(slots[i].exitKey)) { EndSession(i); continue; }
            if (Input.GetKeyDown(slots[i].actionKey)) slots[i].player.exams_knowledge[KNOW_IDX] += keyGain;
        }
    }

    private void UpdateHud(int i)
    {
        var p = slots[i].player;
        var r = p.LvlIncrease(p.exams_knowledge[KNOW_IDX]);

        l[i].name.text = p.name;
        l[i].lvl.text = r.lvl.ToString();
        l[i].exp.text = $"{r.exp}/{r.divide}";
        l[i].time.text = Time.Time_now;
        l[i].day.text = $"Dzień: {Time.Days + 1}"; 
    }
}
