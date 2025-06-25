using TMPro;
using UnityEngine;

public class MathMinigame : BaseMinigame
{
    [System.Serializable]
    public class PlayerSlot
    {
        public Player player;
        public GameObject panel;
        public KeyCode exitKey = KeyCode.F;
    }

    [Header("Players (P1 = 0, P2 = 1)")]
    [SerializeField] private PlayerSlot[] slots = new PlayerSlot[2];

    [Header("Gameplay")]
    [SerializeField] private int mathGain = 10;
    private const int KNOWLEDGE_IDX = 0; // Mathematics

    // Stan lokalny jednego gracza
    private class Local
    {
        public bool active;
        public TMP_InputField input;
        public TextMeshProUGUI numTxt, lvl, exp, time, day, name;
        public int random;
        public PlayerSlot slot;
    }

    private readonly Local[] l = { new Local(), new Local() };

    private int currentIdx = -1;

    // React - wejście w minigrę
    public override void React(GameObject playerGO)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (playerGO == slots[i].player.gameObject && !l[i].active)
            {
                StartSession(i);
                return;
            }
        }
    }

    // Start sesji
    private void StartSession(int i)
    {
        currentIdx = i;
        var s = l[i];
        s.slot = slots[i];
        s.active = true;

        // Uruchamiamy logikę z BaseMinigame (blok ruchu + stamina + zamykanie panelem/klawiszem)
        Boot(s.slot.player.gameObject, s.slot.player.GetConfig(MinigameID.Math));

        // Podpinamy UI tego gracza
        var t = s.slot.panel.transform;
        s.name = t.Find("PlayerName").GetComponent<TextMeshProUGUI>();
        s.input = t.Find("InputNumber").GetComponent<TMP_InputField>();
        s.numTxt = t.Find("DisplayNumber").GetComponent<TextMeshProUGUI>();
        s.lvl = t.Find("DisplayLvl").GetComponent<TextMeshProUGUI>();
        s.exp = t.Find("DisplayExp").GetComponent<TextMeshProUGUI>();
        s.time = t.Find("Time").GetComponent<TextMeshProUGUI>();
        s.day = t.Find("Day").GetComponent<TextMeshProUGUI>();

        SetupInput(i);
        NewNumber(i);
        UpdateHud(i);

        // Wywołanie ToggleUI z odpowiednimi argumentami
        ToggleUI(s.slot.player, false); // Wyłącz UI gracza
    }

    private void EndSession(int i)
    {
        if (!l[i].active) return;
        l[i].active = false;
        l[i].slot.panel.SetActive(false);

        // Ponowne włączenie UI gracza
        ToggleUI(l[i].slot.player, true); // Włącz UI gracza
    }





    // Update
    protected override void Update()
    {
        base.Update();                  

        for (int i = 0; i < l.Length; i++)
            if (l[i].active)
            {
                MaintainCaret(i);
                UpdateHud(i);          
            }
    }

    // Setup input dla minigry
    private void SetupInput(int i)
    {
        var inp = l[i].input;
        inp.contentType = TMP_InputField.ContentType.IntegerNumber;
        inp.characterValidation = TMP_InputField.CharacterValidation.Digit;
        inp.onValidateInput = (_, __, c) => char.IsDigit(c) ? c : '\0';
        inp.onEndEdit.RemoveAllListeners();
        inp.onEndEdit.AddListener(str => CheckAnswer(i, str));
        inp.ActivateInputField();
    }

    // Nowa liczba
    private void NewNumber(int i)
    {
        l[i].random = Random.Range(1_000_000, 10_000_000);
        l[i].numTxt.text = l[i].random.ToString();
        l[i].input.text = "";
    }

    // Sprawdzenie odpowiedzi
    private void CheckAnswer(int i, string s)
    {
        if (int.TryParse(s, out int n) && n == l[i].random)
            l[i].slot.player.exams_knowledge[KNOWLEDGE_IDX] += mathGain;

        NewNumber(i);
        UpdateHud(i);
        l[i].input.ActivateInputField();
    }

    // Utrzymanie kursora
    private void MaintainCaret(int i)
    {
        var inp = l[i].input;
        if (!inp.isFocused) return;
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow) ||
            Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            int len = inp.text.Length;
            inp.caretPosition =
            inp.selectionAnchorPosition =
            inp.selectionFocusPosition = len;
        }
    }

    // Aktualizacja HUD
    private void UpdateHud(int i)
    {
        var p = l[i].slot.player;
        var res = p.LvlIncrease(p.exams_knowledge[KNOWLEDGE_IDX]);

        l[i].name.text = p.name;
        l[i].lvl.text = res.lvl.ToString();
        l[i].exp.text = $"{res.exp} / {res.divide}";
        l[i].time.text = Time.Time_now;
        l[i].day.text = $"Dzień: {Time.Days + 1}";
    }

    // Hook z base minigame
    protected override void OnClose()
    {
        if (currentIdx >= 0) l[currentIdx].active = false;
    }
}
