using TMPro;
using UnityEngine;

public class ProgMinigame : BaseMinigame
{
    [System.Serializable]
    public class PlayerSlot
    {
        public Player player;
        public GameObject panel;
        public KeyCode exitKey;
        public KeyCode actionKey;
    }

    [SerializeField] private PlayerSlot[] slots = new PlayerSlot[2];

    [Header("Gameplay")]
    [SerializeField] private int keyGain = 1;
    private const int KNOW_IDX = 2; // Programming

    /* ----- lokalny ----- */
    private class Local
    {
        public bool active;
        public TextMeshProUGUI lvl, exp, time;
    }
    private readonly Local[] l = { new Local(), new Local() };
    private int current;

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
        Boot(slot.player.gameObject, slot.player.GetConfig(MinigameID.Prog));

        var t = slot.panel.transform;
        l[i].lvl = t.Find("DisplayLvl").GetComponent<TextMeshProUGUI>();
        l[i].exp = t.Find("DisplayExp").GetComponent<TextMeshProUGUI>();
        l[i].time = t.Find("Time").GetComponent<TextMeshProUGUI>();

        l[i].active = true; UpdateHud(i);
    }

    private void EndSession(int i) { if (l[i].active) { current = i; Close(); } }
    protected override void OnClose() { l[current].active = false; }

    protected override void Update()
    {
        base.Update();

        for (int i = 0; i < slots.Length; i++)
        {
            if (!l[i].active) continue;

            if (Input.GetKeyDown(slots[i].exitKey)) { EndSession(i); continue; }

            if (Input.GetKeyDown(slots[i].actionKey))
            {
                slots[i].player.exams_knowledge[KNOW_IDX] += keyGain;
                UpdateHud(i);
            }
        }
    }

    private void UpdateHud(int i)
    {
        var p = slots[i].player;
        var r = p.LvlIncrease(p.exams_knowledge[KNOW_IDX]);
        l[i].lvl.text = r.lvl.ToString();
        l[i].exp.text = $"{r.exp}/{r.divide}";
        l[i].time.text = Time.Time_now;
    }
}
