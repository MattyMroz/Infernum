/* ---------------- InfMinigame ---------------- */
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InfMinigame : BaseMinigame
{
    [System.Serializable] public class PlayerSlot { public Player player; public GameObject panel; public KeyCode exitKey; public KeyCode actionKey; }

    [SerializeField] private PlayerSlot[] slots = new PlayerSlot[2];
    [SerializeField] private float lift = 1800f, gravity = 900f, zoneSpeed = 100f;
    [SerializeField] private int expPerSec = 5;
    private const int KNOW_IDX = 1;

    private class Local
    {
        public bool active;
        public RectTransform playArea;
        public Image notArea, ball;
        public Vector2 vel;
        public int zoneDir = -1;
        public float secTimer;
        public TextMeshProUGUI lvl, exp, time, day;          // ← ➊
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

        Boot(slot.player.gameObject, slot.player.GetConfig(MinigameID.Inf));

        var t = slot.panel.transform;
        loc.playArea = t.Find("NotPlayArea/PlayArea").GetComponent<RectTransform>();
        loc.notArea = t.Find("NotPlayArea").GetComponent<Image>();
        loc.ball = t.Find("NotPlayArea/Ball").GetComponent<Image>();
        loc.lvl = t.Find("DisplayLvl").GetComponent<TextMeshProUGUI>();
        loc.exp = t.Find("DisplayExp").GetComponent<TextMeshProUGUI>();
        loc.time = t.Find("Time").GetComponent<TextMeshProUGUI>();
        loc.day = t.Find("Day").GetComponent<TextMeshProUGUI>();      // ← ➋

        loc.ball.rectTransform.anchoredPosition = loc.playArea.rect.center;
        loc.active = true;
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
            if (l[i].active)
            {
                Tick(i);
                UpdateHud(i);
            }
    }

    private void Tick(int i)
    {
        var slot = slots[i]; var s = l[i];
        if (Input.GetKeyDown(slot.exitKey)) { EndSession(i); return; }

        if (Input.GetKey(slot.actionKey)) s.vel.y += lift * UnityEngine.Time.deltaTime;
        s.vel.y -= gravity * UnityEngine.Time.deltaTime;
        s.vel.y = Mathf.Clamp(s.vel.y, -200, 200);
        s.ball.rectTransform.anchoredPosition += s.vel * UnityEngine.Time.deltaTime;

        float yMin = s.notArea.rectTransform.rect.yMin + s.ball.rectTransform.rect.height * .5f;
        float yMax = s.notArea.rectTransform.rect.yMax - s.ball.rectTransform.rect.height * .5f;
        var pos = s.ball.rectTransform.anchoredPosition;
        pos.y = Mathf.Clamp(pos.y, yMin, yMax);
        s.ball.rectTransform.anchoredPosition = pos;
        if (pos.y == yMin || pos.y == yMax) s.vel = Vector2.zero;

        float pMin = s.notArea.rectTransform.rect.yMin + s.playArea.rect.height * .5f;
        float pMax = s.notArea.rectTransform.rect.yMax - s.playArea.rect.height * .5f;
        s.playArea.anchoredPosition += Vector2.up * zoneSpeed * s.zoneDir * UnityEngine.Time.deltaTime;
        var pa = s.playArea.anchoredPosition;
        pa.y = Mathf.Clamp(pa.y, pMin, pMax);
        s.playArea.anchoredPosition = pa;
        if (pa.y == pMin || pa.y == pMax) s.zoneDir *= -1;

        bool inside = RectTransformUtility.RectangleContainsScreenPoint(s.playArea, s.ball.rectTransform.position, null);
        if (inside)
        {
            s.secTimer += UnityEngine.Time.deltaTime;
            if (s.secTimer >= 1f) { 
                slot.player.exams_knowledge[KNOW_IDX] += expPerSec;
                s.secTimer -= 1f; 
            }
        }
        else s.secTimer = 0f;
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
