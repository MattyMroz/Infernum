using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

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

    [Header("Players")]
    [SerializeField] private PlayerSlot player1;
    [SerializeField] private PlayerSlot player2;

    private Image indicator;
    private TextMeshProUGUI label, lvlTxt, expTxt, timeTxt, nameTxt;

    [Header("Gameplay")]
    [SerializeField] private int baseGain = 25;
    [SerializeField] private float minWait = 1f;
    [SerializeField] private float maxWait = 3f;
    private const int KNOWLEDGE_IDX = 4;

    private PlayerSlot cur;
    private KeyCode reactKey;
    private bool ready;
    private float goTime;

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
        reactKey = cfg.actionKeys[0];
        nameTxt.text = "Elektrotechnika";
        label.text = $"REAGUJ '{reactKey}'";
        indicator.color = Color.red;
        UpdateHUD();
        StartCoroutine(WaitAndGo());
    }

    protected override void OnOpen()
    {
        BindUI(cur.panel);
        indicator.color = Color.red;
        label.text = $"REAGUJ '{reactKey}'";
        StopAllCoroutines();
        StartCoroutine(WaitAndGo());
    }

    /* ───── Update ───── */
    protected override void Update()
    {
        if (player == null)
            return;

        UpdateHUD();

        base.Update();
        if (!active) return;

        if (Input.GetKeyDown(reactKey))
        {
            if (ready)
            {
                float reaction = UnityEngine.Time.time - goTime;
                int gain = Mathf.Max(1, baseGain - Mathf.RoundToInt(reaction * baseGain));
                player.exams_knowledge[KNOWLEDGE_IDX] += gain;
                UpdateHUD();
                StartCoroutine(WaitAndGo());
            }
            else
            {
                StopAllCoroutines();
                StartCoroutine(WaitAndGo());
            }
        }
    }

    /* ───── coroutine ───── */
    private IEnumerator WaitAndGo()
    {
        ready = false;
        indicator.color = Color.red;
        yield return new WaitForSeconds(Random.Range(minWait, maxWait));

        goTime = UnityEngine.Time.time;
        ready = true;
        indicator.color = Color.green;
    }

    /* ───── helpers ───── */
    private void BindUI(GameObject p)
    {
        indicator = p.transform.Find("Image").GetComponent<Image>();
        label = p.transform.Find("DisplayKeys").GetComponent<TextMeshProUGUI>();
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
