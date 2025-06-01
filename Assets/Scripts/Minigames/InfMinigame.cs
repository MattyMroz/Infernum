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
    [SerializeField] private PlayerSlot player1;
    [SerializeField] private PlayerSlot player2;

    /* UI refs (bindowane) */
    private TextMeshProUGUI lvlTxt, expTxt, timeTxt, nameTxt;
    private RectTransform playArea;
    private Image notPlayArea, ball;

    /* Gameplay */
    [SerializeField] private float liftForce = 1800f;
    [SerializeField] private float gravity = 900f;
    [SerializeField] private int expPerSec = 5;
    private const int KNOWLEDGE_IDX = 1;

    private PlayerSlot cur;
    private KeyCode flyKey;
    private Vector2 velocity;
    private float secTimer;
    private int zoneDir = -1;

    /* ───── React ───── */
    public override void React(GameObject playerGO)
    {
        cur = playerGO == player1.player.gameObject ? player1 : player2;

        // boot bazowej logiki (blokada ruchu + pokazanie panelu)
        Boot(playerGO, new MinigameConfig
        {
            panel = cur.panel,
            exitKey = cur.exitKey,
            actionKeys = new[] { cur.actionKey }
        });
    }

    /* ───── Boot / Open ───── */
    protected override void OnBoot(MinigameConfig cfg)
    {
        BindUI(cur.panel);
        flyKey = cfg.actionKeys[0];
        nameTxt.text = "Informatyka";
        CenterBall();
        UpdateHUD();
    }

    protected override void OnOpen()
    {
        BindUI(cur.panel);
        CenterBall();
        secTimer = 0f;
    }

    /* ───── Update ───── */
    protected override void Update()
    {
        UpdateHUD();

        base.Update();
        if (!active) return;

        if (Input.GetKey(flyKey))
            velocity.y += liftForce * UnityEngine.Time.deltaTime;

        velocity.y -= gravity * UnityEngine.Time.deltaTime;
        velocity.y = Mathf.Clamp(velocity.y, -200f, 200f);
        ball.rectTransform.anchoredPosition += velocity * UnityEngine.Time.deltaTime;

        float yMin = notPlayArea.rectTransform.rect.yMin + ball.rectTransform.rect.height / 2f;
        float yMax = notPlayArea.rectTransform.rect.yMax - ball.rectTransform.rect.height / 2f;
        var pos = ball.rectTransform.anchoredPosition;
        pos.y = Mathf.Clamp(pos.y, yMin, yMax);
        ball.rectTransform.anchoredPosition = pos;
        if (pos.y == yMin || pos.y == yMax) velocity = Vector2.zero;

        float pMin = notPlayArea.rectTransform.rect.yMin + playArea.rect.height / 2f;
        float pMax = notPlayArea.rectTransform.rect.yMax - playArea.rect.height / 2f;
        playArea.anchoredPosition += new Vector2(0f, 100f) * zoneDir * UnityEngine.Time.deltaTime;
        var p = playArea.anchoredPosition;
        p.y = Mathf.Clamp(p.y, pMin, pMax);
        playArea.anchoredPosition = p;
        if (p.y == pMin || p.y == pMax) zoneDir *= -1;

        bool inside = RectTransformUtility.RectangleContainsScreenPoint(
            playArea, ball.rectTransform.position, null);

        if (inside)
        {
            secTimer += UnityEngine.Time.deltaTime;
            if (secTimer >= 1f)
            {
                player.exams_knowledge[KNOWLEDGE_IDX] += expPerSec;
                secTimer -= 1f;
                UpdateHUD();
            }
        }
        else secTimer = 0f;
    }

    /* ───── helpers ───── */
    private void BindUI(GameObject p)
    {
        lvlTxt = p.transform.Find("DisplayLvl").GetComponent<TextMeshProUGUI>();
        expTxt = p.transform.Find("DisplayExp").GetComponent<TextMeshProUGUI>();
        timeTxt = p.transform.Find("Time").GetComponent<TextMeshProUGUI>();
        nameTxt = p.transform.Find("MinigameName").GetComponent<TextMeshProUGUI>();
        playArea = p.transform.Find("NotPlayArea/PlayArea").GetComponent<RectTransform>();
        notPlayArea = p.transform.Find("NotPlayArea").GetComponent<Image>();
        ball = p.transform.Find("NotPlayArea/Ball").GetComponent<Image>();
    }

    private void CenterBall()
    {
        ball.rectTransform.anchoredPosition = playArea.rect.center;
        velocity = Vector2.zero;
    }

    private void UpdateHUD()
    {
        var r = player.LvlIncrease(player.exams_knowledge[KNOWLEDGE_IDX]);
        lvlTxt.text = r.lvl.ToString();
        expTxt.text = $"{r.exp} / {r.divide}";
        timeTxt.text = Time.Time_now;
    }
}
