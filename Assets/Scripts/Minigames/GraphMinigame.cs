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
        public KeyCode[] keys = new KeyCode[10];   // 10 klawiszy
    }

    [Header("Players")]
    [SerializeField] private PlayerSlot player1;
    [SerializeField] private PlayerSlot player2;

    private TextMeshProUGUI keysTxt, lvlTxt, expTxt, timeTxt, nameTxt;

    [Header("Gameplay")]
    [SerializeField] private int comboGain = 15;
    [SerializeField] private int comboLength = 3;
    private const int KNOWLEDGE_IDX = 3;

    private PlayerSlot cur;
    private KeyCode[] allowedKeys;
    private KeyCode[] currentCombo;

    /* ───── React ───── */
    public override void React(GameObject playerGO)
    {
        cur = playerGO == player1.player.gameObject ? player1 : player2;

        // boot bazowej logiki (blokada ruchu + pokazanie panelu)
        Boot(playerGO, new MinigameConfig
        {
            panel = cur.panel,
            exitKey = cur.exitKey,
            actionKeys = cur.keys
        });
    }

    /* ───── Boot / Open ───── */
    protected override void OnBoot(MinigameConfig cfg)
    {
        BindUI(cur.panel);
        allowedKeys = cfg.actionKeys;
        nameTxt.text = "Grafika";
        GenerateCombo();
        UpdateHUD();
    }

    protected override void OnOpen()
    {
        BindUI(cur.panel);
        GenerateCombo();
    }

    /* ───── Update ───── */
    protected override void Update()
    {
        base.Update();
        if (!active || currentCombo == null) return;

        bool allPressed = currentCombo.All(Input.GetKey);
        bool anyPressedNow = currentCombo.Any(Input.GetKeyDown);

        if (allPressed && anyPressedNow)
        {
            player.exams_knowledge[KNOWLEDGE_IDX] += comboGain;
            UpdateHUD();
            GenerateCombo();
        }
    }

    /* ───── helpers ───── */
    private void BindUI(GameObject p)
    {
        keysTxt = p.transform.Find("DisplayKeys").GetComponent<TextMeshProUGUI>();
        lvlTxt = p.transform.Find("DisplayLvl").GetComponent<TextMeshProUGUI>();
        expTxt = p.transform.Find("DisplayExp").GetComponent<TextMeshProUGUI>();
        timeTxt = p.transform.Find("Time").GetComponent<TextMeshProUGUI>();
        nameTxt = p.transform.Find("MinigameName").GetComponent<TextMeshProUGUI>();
    }

    private void GenerateCombo()
    {
        currentCombo = allowedKeys
            .OrderBy(_ => Random.value)
            .Take(comboLength)
            .ToArray();

        keysTxt.text = string.Join(" + ",
            currentCombo.Select(k => k.ToString().Replace("Alpha", "").Replace("Keypad", "")));
    }

    private void UpdateHUD()
    {
        var r = player.LvlIncrease(player.exams_knowledge[KNOWLEDGE_IDX]);
        lvlTxt.text = r.lvl.ToString();
        expTxt.text = $"{r.exp} / {r.divide}";
        timeTxt.text = Time.Time_now;
    }
}
