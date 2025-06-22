using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField] private float nextRoundDelay = .5f;
    private const int KNOW_IDX = 4;                  // Electrotechnics

    /* ---------- stan lokalny ---------- */
    private class Local
    {
        public bool active;
        public Image indicator;
        public TextMeshProUGUI label, lvl, exp, time;
        public Coroutine waitCo;
        public bool ready, reacted;
        public float goTime;
    }
    private readonly Local[] l = { new Local(), new Local() };

    private int current;                             // który slot właśnie otwiera / zamyka

    /* ---------- wejście ---------- */
    public override void React(GameObject go)
    {
        for (int i = 0; i < slots.Length; i++)
            if (go == slots[i].player.gameObject && !l[i].active)
                StartSession(i);
    }

    private void StartSession(int i)
    {
        current = i;
        var s = l[i]; var slot = slots[i];

        Boot(slot.player.gameObject, slot.player.GetConfig(MinigameID.Elek));   // blok ruchu + stamina

        // UI
        var t = slot.panel.transform;
        s.indicator = t.Find("Image").GetComponent<Image>();
        s.label = t.Find("DisplayKeys").GetComponent<TextMeshProUGUI>();
        s.lvl = t.Find("DisplayLvl").GetComponent<TextMeshProUGUI>();
        s.exp = t.Find("DisplayExp").GetComponent<TextMeshProUGUI>();
        s.time = t.Find("Time").GetComponent<TextMeshProUGUI>();

        s.label.text = $"REAGUJ '{slot.actionKey}'";
        s.indicator.color = Color.red;
        s.active = true;

        if (s.waitCo != null) StopCoroutine(s.waitCo);
        s.waitCo = StartCoroutine(WaitAndGo(i));

        UpdateHud(i);
    }

    private void EndSession(int i)
    {
        if (!l[i].active) return;
        current = i;
        Close();                       // wywołuje OnClose → flagi + zatrzymanie korutyny
    }

    /* ---------- hooks ---------- */
    protected override void OnClose()
    {
        var s = l[current];
        if (s.waitCo != null) { StopCoroutine(s.waitCo); s.waitCo = null; }
        s.active = false;
    }

    /* ---------- Update ---------- */
    protected override void Update()
    {
        base.Update();                 // exitKey & panel.SetActive

        for (int i = 0; i < slots.Length; i++)
        {
            if (!l[i].active) continue;
            var slot = slots[i];
            var s = l[i];

            if (Input.GetKeyDown(slot.exitKey)) { EndSession(i); continue; }

            if (Input.GetKeyDown(slot.actionKey))
            {
                if (s.ready && !s.reacted)
                {
                    float rt = UnityEngine.Time.time - s.goTime;
                    int gain = Mathf.Max(1, baseGain - Mathf.RoundToInt(rt * baseGain));
                    slot.player.exams_knowledge[KNOW_IDX] += gain;
                    s.reacted = true; s.ready = false;
                    UpdateHud(i);
                }
                else if (!s.ready)
                {
                    if (s.waitCo != null) StopCoroutine(s.waitCo);
                    s.waitCo = StartCoroutine(WaitAndGo(i));
                }
            }
        }
    }

    /* ---------- corutyna ---------- */
    private IEnumerator WaitAndGo(int i)
    {
        var s = l[i]; s.ready = s.reacted = false; s.indicator.color = Color.red;
        yield return new WaitForSeconds(Random.Range(minWait, maxWait));

        s.goTime = UnityEngine.Time.time; s.ready = true; s.indicator.color = Color.green;
        yield return new WaitForSeconds(nextRoundDelay);
        s.waitCo = StartCoroutine(WaitAndGo(i));          // kolejna runda
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
