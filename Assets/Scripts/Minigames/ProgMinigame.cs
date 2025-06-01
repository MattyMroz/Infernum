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

    [Header("Players")]
    [SerializeField] private PlayerSlot player1;
    [SerializeField] private PlayerSlot player2;

    private TextMeshProUGUI lvlTxt, expTxt, timeTxt, nameTxt;

    [Header("Gameplay")]
    [SerializeField] private int keyGain = 1;
    private const int KNOWLEDGE_IDX = 2;

    private PlayerSlot cur;
    private KeyCode clickKey;

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
        clickKey = cfg.actionKeys[0];
        nameTxt.text = "Programowanie";
        UpdateHUD();
    }

    protected override void OnOpen() => BindUI(cur.panel);

    /* ───── Update ───── */
    protected override void Update()
    {
        base.Update();
        if (!active) return;

        if (Input.GetKeyDown(clickKey))
        {
            player.exams_knowledge[KNOWLEDGE_IDX] += keyGain;
            UpdateHUD();
        }
    }

    /* ───── UI helpers ───── */
    private void BindUI(GameObject p)
    {
        lvlTxt = p.transform.Find("DisplayLvl").GetComponent<TextMeshProUGUI>();
        expTxt = p.transform.Find("DisplayExp").GetComponent<TextMeshProUGUI>();
        timeTxt = p.transform.Find("Time").GetComponent<TextMeshProUGUI>();
        nameTxt = p.transform.Find("MinigameName").GetComponent<TextMeshProUGUI>();
    }

    private void UpdateHUD()
    {
        var r = player.LvlIncrease(player.exams_knowledge[KNOWLEDGE_IDX]);
        lvlTxt.text = r.lvl.ToString();
        expTxt.text = $"{r.exp} / {r.divide}";
        timeTxt.text = Time.Time_now;
    }
}
