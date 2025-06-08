using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InfMinigame : BaseMinigame
{
    [System.Serializable]
    public class PlayerSlot
    {
        public Player player;
        public GameObject panel;
        public KeyCode exitKey;
        public KeyCode actionKey;
    }

    [Header("Players")]
    [SerializeField] private PlayerSlot[] slots = new PlayerSlot[2];

    [Header("Gameplay")]
    [SerializeField] private float liftForce = 1800f;
    [SerializeField] private float gravity = 900f;
    [SerializeField] private float zoneSpeed = 100f;
    [SerializeField] private int expPerSec = 5;
    [SerializeField] private int knowledgeId = 1;   // IT

    private class Session
    {
        public bool active;
        public PlayerSlot slot;
        public TextMeshProUGUI lvl, exp, time, name;
        public RectTransform playArea;
        public Image notPlayArea, ball;
        public Vector2 velocity;
        public float secTimer;
        public int zoneDir = -1;
    }

    private readonly Session[] sessions = { new Session(), new Session() };

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
        CenterBall(s);
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

    private void BindUI(Session s)
    {
        var t = s.slot.panel.transform;
        s.lvl = t.Find("DisplayLvl").GetComponent<TextMeshProUGUI>();
        s.exp = t.Find("DisplayExp").GetComponent<TextMeshProUGUI>();
        s.time = t.Find("Time").GetComponent<TextMeshProUGUI>();
        s.name = t.Find("MinigameName").GetComponent<TextMeshProUGUI>();
        s.playArea = t.Find("NotPlayArea/PlayArea").GetComponent<RectTransform>();
        s.notPlayArea = t.Find("NotPlayArea").GetComponent<Image>();
        s.ball = t.Find("NotPlayArea/Ball").GetComponent<Image>();
    }

    protected override void Update()          // ← override
    {
        for (int i = 0; i < sessions.Length; i++)
        {
            var s = sessions[i];
            if (!s.active) continue;

         

            UpdateHud(s);

            if (Input.GetKeyDown(s.slot.exitKey)) {
                EndSession(i);
                continue; 
            }

            TickSession(s);
        }
    }

    private void TickSession(Session s)
    {
        if (Input.GetKey(s.slot.actionKey))
            s.velocity.y += liftForce * UnityEngine.Time.deltaTime;

        s.velocity.y -= gravity * UnityEngine.Time.deltaTime;
        s.velocity.y = Mathf.Clamp(s.velocity.y, -200f, 200f);
        s.ball.rectTransform.anchoredPosition += s.velocity * UnityEngine.Time.deltaTime;

        float yMin = s.notPlayArea.rectTransform.rect.yMin + s.ball.rectTransform.rect.height * 0.5f;
        float yMax = s.notPlayArea.rectTransform.rect.yMax - s.ball.rectTransform.rect.height * 0.5f;
        var pos = s.ball.rectTransform.anchoredPosition;
        pos.y = Mathf.Clamp(pos.y, yMin, yMax);
        s.ball.rectTransform.anchoredPosition = pos;
        if (pos.y == yMin || pos.y == yMax) s.velocity = Vector2.zero;

        float pMin = s.notPlayArea.rectTransform.rect.yMin + s.playArea.rect.height * 0.5f;
        float pMax = s.notPlayArea.rectTransform.rect.yMax - s.playArea.rect.height * 0.5f;
        s.playArea.anchoredPosition += Vector2.up * zoneSpeed * s.zoneDir * UnityEngine.Time.deltaTime;
        var pa = s.playArea.anchoredPosition;
        pa.y = Mathf.Clamp(pa.y, pMin, pMax);
        s.playArea.anchoredPosition = pa;
        if (pa.y == pMin || pa.y == pMax) s.zoneDir *= -1;

        bool inside = RectTransformUtility.RectangleContainsScreenPoint(
            s.playArea, s.ball.rectTransform.position, null);

        if (inside)
        {
            s.secTimer += UnityEngine.Time.deltaTime;
            if (s.secTimer >= 1f)
            {
                s.slot.player.exams_knowledge[knowledgeId] += expPerSec;
                s.secTimer -= 1f;
                UpdateHud(s);
            }
        }
        else s.secTimer = 0f;
    }

    private void CenterBall(Session s) => s.ball.rectTransform.anchoredPosition = s.playArea.rect.center;

    private void UpdateHud(Session s)
    {
        var res = s.slot.player.LvlIncrease(s.slot.player.exams_knowledge[knowledgeId]);
        s.lvl.text = res.lvl.ToString();
        s.exp.text = $"{res.exp} / {res.divide}";
        s.time.text = Time.Time_now;        
        s.name.text = "Informatyka";
    }

    private static void ToggleMovement(Player p, bool freeze)
    {
        var mv = p.GetComponent<Movement>(); if (mv) {
            if (freeze) mv.ResetVelocity(); 
            mv.enabled = !freeze; 
        }

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
