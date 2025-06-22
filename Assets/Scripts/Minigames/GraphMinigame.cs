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
        public KeyCode[] keys = new KeyCode[10];
    }

    [Header("Players (P1 = 0, P2 = 1)")]
    [SerializeField] private PlayerSlot[] slots = new PlayerSlot[2];

    [Header("Gameplay")]
    [SerializeField] private int comboGain = 15;
    [SerializeField] private int comboLength = 3;
    private const int KNOW_IDX = 3; // Graphics

    /* ----- stan ----- */
    private class Local
    {
        public bool active, comboReady;
        public KeyCode[] combo;
        public TextMeshProUGUI keysTxt, lvl, exp, time;
    }
    private readonly Local[] l = { new Local(), new Local() };
    private int current;

    /* ----- React ----- */
    public override void React(GameObject go)
    {
        for (int i = 0; i < slots.Length; i++)
            if (go == slots[i].player.gameObject && !l[i].active)
                StartSession(i);
    }

    private void StartSession(int i)
    {
        current = i;
        var slot = slots[i];
        Boot(slot.player.gameObject, slot.player.GetConfig(MinigameID.Graph));

        var t = slot.panel.transform;
        l[i].keysTxt = t.Find("DisplayKeys").GetComponent<TextMeshProUGUI>();
        l[i].lvl = t.Find("DisplayLvl").GetComponent<TextMeshProUGUI>();
        l[i].exp = t.Find("DisplayExp").GetComponent<TextMeshProUGUI>();
        l[i].time = t.Find("Time").GetComponent<TextMeshProUGUI>();

        l[i].active = true;
        GenerateCombo(i);
        UpdateHud(i);
    }

    private void EndSession(int i)
    {
        if (!l[i].active) return;
        current = i; Close();
    }

    protected override void OnClose() => l[current].active = false;

    /* ----- Update ----- */
    protected override void Update()
    {
        base.Update();

        for (int i = 0; i < slots.Length; i++)
        {
            if (!l[i].active) continue;
            var slot = slots[i]; var s = l[i];

            if (Input.GetKeyDown(slot.exitKey)) { EndSession(i); continue; }

            if (!s.comboReady)
            {
                if (!s.combo.Any(Input.GetKey)) s.comboReady = true;
                continue;
            }

            if (s.combo.All(Input.GetKey) && s.combo.Any(Input.GetKeyDown))
            {
                slot.player.exams_knowledge[KNOW_IDX] += comboGain;
                GenerateCombo(i); s.comboReady = false; UpdateHud(i);
            }
        }
    }

    /* ----- helpers ----- */
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
        l[i].exp.text = $"{r.exp} / {r.divide}";
        l[i].time.text = Time.Time_now;
    }
}
