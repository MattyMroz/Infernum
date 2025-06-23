/* ---------------- GraphMinigame ---------------- */
using System.Linq;
using TMPro;
using UnityEngine;

public class GraphMinigame : BaseMinigame
{
    [System.Serializable] public class PlayerSlot { 
        public Player player;
        public GameObject panel;
        public KeyCode exitKey; 
        public KeyCode[] keys = new KeyCode[10];
    }

    [SerializeField] private PlayerSlot[] slots = new PlayerSlot[2];
    [SerializeField] private int comboGain = 15, comboLength = 3;
    private const int KNOW_IDX = 3;

    private class Local
    {
        public bool active, ready;
        public KeyCode[] combo;
        public TextMeshProUGUI keysTxt, lvl, exp, time, day;  // ← ➊
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

        Boot(slot.player.gameObject, slot.player.GetConfig(MinigameID.Graph));

        var t = slot.panel.transform;
        loc.keysTxt = t.Find("DisplayKeys").GetComponent<TextMeshProUGUI>();
        loc.lvl = t.Find("DisplayLvl").GetComponent<TextMeshProUGUI>();
        loc.exp = t.Find("DisplayExp").GetComponent<TextMeshProUGUI>();
        loc.time = t.Find("Time").GetComponent<TextMeshProUGUI>();
        loc.day = t.Find("Day").GetComponent<TextMeshProUGUI>();       // ← ➋

        loc.active = true;
        GenerateCombo(i);
        UpdateHud(i);
        ToggleUI(slot.player, false);
    }

    private void EndSession(int i) { 
        ToggleUI(slots[i].player, true);
        if (l[i].active) {
            cur = i;
            Close();
        }
    }

    protected override void OnClose() {
        l[cur].active = false;
    }

    protected override void Update()
    {
        base.Update();
        for (int i = 0; i < slots.Length; i++)
        {
            if (!l[i].active) continue;
            UpdateHud(i);

            var slot = slots[i]; 
            var loc = l[i];

            if (Input.GetKeyDown(slot.exitKey)) { 
                EndSession(i); 
                continue;
            }

            if (!loc.ready)
            {
                if (!loc.combo.Any(Input.GetKey)) loc.ready = true;
                continue;
            }
            if (loc.combo.All(Input.GetKey) && loc.combo.Any(Input.GetKeyDown))
            {
                slot.player.exams_knowledge[KNOW_IDX] += comboGain;
                GenerateCombo(i); 
                loc.ready = false;
            }
        }
    }

    private void GenerateCombo(int i)
    {
        l[i].combo = slots[i].keys.OrderBy(_ => Random.value).Take(comboLength).ToArray();
        l[i].keysTxt.text = string.Join(" + ", l[i].combo.Select(k => k.ToString().Replace("Alpha", "").Replace("Keypad", "")));
    }

    private void UpdateHud(int i)
    {
        var p = slots[i].player;
        var r = p.LvlIncrease(p.exams_knowledge[KNOW_IDX]);

        l[i].lvl.text = r.lvl.ToString();
        l[i].exp.text = $"{r.exp}/{r.divide}";
        l[i].time.text = Time.Time_now;
        l[i].day.text = $"Dzień: {Time.Days}";             // ← ➌
    }
}
