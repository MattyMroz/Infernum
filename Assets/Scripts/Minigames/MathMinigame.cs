using TMPro;
using UnityEngine;

public class MathMinigame : BaseMinigame
{
    [System.Serializable]
    public class PlayerSlot
    {
        public Player player;               // obiekt Player
        public GameObject panel;                // panel UI gracza
        public KeyCode exitKey = KeyCode.F;  // klawisz wyjścia
    }

    [Header("Players (P1 = 0, P2 = 1)")]
    [SerializeField] private PlayerSlot[] slots = new PlayerSlot[2];

    [Header("Gameplay")]
    [SerializeField] private int mathGain = 10;
    [SerializeField] private int knowledgeIndex = 0;   // Mathematics

    /* --------- stan jednej sesji --------- */
    private class Session
    {
        public bool active;
        public PlayerSlot slot;

        // UI
        public TextMeshProUGUI displayNumber;
        public TMP_InputField inputField;
        public TextMeshProUGUI playerName, day, lvl, exp, time, name;

        // Gameplay
        public int randomNumber;
    }

    private readonly Session[] sessions = { new Session(), new Session() };

    private void Awake()
    {
        displayName = "Matematyka";
    }

    /* --------- wejście w minigrę --------- */
    public override void React(GameObject playerGO)
    {
        for (int i = 0; i < slots.Length; i++)
            if (playerGO == slots[i].player.gameObject && !sessions[i].active)
            {
                StartSession(i);
                return;
            }
    }

    /* --------- start / koniec --------- */
    private void StartSession(int id)
    {
        var s = sessions[id];
        s.active = true;
        s.slot = slots[id];

        // ← najważniejsze – ręczne odpalenie Boot()
        Boot(s.slot.player.gameObject, s.slot.player.GetConfig(MinigameID.Math));

        BindUI(s);
        SetupInput(s);
        GenerateNumber(s);
        UpdateHud(s);
    }


    private void EndSession(int id)
    {
        var s = sessions[id];
        s.active = false;
        s.slot.panel.SetActive(false);

        ToggleMovement(s.slot.player, false);
        TogglePlayerHud(s.slot.player, true);
    }

    /* --------- UNITY UPDATE --------- */
    protected override void Update()       // <- override, nie ukrywa bazy
    {
        for (int i = 0; i < sessions.Length; i++)
        {
            var s = sessions[i];
            if (!s.active) continue;

        

            UpdateHud(s);

            if (Input.GetKeyDown(s.slot.exitKey)) { EndSession(i); continue; }

            MaintainCaret(s);
            UpdateHud(s);      // odświeżamy co klatkę, by HUD był aktualny
        }
    }

    /* --------- Helpers --------- */
    private void BindUI(Session s)
    {
        Transform t = s.slot.panel.transform;
        s.day = t.Find("Day").GetComponent<TextMeshProUGUI>();
        s.playerName = t.Find("PlayerName").GetComponent<TextMeshProUGUI>();
        s.displayNumber = t.Find("DisplayNumber").GetComponent<TextMeshProUGUI>();
        s.inputField = t.Find("InputNumber").GetComponent<TMP_InputField>();
        s.lvl = t.Find("DisplayLvl").GetComponent<TextMeshProUGUI>();
        s.exp = t.Find("DisplayExp").GetComponent<TextMeshProUGUI>();
        s.time = t.Find("Time").GetComponent<TextMeshProUGUI>();
        s.name = t.Find("MinigameName").GetComponent<TextMeshProUGUI>();
    }

    private void SetupInput(Session s)
    {
        s.name.text = "Matematyka";
        s.playerName.text = s.slot.player.player_name;
        s.day.text = $"Dzień: {Time.Days}";

        s.inputField.contentType = TMP_InputField.ContentType.IntegerNumber;
        s.inputField.characterValidation = TMP_InputField.CharacterValidation.Digit;
        s.inputField.onValidateInput = OnlyDigits;
        s.inputField.onEndEdit.RemoveAllListeners();
        s.inputField.onEndEdit.AddListener(str => CheckInput(s, str));
        s.inputField.ActivateInputField();
    }

    private void GenerateNumber(Session s)
    {
        s.randomNumber = Random.Range(1_000_000, 10_000_000);
        s.displayNumber.text = s.randomNumber.ToString();
        s.inputField.text = string.Empty;
    }

    private void CheckInput(Session s, string userInput)
    {
        if (string.IsNullOrWhiteSpace(userInput)) return;

        if (int.TryParse(userInput, out int n) && n == s.randomNumber)
            s.slot.player.exams_knowledge[knowledgeIndex] += mathGain;

        GenerateNumber(s);
        UpdateHud(s);
        s.inputField.ActivateInputField();
    }

    private static char OnlyDigits(string _, int __, char c) =>
        char.IsDigit(c) ? c : '\0';

    private void MaintainCaret(Session s)
    {
        if (!s.inputField.isFocused) return;

        if (Input.GetKeyDown(KeyCode.LeftArrow) ||
            Input.GetKeyDown(KeyCode.RightArrow) ||
            Input.GetKeyDown(KeyCode.UpArrow) ||
            Input.GetKeyDown(KeyCode.DownArrow))
        {
            int len = s.inputField.text.Length;
            s.inputField.caretPosition =
            s.inputField.selectionAnchorPosition =
            s.inputField.selectionFocusPosition = len;
        }
    }

    private void UpdateHud(Session s)
    {
        var res = s.slot.player.LvlIncrease(s.slot.player.exams_knowledge[knowledgeIndex]);
        s.lvl.text = res.lvl.ToString();
        s.exp.text = $"{res.exp} / {res.divide}";
        s.time.text = Time.Time_now;
    }

    /* --------- utility --------- */
    private static void ToggleMovement(Player p, bool freeze)
    {
        var mv = p.GetComponent<Movement>(); if (mv) { if (freeze) mv.ResetVelocity(); mv.enabled = !freeze; }
        var rb = p.GetComponent<Rigidbody2D>(); if (rb) rb.bodyType = freeze ? RigidbodyType2D.Static : RigidbodyType2D.Dynamic;
    }

    private static void TogglePlayerHud(Player p, bool show)
    {
        foreach (var c in p.GetComponentsInChildren<MonoBehaviour>(true))
            if (c.GetType().Name.StartsWith("Display"))
                c.enabled = show;
    }
}
