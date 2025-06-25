/* ---------------- ElekMinigame ---------------- */
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ElekMinigame : BaseMinigame
{
    [System.Serializable] public class PlayerSlot { public Player player; public GameObject panel; public KeyCode exitKey; public KeyCode actionKey; }

    [SerializeField] private PlayerSlot[] slots = new PlayerSlot[2];
    [SerializeField] private int baseGain = 25;
    [SerializeField] private float minWait = 1f, maxWait = 3f, nextDelay = .5f;
    private const int KNOW_IDX = 4;

    private class Local
    {
        public bool active, ready, reacted;
        public Image ind;
        public TextMeshProUGUI lbl, lvl, exp, time, day, name;   // ← ➊ DODANE „day”
        public Coroutine co;
        public float go;
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

        Boot(slot.player.gameObject, slot.player.GetConfig(MinigameID.Elek));

        var t = slot.panel.transform;
        loc.name = t.Find("PlayerName").GetComponent<TextMeshProUGUI>();
        loc.ind = t.Find("Image").GetComponent<Image>();
        loc.lbl = t.Find("DisplayKeys").GetComponent<TextMeshProUGUI>();
        loc.lvl = t.Find("DisplayLvl").GetComponent<TextMeshProUGUI>();
        loc.exp = t.Find("DisplayExp").GetComponent<TextMeshProUGUI>();
        loc.time = t.Find("Time").GetComponent<TextMeshProUGUI>();
        loc.day = t.Find("Day").GetComponent<TextMeshProUGUI>();      

        loc.lbl.text = $"REAGUJ 'Num0'";
        loc.ind.color = Color.red;
        loc.active = true;

        if (loc.co != null) StopCoroutine(loc.co);
        loc.co = StartCoroutine(WaitGo(i));

        UpdateHud(i);
        ToggleUI(slot.player, false);
    }

    private void EndSession(int i) { ToggleUI(slots[i].player, true); if (l[i].active) { cur = i; Close(); } }
    protected override void OnClose() { if (l[cur].co != null) StopCoroutine(l[cur].co); l[cur].active = false; }

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

            if (Input.GetKeyDown(slot.actionKey))
            {
                if (loc.ready && !loc.reacted)
                {
                    int gain = Mathf.Max(1, baseGain - Mathf.RoundToInt((UnityEngine.Time.time - loc.go) * baseGain));
                    slot.player.exams_knowledge[KNOW_IDX] += gain;
                    loc.reacted = true; 
                    loc.ready = false;
                }
                else if (!loc.ready)
                {
                    if (loc.co != null) StopCoroutine(loc.co);
                    loc.co = StartCoroutine(WaitGo(i));
                }
            }
        }
    }

    private IEnumerator WaitGo(int i)
    {
        var loc = l[i]; loc.ready = loc.reacted = false; loc.ind.color = Color.red;
        yield return new WaitForSeconds(Random.Range(minWait, maxWait));
        loc.go = UnityEngine.Time.time; loc.ready = true; loc.ind.color = Color.green;
        yield return new WaitForSeconds(nextDelay);
        loc.co = StartCoroutine(WaitGo(i));
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
